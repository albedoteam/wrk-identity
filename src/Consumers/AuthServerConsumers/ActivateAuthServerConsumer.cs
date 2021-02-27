using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Events;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;
using Identity.Business.Services.IdentityServers;
using MassTransit;
using MongoDB.Driver;

namespace Identity.Business.Consumers.AuthServerConsumers
{
    public class ActivateAuthServerConsumer : IConsumer<ActivateAuthServer>
    {
        private readonly IIdentityServerService _identityServer;
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public ActivateAuthServerConsumer(
            IAuthServerMapper mapper,
            IAuthServerRepository repository,
            IIdentityServerService identityServer)
        {
            _mapper = mapper;
            _repository = repository;
            _identityServer = identityServer;
        }

        public async Task Consume(ConsumeContext<ActivateAuthServer> context)
        {
            if (!context.Message.Id.IsValidObjectId())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The auth server ID does not have a valid ObjectId format"
                });

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

            var isActivated = await _identityServer
                .AuthServerProvider(authServer.Provider)
                .Activate(authServer.ProviderId);

            if (!isActivated)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InternalServerError,
                    ErrorMessage = "Cannot activate Auth Server on Provider"
                });
                return;
            }

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