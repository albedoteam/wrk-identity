using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Models;

namespace Identity.Business.Mappers.Abstractions
{
    public interface IUserTypeMapper
    {
        // request to model
        UserType RequestToModel(CreateUserType request);

        // model to response
        UserTypeResponse MapModelToResponse(UserType model);
        List<UserTypeResponse> MapModelToResponse(List<UserType> model);
    }
}