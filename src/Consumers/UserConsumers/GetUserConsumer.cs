using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class GetUserConsumer : IConsumer<GetUser>
    {
        public Task Consume(ConsumeContext<GetUser> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}