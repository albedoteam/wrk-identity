using Identity.Business.Services.IdentityServers.Providers.Abstractions;

namespace Identity.Business.Services.IdentityServers.Providers
{
    public class OktaUserTypeProvider : IUserTypeProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;

        public OktaUserTypeProvider(IdentityServerOptions identityServerOptions)
        {
            _identityServerOptions = identityServerOptions;
        }
    }
}