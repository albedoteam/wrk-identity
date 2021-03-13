using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using AlbedoTeam.Identity.Contracts.Events;
using Identity.Business.Db.Abstractions;
using Identity.Business.Models;
using Identity.Business.Services.Accounts;
using Identity.Business.Services.IdentityServers.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Identity.Business.Consumers.AuthServerConsumers
{
    public class ActivateAuthServerConsumer : IConsumer<ActivateAuthServer>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly ILogger<ActivateAuthServerConsumer> _logger;
        private readonly IAuthServerRepository _repository;

        public ActivateAuthServerConsumer(
            IAuthServerRepository repository,
            IIdentityServerService identityServer,
            ILogger<ActivateAuthServerConsumer> logger,
            IAccountService accountService)
        {
            _repository = repository;
            _identityServer = identityServer;
            _logger = logger;
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<ActivateAuthServer> context)
        {
            if (!context.Message.Id.IsValidObjectId())
            {
                _logger.LogError("The Auth Server ID does not have a valid ObjectId format {AuthServerId}",
                    context.Message.Id);
                return;
            }

            var account = await _accountService.GetAccount(context.Message.AccountId);
            var isAccountValid = account is { } && account.Enabled;
            if (!isAccountValid)
            {
                _logger.LogError("Account invalid for id {AccountId}", context.Message.AccountId);
                return;
            }

            var authServer = await _repository.FindById(context.Message.AccountId, context.Message.Id);
            if (authServer is null)
            {
                _logger.LogError("Auth Server not found for id {AuthServerId}", context.Message.Id);
                return;
            }

            await _identityServer
                .AuthServerProvider(authServer.Provider)
                .Activate(authServer.ProviderId);

            var update = Builders<AuthServer>.Update.Combine(
                Builders<AuthServer>.Update.Set(a => a.Active, true),
                Builders<AuthServer>.Update.Set(a => a.UpdateReason, context.Message.Reason));

            await _repository.UpdateById(context.Message.AccountId, context.Message.Id, update);

            // notifies
            await context.Publish<AuthServerActivated>(new
            {
                context.Message.AccountId,
                context.Message.Id,
                ActivatedAt = DateTime.UtcNow,
                context.Message.Reason
            });
        }
    }
}