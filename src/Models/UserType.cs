﻿namespace Identity.Business.Models
{
    using System.Collections.Generic;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Sdk.DataLayerAccess.Abstractions;
    using AlbedoTeam.Sdk.DataLayerAccess.Attributes;

    [Collection("UserTypes")]
    public class UserType : DocumentWithAccount
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> PredefinedGroups { get; set; }
        public Provider Provider { get; set; }
        public string ProviderId { get; set; }
    }
}