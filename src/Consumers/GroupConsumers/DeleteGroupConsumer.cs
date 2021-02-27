using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.GroupConsumers
{
    public class DeleteGroupConsumer : IConsumer<DeleteGroup>
    {
        public Task Consume(ConsumeContext<DeleteGroup> context)
        {
            // context.Publish
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}