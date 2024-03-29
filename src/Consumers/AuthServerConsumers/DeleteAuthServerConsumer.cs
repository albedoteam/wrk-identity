﻿namespace Identity.Business.Consumers.AuthServerConsumers
{
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Services.Accounts;
    using Services.IdentityServers.Abstractions;

    public class DeleteAuthServerConsumer : IConsumer<DeleteAuthServer>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public DeleteAuthServerConsumer(
            IAuthServerMapper mapper,
            IAuthServerRepository repository,
            IIdentityServerService identityServer,
            IAccountService accountService)
        {
            _mapper = mapper;
            _repository = repository;
            _identityServer = identityServer;
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<DeleteAuthServer> context)
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

            await _identityServer
                .AuthServerProvider(authServer.Provider)
                .Delete(authServer.ProviderId);

            await _repository.DeleteById(context.Message.AccountId, context.Message.Id);

            // get "soft-deleted"
            authServer = await _repository.FindById(context.Message.AccountId, context.Message.Id, true);

            await context.RespondAsync(_mapper.MapModelToResponse(authServer));
        }
    }
}