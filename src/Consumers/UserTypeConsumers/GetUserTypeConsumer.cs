using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class GetUserTypeConsumer : IConsumer<GetUserType>
    {
        public Task Consume(ConsumeContext<GetUserType> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}