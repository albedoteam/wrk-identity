using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Db.Abstractions
{
    public interface IGroupRepository : IBaseRepositoryWithAccount<Group>
    {
    }
}