using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using Identity.Business.Models;
using Identity.Business.Services.IdentityServers.Abstractions;
using Microsoft.Extensions.Logging;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace Identity.Business.Services.IdentityServers.Providers.Okta
{
    public class OktaAuthServerProvider : IAuthServerProvider
    {
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly ILogger<OktaAuthServerProvider> _logger;

        public OktaAuthServerProvider(
            IdentityServerOptions identityServerOptions,
            ILogger<OktaAuthServerProvider> logger)
        {
            _identityServerOptions = identityServerOptions;
            _logger = logger;
        }

        public async Task<AuthServer> Create(
            string accountId,
            string accountName,
            string accountDisplayName)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            accountName = accountName.Replace(" ", "-").ToLower();

            var authServerResponse = await client.AuthorizationServers.CreateAuthorizationServerAsync(
                new AuthorizationServer
                {
                    Audiences = new List<string> {$"api://{accountName}"},
                    Description = $"{accountDisplayName} Authorization Server",
                    Name = $"{accountName}-authserver"
                });

            if (authServerResponse is null)
            {
                _logger.LogError("Authorization server creation failed for account {Account}", accountName);
                return null;
            }

            var authServer = new AuthServer
            {
                AccountId = accountId,
                Provider = Provider.Okta,
                ProviderId = authServerResponse.Id,
                Audience = authServerResponse.Audiences[0],
                Description = authServerResponse.Description,
                Issuer = authServerResponse.Issuer,
                AuthUrl = $"https://{_identityServerOptions.OrgUrl}/oauth2/{authServerResponse.Id}/v1/authorize",
                AccessTokenUrl = $"https://{_identityServerOptions.OrgUrl}/oauth2/{authServerResponse.Id}/v1/token",
                ClientId = _identityServerOptions.PandorasClientId,
                BasicScopes = new List<string> {"openid", "profile"},
                Active = authServerResponse.Status.Equals("ACTIVE", StringComparison.InvariantCultureIgnoreCase)
            };

            return authServer;
        }

        public async Task Activate(string authServerProviderId)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            await client.AuthorizationServers.ActivateAuthorizationServerAsync(authServerProviderId);
        }

        public async Task Deactivate(string providerId)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            await client.AuthorizationServers.DeactivateAuthorizationServerAsync(providerId);
        }

        public async Task Delete(string providerId)
        {
            var client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = _identityServerOptions.ApiUrl,
                Token = _identityServerOptions.ApiKey
            });

            await client.AuthorizationServers.DeleteAuthorizationServerAsync(providerId);
        }
    }
}