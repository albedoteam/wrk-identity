using AlbedoTeam.Sdk.DataLayerAccess;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using Identity.Business.Db.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Db
{
    public class GroupRepository : BaseRepositoryWithAccount<Group>, IGroupRepository
    {
        public GroupRepository(
            IBaseRepository<Group> baseRepository,
            IHelpersWithAccount<Group> helpers) : base(baseRepository, helpers)
        {
        }
    }
}