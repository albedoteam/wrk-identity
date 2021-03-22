using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;
using Identity.Business.Services.Accounts;
using MassTransit;
using MongoDB.Driver;

namespace Identity.Business.Consumers.AuthServerConsumers
{
    public class UpdateAuthServerConsumer : IConsumer<UpdateAuthServer>
    {
        private readonly IAccountService _accountService;
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public UpdateAuthServerConsumer(
            IAccountService accountService,
            IAuthServerMapper mapper,
            IAuthServerRepository repository)
        {
            _accountService = accountService;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateAuthServer> context)
        {
            if (!context.Message.Id.IsValidObjectId())
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The auth server ID does not have a valid ObjectId format"
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

            var authServer = await _repository.FindById(context.Message.AccountId, context.Message.Id);
            if (authServer is null)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "Auth Server not found"
                });
                return;
            }

            var communicationRules = _mapper.MapRequestToModel(context.Message.CommunicationRules);

            var updateDefinition = Builders<AuthServer>.Update.Combine(
                Builders<AuthServer>.Update.Set(g => g.CommunicationRules, communicationRules)
            );

            await _repository.UpdateById(context.Message.AccountId, context.Message.Id, updateDefinition);

            // get updated one
            authServer = await _repository.FindById(context.Message.AccountId, context.Message.Id);

            await context.RespondAsync(_mapper.MapModelToResponse(authServer));
        }
    }
}