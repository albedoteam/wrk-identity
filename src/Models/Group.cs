namespace Identity.Business.Models
{
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess.Attributes;

    [Collection("Groups")]
    public class Group : DocumentWithAccount
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public Provider Provider { get; set; }
        public string ProviderId { get; set; }
    }
}