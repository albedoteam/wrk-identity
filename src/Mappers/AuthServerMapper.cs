using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using AutoMapper;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Mappers
{
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

                // model to event
            });

            _mapper = config.CreateMapper();
        }

        public AuthServer MapRequestToModel(CreateAuthServer request)
        {
            return _mapper.Map<CreateAuthServer, AuthServer>(request);
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