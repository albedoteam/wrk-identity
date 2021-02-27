using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
using AlbedoTeam.Sdk.DataLayerAccess.Attributes;

namespace Identity.Business.Models
{
    [BsonCollection("Groups")]
    public class Group : DocumentWithAccount
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string IsDefault { get; set; }
        public Provider Provider { get; set; }
    }
}