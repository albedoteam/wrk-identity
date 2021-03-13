using System.Linq;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;
using MassTransit;
using MongoDB.Driver;

namespace Identity.Business.Consumers.UserTypeConsumers
{
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
                null,
                AddFilterBy(context.Message.FilterBy));

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

        private FilterDefinition<UserType> AddFilterBy(string filterBy)
        {
            if (string.IsNullOrWhiteSpace(filterBy))
                return null;

            var optionalFilters = Builders<UserType>.Filter.Or(
                _repository.Helpers.Like(a => a.Name, filterBy),
                _repository.Helpers.Like(a => a.DisplayName, filterBy),
                _repository.Helpers.Like(a => a.Description, filterBy)
            );

            return optionalFilters;
        }

        private static FilterDefinition<UserType> CreateFilters(
            bool showDeleted = false,
            FilterDefinition<UserType> requiredFields = null,
            FilterDefinition<UserType> filteredByFilters = null)
        {
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