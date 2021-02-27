using System.Collections.Generic;
using System.Threading.Tasks;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using Identity.Business.Models;
using MongoDB.Driver;

namespace Identity.Business.Db.Abstractions
{
    public interface IAuthServerRepository : IBaseRepositoryWithAccount<AuthServer>
    {
        Task<(int totalPages, IReadOnlyList<AuthServer> readOnlyList)> QueryByPage(
            int page,
            int pageSize,
            FilterDefinition<AuthServer> filterDefinition,
            SortDefinition<AuthServer> sortDefinition = null);
    }
}