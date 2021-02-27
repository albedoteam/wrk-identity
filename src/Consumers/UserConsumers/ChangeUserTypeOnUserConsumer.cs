using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class ChangeUserTypeOnUserConsumer : IConsumer<ChangeUserTypeOnUser>
    {
        public Task Consume(ConsumeContext<ChangeUserTypeOnUser> context)
        {
            // context.Publish
            throw new NotImplementedException();
        }
    }
}