namespace Identity.Business.Mappers.Abstractions
{
    using System.Collections.Generic;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using Models;
    using Models.SubDocuments;

    public interface IAuthServerMapper
    {
        AuthServer MapRequestToModel(CreateAuthServer request);
        CommunicationRules MapRequestToModel(ICommunicationRules request);
        AuthServerResponse MapModelToResponse(AuthServer model);
        List<AuthServerResponse> MapModelToResponse(List<AuthServer> models);
    }
}