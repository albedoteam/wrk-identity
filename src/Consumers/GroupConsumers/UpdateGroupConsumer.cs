using System.Linq;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;
using Identity.Business.Services.Accounts;
using Identity.Business.Services.IdentityServers.Abstractions;
using MassTransit;
using MongoDB.Driver;

namespace Identity.Business.Consumers.GroupConsumers
{
    public class UpdateGroupConsumer : IConsumer<UpdateGroup>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IGroupMapper _mapper;
        private readonly IGroupRepository _repository;

        public UpdateGroupConsumer(
            IAccountService accountService,
            IIdentityServerService identityServer,
            IGroupMapper mapper,
            IGroupRepository repository)
        {
            _accountService = accountService;
            _identityServer = identityServer;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateGroup> context)
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

            var exists = (await _repository.FilterBy(
                context.Message.AccountId,
                g => g.Name.Equals(context.Message.Name))).Any();

            if (exists)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.AlreadyExists,
                    ErrorMessage = $"Group with name {context.Message.Name} already exists"
                });
                return;
            }

            // adjust group name 
            var accountName = account.Name.Replace(" ", "-").ToLower();
            var newName = $"{accountName}-{context.Message.Name}";

            var updated = await _identityServer
                .GroupProvider(group.Provider)
                .Update(group.ProviderId, newName, context.Message.Description);

            if (!updated)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "Group update failed at Provider"
                });
                return;
            }

            var updateDefinition = Builders<Group>.Update.Combine(
                Builders<Group>.Update.Set(g => g.Name, newName),
                Builders<Group>.Update.Set(g => g.Description, context.Message.Description),
                Builders<Group>.Update.Set(g => g.DisplayName, context.Message.DisplayName),
                Builders<Group>.Update.Set(g => g.IsDefault, context.Message.IsDefault)
            );

            await _repository.UpdateById(context.Message.AccountId, context.Message.Id, updateDefinition);

            // get updated one
            group = await _repository.FindById(context.Message.AccountId, context.Message.Id);

            await context.RespondAsync(_mapper.MapModelToResponse(group));
        }
    }
}