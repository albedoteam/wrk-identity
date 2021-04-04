namespace Identity.Business.Mappers.Abstractions
{
    using System.Collections.Generic;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Models;

    public interface IGroupMapper
    {
        // request <-> model
        Group RequestToModel(CreateGroup request);

        // model <-> response
        GroupResponse MapModelToResponse(Group response);
        List<GroupResponse> MapModelToResponse(List<Group> response);
    }
}