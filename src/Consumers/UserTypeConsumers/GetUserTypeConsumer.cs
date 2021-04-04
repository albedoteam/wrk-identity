namespace Identity.Business.Consumers.UserTypeConsumers
{
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;

    public class GetUserTypeConsumer : IConsumer<GetUserType>
    {
        private readonly IUserTypeMapper _mapper;
        private readonly IUserTypeRepository _repository;

        public GetUserTypeConsumer(
            IUserTypeMapper mapper,
            IUserTypeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<GetUserType> context)
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
                    ErrorMessage = "The UserType ID does not have a valid ObjectId format"
                });
                return;
            }

            var userType = await _repository.FindById(
                context.Message.AccountId,
                context.Message.Id,
                context.Message.ShowDeleted);

            if (userType is null)
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "UserType not found"
                });
            else
                await context.RespondAsync(_mapper.MapModelToResponse(userType));
        }
    }
}