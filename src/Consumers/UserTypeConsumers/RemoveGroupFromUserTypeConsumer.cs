using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class RemoveGroupFromUserTypeConsumer : IConsumer<RemoveGroupFromUserType>
    {
        public Task Consume(ConsumeContext<RemoveGroupFromUserType> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}