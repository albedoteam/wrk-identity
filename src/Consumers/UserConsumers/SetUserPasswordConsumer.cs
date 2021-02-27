using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class SetUserPasswordConsumer : IConsumer<SetUserPassword>
    {
        public Task Consume(ConsumeContext<SetUserPassword> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}