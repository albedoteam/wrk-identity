using Identity.Business.Services.IdentityServers.Providers.Abstractions;

namespace Identity.Business.Services.IdentityServers.Providers
{
    public class OktaGroupProvider : IGroupProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;

        public OktaGroupProvider(IdentityServerOptions identityServerOptions)
        {
            _identityServerOptions = identityServerOptions;
        }
    }
}