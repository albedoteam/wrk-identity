namespace Identity.Business.Mappers
{
    using System.Collections.Generic;
    using Abstractions;
    using AlbedoTeam.Identity.Contracts.Common;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AutoMapper;
    using Models;
    using Models.SubDocuments;

    public class AuthServerMapper : IAuthServerMapper
    {
        private readonly IMapper _mapper;

        public AuthServerMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // request <-> model
                cfg.CreateMap<CreateAuthServer, AuthServer>().ReverseMap();

                // model -> response
                cfg.CreateMap<AuthServer, AuthServerResponse>(MemberList.Destination)
                    .ForMember(t => t.Id, opt => opt.MapFrom(o => o.Id.ToString()));

                cfg.CreateMap<CommunicationRules, ICommunicationRules>().ReverseMap();
                cfg.CreateMap<CommunicationRule, ICommunicationRule>().ReverseMap();

                // model to event
            });

            _mapper = config.CreateMapper();
        }

        public AuthServer MapRequestToModel(CreateAuthServer request)
        {
            return _mapper.Map<CreateAuthServer, AuthServer>(request);
        }

        public CommunicationRules MapRequestToModel(ICommunicationRules request)
        {
            return _mapper.Map<ICommunicationRules, CommunicationRules>(request);
        }

        public AuthServerResponse MapModelToResponse(AuthServer model)
        {
            return _mapper.Map<AuthServer, AuthServerResponse>(model);
        }

        public List<AuthServerResponse> MapModelToResponse(List<AuthServer> models)
        {
            return _mapper.Map<List<AuthServer>, List<AuthServerResponse>>(models);
        }
    }
}