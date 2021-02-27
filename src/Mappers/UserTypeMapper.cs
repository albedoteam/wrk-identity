using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using AutoMapper;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Mappers
{
    public class UserTypeMapper : IUserTypeMapper
    {
        private readonly IMapper _mapper;

        public UserTypeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // request to model
                cfg.CreateMap<CreateUserType, UserType>().ReverseMap();

                // model to response
                cfg.CreateMap<UserType, UserTypeResponse>(MemberList.Destination)
                    .ForMember(t => t.Id, opt => opt.MapFrom(o => o.Id.ToString()));

                // model to event
            });

            _mapper = config.CreateMapper();
        }
    }
}