using System.Linq;
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
    public class CreateUserTypeConsumer : IConsumer<CreateUserType>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IUserTypeMapper _mapper;
        private readonly IUserTypeRepository _repository;

        public CreateUserTypeConsumer(
            IAccountService accountService,
            IIdentityServerService identityServer,
            IUserTypeMapper mapper, IUserTypeRepository repository)
        {
            _accountService = accountService;
            _identityServer = identityServer;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CreateUserType> context)
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
                    ErrorMessage = "UserType already exists"
                });
                return;
            }

            var model = _mapper.RequestToModel(context.Message);

            // adjust usertype name 
            var accountName = account.Name.Replace(" ", "-").ToLower();
            var userTypeNameOnProvider = $"{accountName}-{model.Name}";

            var userTypeProviderId = await _identityServer
                .UserTypeProvider(context.Message.Provider)
                .Create(userTypeNameOnProvider, context.Message.DisplayName, context.Message.Description);

            if (string.IsNullOrWhiteSpace(userTypeProviderId))
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InternalServerError,
                    ErrorMessage = "It was not possible to create UserType on Provider"
                });
                return;
            }

            model.ProviderId = userTypeProviderId;

            var userType = await _repository.InsertOne(model);
            await context.RespondAsync(_mapper.MapModelToResponse(userType));
        }
    }
}