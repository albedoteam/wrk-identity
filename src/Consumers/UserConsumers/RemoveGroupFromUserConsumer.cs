using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class RemoveGroupFromUserConsumer : IConsumer<RemoveGroupFromUser>
    {
        public Task Consume(ConsumeContext<RemoveGroupFromUser> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}