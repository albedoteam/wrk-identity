namespace Identity.Business.Consumers.GroupConsumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
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

            var page = context.Message.Page > 0 ? context.Message.Page : 1;
            var pageSize = context.Message.PageSize <= 1 ? 1 : context.Message.PageSize;

            var filterBy = CreateFilters(
                context.Message.ShowDeleted,
                context.Message.FilterBy);

            var orderBy = _repository.Helpers.CreateSorting(
                context.Message.OrderBy,
                context.Message.Sorting.ToString());

            var (totalPages, groups) = await _repository.QueryByPage(
                context.Message.AccountId,
                page,
                pageSize,
                filterBy,
                orderBy);

            if (!groups.Any())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "Groups not found"
                });
            else
                await context.RespondAsync<ListGroupsResponse>(new
                {
                    context.Message.Page,
                    context.Message.PageSize,
                    RecordsInPage = groups.Count,
                    TotalPages = totalPages,
                    Items = _mapper.MapModelToResponse(groups.ToList()),
                    context.Message.FilterBy,
                    context.Message.OrderBy,
                    context.Message.Sorting
                });
        }

        private static FilterDefinition<Group> CreateFilters(
            bool showDeleted,
            string filterBy,
            FilterDefinition<Group> requiredFields = null)
        {
            var filteredByFilters = string.IsNullOrWhiteSpace(filterBy)
                ? null
                : FilterLanguage.ParseToFilterDefinition<Group>(filterBy);
            
            var mainFilters = Builders<Group>.Filter.And(Builders<Group>.Filter.Empty);

            if (!showDeleted)
                mainFilters &= Builders<Group>.Filter.Eq(a => a.IsDeleted, false);

            if (requiredFields is { })
                mainFilters &= requiredFields;

            if (filteredByFilters is { })
                mainFilters &= filteredByFilters;

            return mainFilters;
        }
    }
}