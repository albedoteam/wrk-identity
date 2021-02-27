using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class ClearUserSessionsConsumer : IConsumer<ClearUserSessions>
    {
        public Task Consume(ConsumeContext<ClearUserSessions> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}