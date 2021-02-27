using AlbedoTeam.Identity.Contracts.Common;
using Identity.Business.Services.IdentityServers.Providers.Abstractions;

namespace Identity.Business.Services.IdentityServers
{
    public class IdentityServerService : IIdentityServerService
    {
        private readonly IdentityProviderFactory _factory;

        public IdentityServerService(IdentityProviderFactory factory)
        {
            _factory = factory;
        }

        public IAuthServerProvider AuthServerProvider(Provider provider)
        {
            return _factory.GetAuthServerProvider(provider);
        }

        public IGroupProvider GroupProvider(Provider provider)
        {
            return _factory.GetGroupProvider(provider);
        }

        public IUserProvider UserProvider(Provider provider)
        {
            return _factory.GetUserProvider(provider);
        }

        public IUserTypeProvider UserTypeProvider(Provider provider)
        {
            return _factory.GetUserTypeProvider(provider);
        }
    }
}