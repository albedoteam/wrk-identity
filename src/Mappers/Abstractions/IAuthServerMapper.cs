using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Models;

namespace Identity.Business.Mappers.Abstractions
{
    public interface IAuthServerMapper
    {
        AuthServer MapRequestToModel(CreateAuthServer request);
        AuthServerResponse MapModelToResponse(AuthServer model);
        List<AuthServerResponse> MapModelToResponse(List<AuthServer> models);
    }
}