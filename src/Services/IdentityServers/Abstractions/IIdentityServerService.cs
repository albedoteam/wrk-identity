using AlbedoTeam.Identity.Contracts.Common;

namespace Identity.Business.Services.IdentityServers.Abstractions
{
    public interface IIdentityServerService
    {
        IAuthServerProvider AuthServerProvider(Provider provider);
        IGroupProvider GroupProvider(Provider provider);
        IUserTypeProvider UserTypeProvider(Provider provider);
    }
}