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
                // request to model
                cfg.CreateMap<CreateGroup, Group>().ReverseMap();

                // model to response
                cfg.CreateMap<Group, GroupResponse>(MemberList.Destination)
                    .ForMember(t => t.Id, opt => opt.MapFrom(o => o.Id.ToString()));

                // model to event
            });

            _mapper = config.CreateMapper();
        }
    }
}