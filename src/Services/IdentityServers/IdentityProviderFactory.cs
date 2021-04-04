namespace Identity.Business.Services.IdentityServers
{
    using System;
    using Abstractions;
    using AlbedoTeam.Identity.Contracts.Common;
    using Providers.Okta;

    public class IdentityProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public IdentityProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAuthServerProvider GetAuthServerProvider(Provider provider)
        {
            return provider switch
            {
                Provider.Okta => (IAuthServerProvider) _serviceProvider.GetService(typeof(OktaAuthServerProvider)),
                _ => throw new NotImplementedException()
            };
        }

        public IGroupProvider GetGroupProvider(Provider provider)
        {
            return provider switch
            {
                Provider.Okta => (IGroupProvider) _serviceProvider.GetService(typeof(OktaGroupProvider)),
                _ => throw new NotImplementedException()
            };
        }

        public IUserTypeProvider GetUserTypeProvider(Provider provider)
        {
            return provider switch
            {
                Provider.Okta => (IUserTypeProvider) _serviceProvider.GetService(typeof(OktaUserTypeProvider)),
                _ => throw new NotImplementedException()
            };
        }
    }
}