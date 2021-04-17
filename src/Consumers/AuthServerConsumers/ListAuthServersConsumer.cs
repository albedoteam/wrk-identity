namespace Identity.Business.Consumers.AuthServerConsumers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AlbedoTeam.Sdk.DataLayerAccess;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using AlbedoTeam.Sdk.FilterLanguage;
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
            var queryParams = QueryUtils.GetQueryParams<AuthServer>(_mapper.RequestToQuery(context.Message));
            var queryResponse = await _repository.QueryByPage(queryParams);

            if (!queryResponse.Records.Any())
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.NotFound,
                    ErrorMessage = "Auth Servers not found"
                });
            else
                await context.RespondAsync<ListAuthServersResponse>(new
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