namespace Identity.Business.Consumers.GroupConsumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Services.Accounts;
    using Services.IdentityServers.Abstractions;

    public class CreateGroupConsumer : IConsumer<CreateGroup>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IGroupMapper _mapper;
        private readonly IGroupRepository _repository;

        public CreateGroupConsumer(
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

        public async Task Consume(ConsumeContext<CreateGroup> context)
        {
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

            var exists = (await _repository.FilterBy(context.Message.AccountId,
                a => a.Name.Equals(context.Message.Name) && !a.IsDeleted)).Any();

            if (exists)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.AlreadyExists,
                    ErrorMessage = "Group already exists"
                });
                return;
            }

            var model = _mapper.RequestToModel(context.Message);

            // adjust group name 
            var accountName = account.Name.Replace(" ", "-").ToLower();
            var groupNameOnProvider = $"{accountName}-{model.Name}";

            var groupProviderId = await _identityServer
                .GroupProvider(context.Message.Provider)
                .Create(groupNameOnProvider, context.Message.Description);

            if (string.IsNullOrWhiteSpace(groupProviderId))
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InternalServerError,
                    ErrorMessage = "It was not possible to create Group on Provider"
                });
                return;
            }

            model.ProviderId = groupProviderId;

            var group = await _repository.InsertOne(model);
            await context.RespondAsync(_mapper.MapModelToResponse(group));
        }
    }
}