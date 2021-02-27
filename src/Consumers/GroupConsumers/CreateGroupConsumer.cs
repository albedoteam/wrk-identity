using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.GroupConsumers
{
    public class CreateGroupConsumer : IConsumer<CreateGroup>
    {
        public Task Consume(ConsumeContext<CreateGroup> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}