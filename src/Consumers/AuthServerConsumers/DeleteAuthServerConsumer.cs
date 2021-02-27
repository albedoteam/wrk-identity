using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Services.IdentityServers;
using MassTransit;

namespace Identity.Business.Consumers.AuthServerConsumers
{
    public class DeleteAuthServerConsumer : IConsumer<DeleteAuthServer>
    {
        private readonly IIdentityServerService _identityServer;
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public DeleteAuthServerConsumer(
            IAuthServerMapper mapper,
            IAuthServerRepository repository,
            IIdentityServerService identityServer)
        {
            _mapper = mapper;
            _repository = repository;
            _identityServer = identityServer;
        }

        public async Task Consume(ConsumeContext<DeleteAuthServer> context)
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

            var isDeleted = await _identityServer
                .AuthServerProvider(authServer.Provider)
                .Delete(authServer.ProviderId);

            if (!isDeleted)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InternalServerError,
                    ErrorMessage = "Cannot delete Auth Server on Provider"
                });
                return;
            }

            await _repository.DeleteById(context.Message.AccountId, context.Message.Id);

            // get "soft-deleted"
            authServer = await _repository.FindById(context.Message.AccountId, context.Message.Id, true);

            await context.RespondAsync(_mapper.MapModelToResponse(authServer)); // respond async
        }
    }
}