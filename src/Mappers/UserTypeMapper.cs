namespace Identity.Business.Mappers
{
    using System.Collections.Generic;
    using Abstractions;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AutoMapper;
    using Models;

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

        public UserType RequestToModel(CreateUserType request)
        {
            return _mapper.Map<CreateUserType, UserType>(request);
        }

        public UserTypeResponse MapModelToResponse(UserType model)
        {
            return _mapper.Map<UserType, UserTypeResponse>(model);
        }

        public List<UserTypeResponse> MapModelToResponse(List<UserType> model)
        {
            return _mapper.Map<List<UserType>, List<UserTypeResponse>>(model);
        }
    }
}