namespace Identity.Business.Consumers.AuthServerConsumers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Db.Abstractions;
    using Mappers.Abstractions;
    using MassTransit;
    using Models;
    using MongoDB.Driver;

    public class ListAuthServersConsumer : IConsumer<ListAuthServers>
    {
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public ListAuthServersConsumer(IAuthServerMapper mapper, IAuthServerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ListAuthServers> context)
        {
            var page = context.Message.Page > 0 ? context.Message.Page : 1;
            var pageSize = context.Message.PageSize <= 1 ? 1 : context.Message.PageSize;

            var filterBy = CreateFilters(
                context.Message.ShowDeleted,
                null,
                AddFilterBy(context.Message.FilterBy));

            var orderBy = _repository.Helpers.CreateSorting(
                context.Message.OrderBy,
                context.Message.Sorting.ToString());

            var (totalPages, authServers) = await _repository.QueryByPage(page, pageSize, filterBy, orderBy);

            if (!authServers.Any())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "Auth Servers not found"
                });
            else
                await context.RespondAsync<ListAuthServersResponse>(new
                {
                    context.Message.Page,
                    context.Message.PageSize,
                    RecordsInPage = authServers.Count,
                    TotalPages = totalPages,
                    Items = _mapper.MapModelToResponse(authServers.ToList()),
                    context.Message.FilterBy,
                    context.Message.OrderBy,
                    context.Message.Sorting
                });
        }

        private FilterDefinition<AuthServer> AddFilterBy(string filterBy)
        {
            if (string.IsNullOrWhiteSpace(filterBy))
                return null;

            var optionalFilters = Builders<AuthServer>.Filter.Or(
                _repository.Helpers.Like(a => a.Name, filterBy),
                _repository.Helpers.Like(a => a.Audience, filterBy),
                _repository.Helpers.Like(a => a.Description, filterBy),
                _repository.Helpers.Like(a => a.AccountId, filterBy)
            );

            return optionalFilters;
        }

        private static FilterDefinition<AuthServer> CreateFilters(
            bool showDeleted = false,
            FilterDefinition<AuthServer> requiredFields = null,
            FilterDefinition<AuthServer> filteredByFilters = null)
        {
            var mainFilters = Builders<AuthServer>.Filter.And(Builders<AuthServer>.Filter.Empty);

            if (!showDeleted)
                mainFilters &= Builders<AuthServer>.Filter.Eq(a => a.IsDeleted, false);

            if (requiredFields is { })
                mainFilters &= requiredFields;

            if (filteredByFilters is { })
                mainFilters &= filteredByFilters;

            return mainFilters;
        }
    }
}