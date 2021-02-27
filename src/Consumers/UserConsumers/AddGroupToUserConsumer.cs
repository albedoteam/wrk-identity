using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class AddGroupToUserConsumer : IConsumer<AddGroupToUser>
    {
        public Task Consume(ConsumeContext<AddGroupToUser> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}