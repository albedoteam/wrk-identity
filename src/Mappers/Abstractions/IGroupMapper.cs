namespace Identity.Business.Mappers.Abstractions
{
    using System.Collections.Generic;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using Models;

    public interface IGroupMapper
    {
        // request <-> model
        Group RequestToModel(CreateGroup request);

        // request <-> query
        QueryParams RequestToQuery(ListGroups request);

        // model <-> response
        GroupResponse MapModelToResponse(Group response);
        List<GroupResponse> MapModelToResponse(List<Group> response);
    }
}