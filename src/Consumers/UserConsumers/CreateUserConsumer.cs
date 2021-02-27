using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class CreateUserConsumer : IConsumer<CreateUser>
    {
        public Task Consume(ConsumeContext<CreateUser> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}