using AlbedoTeam.Sdk.DataLayerAccess;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using Identity.Business.Db.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Db
{
    public class UserTypeRepository : BaseRepositoryWithAccount<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(
            IBaseRepository<UserType> baseRepository,
            IHelpersWithAccount<UserType> helpers) : base(baseRepository, helpers)
        {
        }
    }
}