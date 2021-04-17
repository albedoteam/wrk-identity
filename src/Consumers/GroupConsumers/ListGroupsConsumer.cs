namespace Identity.Business.Consumers.GroupConsumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using AlbedoTeam.Sdk.FilterLanguage;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Models;
    using MongoDB.Driver;

    public class ListGroupsConsumer : IConsumer<ListGroups>
    {
        private readonly IGroupMapper _mapper;
        private readonly IGroupRepository _repository;

        public ListGroupsConsumer(IGroupMapper mapper, IGroupRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ListGroups> context)
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

            var queryRequest = QueryUtils.GetQueryParams<Group>(_mapper.RequestToQuery(context.Message));
            var queryResponse = await _repository.QueryByPage(context.Message.AccountId, queryRequest);

            if (!queryResponse.Records.Any())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "Groups not found"
                });
            else
                await context.RespondAsync<ListGroupsResponse>(new
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