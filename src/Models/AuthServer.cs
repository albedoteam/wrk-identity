using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using AlbedoTeam.Sdk.DataLayerAccess.Attributes;

namespace Identity.Business.Models
{
    [BsonCollection("AuthorizationServers")]
    public class AuthServer : DocumentWithAccount
    {
        public string Name { get; set; }
        public string Audience { get; set; }
        public string Description { get; set; }
        public string Issuer { get; set; }
        public string AuthUrl { get; set; }
        public string AccessTokenUrl { get; set; }
        public string ClientId { get; set; }
        public List<string> BasicScopes { get; set; }
        public bool Active { get; set; }
        public Provider Provider { get; set; }
        public string ProviderId { get; set; }
        public string UpdateReason { get; set; }
    }
}