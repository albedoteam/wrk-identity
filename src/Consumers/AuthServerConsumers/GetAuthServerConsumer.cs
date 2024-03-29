﻿namespace Identity.Business.Consumers.AuthServerConsumers
{
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;

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
            if (!context.Message.AccountId.IsValidObjectId())
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The Account ID does not have a valid ObjectId format"
                });
                return;
            }

            if (!context.Message.Id.IsValidObjectId())
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = "The Auth Server ID does not have a valid ObjectId format"
                });
                return;
            }

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