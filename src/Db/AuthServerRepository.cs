﻿namespace Identity.Business.Db
{
    using System.Threading.Tasks;
    using Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using Models;

    public class AuthServerRepository : BaseRepositoryWithAccount<AuthServer>, IAuthServerRepository
    {
        public AuthServerRepository(
            IBaseRepository<AuthServer> baseRepository,
            IHelpersWithAccount<AuthServer> helpers) : base(baseRepository, helpers)
        {
        }

        public async Task<QueryResponse<AuthServer>> QueryByPage(QueryRequest<AuthServer> queryRequest)
        {
            return await BaseRepository.QueryByPage(queryRequest);
        }
    }
}