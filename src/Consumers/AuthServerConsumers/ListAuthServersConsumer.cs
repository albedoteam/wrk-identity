namespace Identity.Business.Consumers.AuthServerConsumers
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