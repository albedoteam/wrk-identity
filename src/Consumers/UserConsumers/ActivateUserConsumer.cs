using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class ActivateUserConsumer : IConsumer<ActivateUser>
    {
        public Task Consume(ConsumeContext<ActivateUser> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}