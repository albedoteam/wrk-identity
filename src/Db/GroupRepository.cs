namespace Identity.Business.Db
{
    using Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using Models;

    public class GroupRepository : BaseRepositoryWithAccount<Group>, IGroupRepository
    {
        public GroupRepository(
            IBaseRepository<Group> baseRepository,
            IHelpersWithAccount<Group> helpers) : base(baseRepository, helpers)
        {
        }
    }
}