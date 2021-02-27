using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Common;

namespace Identity.Business.Services.IdentityServers.Models
{
    public class AuthServerRequest
    {
        public Provider Provider { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Audiences { get; set; }
    }
}