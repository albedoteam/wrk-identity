using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class ChangeUserPasswordConsumer : IConsumer<ChangeUserPassword>
    {
        public Task Consume(ConsumeContext<ChangeUserPassword> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}