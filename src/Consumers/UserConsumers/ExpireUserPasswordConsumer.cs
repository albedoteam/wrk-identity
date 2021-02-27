using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class ExpireUserPasswordConsumer : IConsumer<ExpireUserPassword>
    {
        public Task Consume(ConsumeContext<ExpireUserPassword> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}