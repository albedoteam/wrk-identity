using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class CreateUserTypeConsumer : IConsumer<CreateUserType>
    {
        public Task Consume(ConsumeContext<CreateUserType> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}