using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Services.Accounts;
using Identity.Business.Services.IdentityServers.Abstractions;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class DeleteUserTypeConsumer : IConsumer<DeleteUserType>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IUserTypeMapper _mapper;
        private readonly IUserTypeRepository _repository;

        public DeleteUserTypeConsumer(
            IAccountService accountService,
            IIdentityServerService identityServer,
            IUserTypeMapper mapper,
            IUserTypeRepository repository)
        {
            _accountService = accountService;
            _identityServer = identityServer;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<DeleteUserType> context)
        {
            if (!context.Message.Id.IsValidObjectId())
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The UserType ID does not have a valid ObjectId format"
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

            var userType = await _repository.FindById(context.Message.AccountId, context.Message.Id);
            if (userType is null)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = $"UserType not found for id {context.Message.Id}"
                });
                return;
            }

            await _identityServer
                .GroupProvider(userType.Provider)
                .Delete(userType.ProviderId);

            await _repository.DeleteById(context.Message.AccountId, context.Message.Id);

            // get "soft-deleted"
            userType = await _repository.FindById(context.Message.AccountId, context.Message.Id, true);

            await context.RespondAsync(_mapper.MapModelToResponse(userType));
        }
    }
}