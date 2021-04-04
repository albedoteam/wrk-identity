namespace Identity.Business.Db.Abstractions
{
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using Models;

    public interface IUserTypeRepository : IBaseRepositoryWithAccount<UserType>
    {
    }
}