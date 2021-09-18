namespace Identity.Business.Db
{
    using Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using Models;

    public class UserTypeRepository : BaseRepositoryWithAccount<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(
            IBaseRepository<UserType> baseRepository,
            IHelpersWithAccount<UserType> helpers) : base(baseRepository, helpers)
        {
        }
    }
}