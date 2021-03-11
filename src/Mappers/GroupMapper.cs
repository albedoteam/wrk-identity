using System.Collections.Generic;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using AutoMapper;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Mappers
{
    public class GroupMapper : IGroupMapper
    {
        private readonly IMapper _mapper;

        public GroupMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // request <--> model
                cfg.CreateMap<CreateGroup, Group>().ReverseMap();

                // model -> response
                cfg.CreateMap<Group, GroupResponse>(MemberList.Destination)
                    .ForMember(t => t.Id, opt => opt.MapFrom(o => o.Id.ToString()));

                // model to event
            });

            _mapper = config.CreateMapper();
        }

        public Group RequestToModel(CreateGroup request)
        {
            return _mapper.Map<CreateGroup, Group>(request);
        }

        public GroupResponse MapModelToResponse(Group response)
        {
            return _mapper.Map<Group, GroupResponse>(response);
        }

        public List<GroupResponse> MapModelToResponse(List<Group> response)
        {
            return _mapper.Map<List<Group>, List<GroupResponse>>(response);
        }
    }
}