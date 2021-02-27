using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class DeactivateUserConsumer : IConsumer<DeactivateUser>
    {
        public Task Consume(ConsumeContext<DeactivateUser> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}