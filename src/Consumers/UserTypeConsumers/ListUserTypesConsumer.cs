namespace Identity.Business.Consumers.UserTypeConsumers
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

            var page = context.Message.Page > 0 ? context.Message.Page : 1;
            var pageSize = context.Message.PageSize <= 1 ? 1 : context.Message.PageSize;

            var filterBy = CreateFilters(
                context.Message.ShowDeleted,
                context.Message.FilterBy);

            var orderBy = _repository.Helpers.CreateSorting(
                context.Message.OrderBy,
                context.Message.Sorting.ToString());

            var (totalPages, userTypes) = await _repository.QueryByPage(
                context.Message.AccountId,
                page,
                pageSize,
                filterBy,
                orderBy);

            if (!userTypes.Any())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "UserTypes not found"
                });
            else
                await context.RespondAsync<ListUserTypesResponse>(new
                {
                    context.Message.Page,
                    context.Message.PageSize,
                    RecordsInPage = userTypes.Count,
                    TotalPages = totalPages,
                    Items = _mapper.MapModelToResponse(userTypes.ToList()),
                    context.Message.FilterBy,
                    context.Message.OrderBy,
                    context.Message.Sorting
                });
        }

        private static FilterDefinition<UserType> CreateFilters(
            bool showDeleted,
            string filterBy,
            FilterDefinition<UserType> requiredFields = null)
        {
            var filteredByFilters = string.IsNullOrWhiteSpace(filterBy)
                ? null
                : FilterLanguage.ParseToFilterDefinition<UserType>(filterBy);
            
            var mainFilters = Builders<UserType>.Filter.And(Builders<UserType>.Filter.Empty);

            if (!showDeleted)
                mainFilters &= Builders<UserType>.Filter.Eq(a => a.IsDeleted, false);

            if (requiredFields is { })
                mainFilters &= requiredFields;

            if (filteredByFilters is { })
                mainFilters &= filteredByFilters;

            return mainFilters;
        }
    }
}