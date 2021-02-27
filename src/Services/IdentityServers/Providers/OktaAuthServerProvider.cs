using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using Identity.Business.Models;
using Identity.Business.Services.IdentityServers.Models;
using Identity.Business.Services.IdentityServers.Providers.Abstractions;

namespace Identity.Business.Services.IdentityServers.Providers
{
    public class OktaAuthServerProvider : IAuthServerProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;

        public OktaAuthServerProvider(IdentityServerOptions identityServerOptions)
        {
            _identityServerOptions = identityServerOptions;
        }

        public Task<AuthServer> Create(
            Provider provider,
            string accountId,
            string accountName,
            string accountDisplayName)
        {
            accountName = accountName.Replace(" ", "-");

            var request = new AuthServerRequest
            {
                Provider = provider,
                Name = $"{accountName.ToLower()}-authserver",
                Description = $"{accountDisplayName} Authorization Server",
                Audiences = new List<string> {$"api://{accountName}"}
            };

            // todo integrate with Okta
            var identityResponse = new AuthServerResponse
            {
                Id = Guid.NewGuid().ToString(),
                Audiences = request.Audiences,
                Description = request.Description,
                Status = "ACTIVE"
            };

            var authServer = new AuthServer
            {
                AccountId = accountId,
                Provider = Provider.Okta,
                ProviderId = identityResponse.Id,
                Audience = identityResponse.Audiences[0],
                Description = identityResponse.Description,
                Issuer = identityResponse.Issuer,
                AuthUrl = $"https://{_identityServerOptions.OrgUrl}/oauth2/{identityResponse.Id}/v1/authorize",
                AccessTokenUrl = $"https://{_identityServerOptions.OrgUrl}/oauth2/{identityResponse.Id}/v1/token",
                ClientId = _identityServerOptions.PandorasClientId,
                BasicScopes = new List<string> {"openid", "profile"},
                Active = identityResponse.Status.Equals("ACTIVE", StringComparison.InvariantCultureIgnoreCase)
            };

            return Task.FromResult(authServer);
        }

        public async Task<bool> Activate(string providerId)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> Deactivate(string providerId)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> Delete(string providerId)
        {
            return await Task.FromResult(true);
        }
    }
}