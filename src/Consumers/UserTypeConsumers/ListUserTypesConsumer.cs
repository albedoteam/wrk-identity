namespace Identity.Business.Consumers.UserTypeConsumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Models;

    public class ListUserTypesConsumer : IConsumer<ListUserTypes>
    {
        private readonly IUserTypeMapper _mapper;
        private readonly IUserTypeRepository _repository;

        public ListUserTypesConsumer(IUserTypeMapper mapper, IUserTypeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ListUserTypes> context)
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

            var queryRequest = QueryUtils.GetQueryParams<UserType>(_mapper.RequestToQuery(context.Message));
            var queryResponse = await _repository.QueryByPage(context.Message.AccountId, queryRequest);

            if (!queryResponse.Records.Any())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "UserTypes not found"
                });
            else
                await context.RespondAsync<ListUserTypesResponse>(new
                {
                    queryResponse.Page,
                    queryResponse.PageSize,
                    queryResponse.RecordsInPage,
                    queryResponse.TotalPages,
                    Items = _mapper.MapModelToResponse(queryResponse.Records.ToList()),
                    context.Message.FilterBy,
                    context.Message.OrderBy,
                    context.Message.Sorting
                });
        }
    }
}