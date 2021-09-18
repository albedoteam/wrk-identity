namespace Identity.Business.Consumers.UserTypeConsumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Models;
    using MongoDB.Driver;
    using Services.Accounts;
    using Services.IdentityServers.Abstractions;

    public class UpdateUserTypeConsumer : IConsumer<UpdateUserType>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IUserTypeMapper _mapper;
        private readonly IUserTypeRepository _repository;

        public UpdateUserTypeConsumer(
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

        public async Task Consume(ConsumeContext<UpdateUserType> context)
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

            var exists = (await _repository.FilterBy(
                context.Message.AccountId,
                g => g.Name.Equals(context.Message.Name))).Any();

            if (exists)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.AlreadyExists,
                    ErrorMessage = $"UserType with name {context.Message.Name} already exists"
                });
                return;
            }

            // adjust usertype name 
            var accountName = account.Name.Replace(" ", "_").ToLower();
            var newName = context.Message.Name.Replace("-", "_").Replace(" ", "_").ToLower();
            var userTypeNameOnProvider = $"{accountName}_{newName}";

            var updated = await _identityServer
                .UserTypeProvider(userType.Provider)
                .Update(userType.ProviderId, userTypeNameOnProvider, context.Message.DisplayName,
                    context.Message.Description);

            if (!updated)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "UserType update failed at Provider"
                });
                return;
            }

            var updateDefinition = Builders<UserType>.Update.Combine(
                Builders<UserType>.Update.Set(g => g.Name, newName),
                Builders<UserType>.Update.Set(g => g.Description, context.Message.Description),
                Builders<UserType>.Update.Set(g => g.DisplayName, context.Message.DisplayName)
            );

            await _repository.UpdateById(context.Message.AccountId, context.Message.Id, updateDefinition);

            // get updated one
            userType = await _repository.FindById(context.Message.AccountId, context.Message.Id);

            await context.RespondAsync(_mapper.MapModelToResponse(userType));
        }
    }
}