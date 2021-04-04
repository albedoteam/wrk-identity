namespace Identity.Business.Services.IdentityServers.Providers.Okta
{
    using System.Threading.Tasks;
    using Abstractions;
    using global::Okta.Sdk;
    using global::Okta.Sdk.Configuration;
    using Microsoft.Extensions.Logging;

    public class OktaGroupProvider : IGroupProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly ILogger<OktaGroupProvider> _logger;

        public OktaGroupProvider(IdentityServerOptions identityServerOptions, ILogger<OktaGroupProvider> logger)
        {
            _identityServerOptions = identityServerOptions;
            _logger = logger;
        }

        public async Task<string> Create(
            string name,
            string description)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            var groupResponse = await client.Groups.CreateGroupAsync(new CreateGroupOptions
            {
                Name = name,
                Description = description
            });

            if (groupResponse is { }) return groupResponse.Id;

            _logger.LogError("Group creation failed for name {Name}", name);
            return null;
        }

        public async Task Delete(string groupProviderId)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            await client.Groups.DeleteGroupAsync(groupProviderId);
        }

        public async Task<bool> Update(string groupProviderId, string name, string description)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            var group = await client.Groups.UpdateGroupAsync(new Group
            {
                Profile = new GroupProfile
                {
                    Name = name,
                    Description = description
                }
            }, groupProviderId);

            if (group is { }) return true;

            _logger.LogError("Group update failed for group providerId {GroupProviderId}", groupProviderId);
            return false;
        }
    }
}