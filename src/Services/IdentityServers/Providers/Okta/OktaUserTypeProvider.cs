namespace Identity.Business.Services.IdentityServers.Providers.Okta
{
    using System.Threading.Tasks;
    using Abstractions;
    using global::Okta.Sdk;
    using global::Okta.Sdk.Configuration;
    using Microsoft.Extensions.Logging;

    public class OktaUserTypeProvider : IUserTypeProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly ILogger<OktaUserTypeProvider> _logger;

        public OktaUserTypeProvider(IdentityServerOptions identityServerOptions, ILogger<OktaUserTypeProvider> logger)
        {
            _identityServerOptions = identityServerOptions;
            _logger = logger;
        }

        public async Task<string> Create(string name, string displayName, string description)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            var userType = await client.UserTypes.CreateUserTypeAsync(new UserType
            {
                Name = name,
                DisplayName = displayName,
                Description = description
            });

            if (userType is { }) return userType.Id;

            _logger.LogError("User Type creation failed for name {Name}", name);
            return null;
        }

        public async Task Delete(string userTypeProviderId)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            await client.UserTypes.DeleteUserTypeAsync(userTypeProviderId);
        }

        public async Task<bool> Update(string userTypeProviderId, string name, string displayName, string description)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            var group = await client.UserTypes.UpdateUserTypeAsync(new UserType
            {
                Name = name,
                DisplayName = displayName,
                Description = description
            }, userTypeProviderId);

            if (group is { }) return true;

            _logger.LogError("UserType update failed for usertype providerId {UserTypeProviderId}", userTypeProviderId);
            return false;
        }
    }
}