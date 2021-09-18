namespace Identity.Business.Consumers.GroupConsumers
{
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Events;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Services.Accounts;
    using Services.IdentityServers.Abstractions;

    public class DeleteGroupConsumer : IConsumer<DeleteGroup>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IGroupMapper _mapper;
        private readonly IGroupRepository _repository;

        public DeleteGroupConsumer(
            IIdentityServerService identityServer,
            IGroupMapper mapper,
            IGroupRepository repository,
            IAccountService accountService)
        {
            _identityServer = identityServer;
            _mapper = mapper;
            _repository = repository;
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<DeleteGroup> context)
        {
            if (!context.Message.Id.IsValidObjectId())
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The group ID does not have a valid ObjectId format"
                });
                return;
            }

            var account = await _accountService.GetAccount(context.Message.AccountId);
            var isAccountValid = account is { } && account.Enabled;
            if (!isAccountValid)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = $"Account invalid for id {context.Message.AccountId}"
                });
                return;
            }

            var group = await _repository.FindById(context.Message.AccountId, context.Message.Id);
            if (group is null)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = $"Group not found for id {context.Message.Id}"
                });
                return;
            }

            await _identityServer
                .GroupProvider(group.Provider)
                .Delete(group.ProviderId);

            await _repository.DeleteById(context.Message.AccountId, context.Message.Id);

            // get "soft-deleted"
            group = await _repository.FindById(context.Message.AccountId, context.Message.Id, true);

            await context.Publish<GroupDeleted>(new
            {
                context.Message.AccountId,
                context.Message.Id,
                group.DeletedAt
            });

            await context.RespondAsync(_mapper.MapModelToResponse(group));
        }
    }
}