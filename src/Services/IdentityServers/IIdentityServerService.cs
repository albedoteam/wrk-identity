using AlbedoTeam.Identity.Contracts.Common;
using Identity.Business.Services.IdentityServers.Providers.Abstractions;

namespace Identity.Business.Services.IdentityServers
{
    public interface IIdentityServerService
    {
        IAuthServerProvider AuthServerProvider(Provider provider);
        IGroupProvider GroupProvider(Provider provider);
        IUserProvider UserProvider(Provider provider);
        IUserTypeProvider UserTypeProvider(Provider provider);
    }
}