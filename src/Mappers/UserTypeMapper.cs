namespace Identity.Business.Mappers
{
    using System.Collections.Generic;
    using Abstractions;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
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

                // request -> query
                cfg.CreateMap<ListUserTypes, QueryParams>(MemberList.Destination)
                    .ForMember(l => l.Sorting, opt => opt.MapFrom(o => o.Sorting.ToString()));
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

        public QueryParams RequestToQuery(ListUserTypes request)
        {
            return _mapper.Map<ListUserTypes, QueryParams>(request);
        }
    }
}