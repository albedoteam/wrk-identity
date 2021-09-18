namespace Identity.Business.Services.IdentityServers.Abstractions
{
    using AlbedoTeam.Identity.Contracts.Common;

    public interface IIdentityServerService
    {
        IAuthServerProvider AuthServerProvider(Provider provider);
        IGroupProvider GroupProvider(Provider provider);
        IUserTypeProvider UserTypeProvider(Provider provider);
    }
}