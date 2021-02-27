using AlbedoTeam.Sdk.DataLayerAccess;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using Identity.Business.Db.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Db
{
    public class UserRepository : BaseRepositoryWithAccount<User>, IUserRepository
    {
        public UserRepository(
            IBaseRepository<User> baseRepository,
            IHelpersWithAccount<User> helpers) : base(baseRepository, helpers)
        {
        }
    }
}