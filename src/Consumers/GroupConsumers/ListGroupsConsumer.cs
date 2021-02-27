using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.GroupConsumers
{
    public class ListGroupsConsumer : IConsumer<ListGroups>
    {
        public Task Consume(ConsumeContext<ListGroups> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}