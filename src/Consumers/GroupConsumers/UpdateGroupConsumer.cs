using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.GroupConsumers
{
    public class UpdateGroupConsumer : IConsumer<UpdateGroup>
    {
        public Task Consume(ConsumeContext<UpdateGroup> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}