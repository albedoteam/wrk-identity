using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class UpdateUserTypeConsumer : IConsumer<UpdateUserType>
    {
        public Task Consume(ConsumeContext<UpdateUserType> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}