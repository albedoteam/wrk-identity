﻿namespace Identity.Business.Mappers
{
    using System.Collections.Generic;
    using Abstractions;
    using AlbedoTeam.Identity.Contracts.Requests;
    using AlbedoTeam.Identity.Contracts.Responses;
    using AlbedoTeam.Sdk.DataLayerAccess.Utils.Query;
    using AutoMapper;
    using Models;

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

                // request -> query
                cfg.CreateMap<ListGroups, QueryParams>(MemberList.Destination)
                    .ForMember(l => l.Sorting, opt => opt.MapFrom(o => o.Sorting.ToString()));
            });

            _mapper = config.CreateMapper();
        }

        public Group RequestToModel(CreateGroup request)
        {
            return _mapper.Map<CreateGroup, Group>(request);
        }

        public QueryParams RequestToQuery(ListGroups request)
        {
            return _mapper.Map<ListGroups, QueryParams>(request);
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