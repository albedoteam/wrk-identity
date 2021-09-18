namespace Identity.Business.Db.Abstractions
{
    using System.Threading.Tasks;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using Models;

    public interface IAuthServerRepository : IBaseRepositoryWithAccount<AuthServer>
    {
        Task<QueryResponse<AuthServer>> QueryByPage(QueryRequest<AuthServer> queryRequest);
    }
}