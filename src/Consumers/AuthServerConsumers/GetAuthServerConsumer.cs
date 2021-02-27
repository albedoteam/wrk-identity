using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using MassTransit;

namespace Identity.Business.Consumers.AuthServerConsumers
{
    public class GetAuthServerConsumer : IConsumer<GetAuthServer>
    {
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public GetAuthServerConsumer(IAuthServerMapper mapper, IAuthServerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetAuthServer> context)
        {
            if (!context.Message.Id.IsValidObjectId())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The auth server ID does not have a valid ObjectId format"
                });

            var authServer = await _repository.FindById(
                context.Message.AccountId,
                context.Message.Id,
                context.Message.ShowDeleted);

            if (authServer is null)
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "Auth Server not found"
                });
            else
                await context.RespondAsync(_mapper.MapModelToResponse(authServer));
        }
    }
}