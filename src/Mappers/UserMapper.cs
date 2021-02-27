﻿using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using AutoMapper;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Models;

namespace Identity.Business.Mappers
{
    public class UserMapper : IUserMapper
    {
        private readonly IMapper _mapper;

        public UserMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // request to model
                cfg.CreateMap<CreateUser, User>().ReverseMap();

                // model to response
                cfg.CreateMap<User, UserResponse>(MemberList.Destination)
                    .ForMember(t => t.Id, opt => opt.MapFrom(o => o.Id.ToString()));

                // model to event
            });

            _mapper = config.CreateMapper();
        }
    }
}