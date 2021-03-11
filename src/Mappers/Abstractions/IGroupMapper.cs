using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Models;

namespace Identity.Business.Mappers.Abstractions
{
    public interface IGroupMapper
    {
        // request <-> model
        Group RequestToModel(CreateGroup request);

        // model <-> response
        GroupResponse MapModelToResponse(Group response);
        List<GroupResponse> MapModelToResponse(List<Group> response);
    }
}