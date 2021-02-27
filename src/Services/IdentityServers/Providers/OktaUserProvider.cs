using Identity.Business.Services.IdentityServers.Providers.Abstractions;

namespace Identity.Business.Services.IdentityServers.Providers
{
    public class OktaUserProvider : IUserProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;

        public OktaUserProvider(IdentityServerOptions identityServerOptions)
        {
            _identityServerOptions = identityServerOptions;
        }
    }
}