using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.GroupConsumers
{
    public class GetGroupConsumer : IConsumer<GetGroup>
    {
        public Task Consume(ConsumeContext<GetGroup> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}