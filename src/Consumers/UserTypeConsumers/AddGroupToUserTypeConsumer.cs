using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class AddGroupToUserTypeConsumer : IConsumer<AddGroupToUserType>
    {
        public Task Consume(ConsumeContext<AddGroupToUserType> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}