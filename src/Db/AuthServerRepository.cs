using System.Collections.Generic;
using System.Threading.Tasks;
using AlbedoTeam.Sdk.DataLayerAccess;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using Identity.Business.Db.Abstractions;
using Identity.Business.Models;
using MongoDB.Driver;

namespace Identity.Business.Db
{
    public class AuthServerRepository : BaseRepositoryWithAccount<AuthServer>, IAuthServerRepository
    {
        public AuthServerRepository(
            IBaseRepository<AuthServer> baseRepository,
            IHelpersWithAccount<AuthServer> helpers) : base(baseRepository, helpers)
        {
        }

        public async Task<(int totalPages, IReadOnlyList<AuthServer> readOnlyList)> QueryByPage(
            int page,
            int pageSize,
            FilterDefinition<AuthServer> filterDefinition,
            SortDefinition<AuthServer> sortDefinition = null)
        {
            return await BaseRepository.QueryByPage(page, pageSize, filterDefinition, sortDefinition);
        }
    }
}