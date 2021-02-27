using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class UpdateUserConsumer : IConsumer<UpdateUser>
    {
        public Task Consume(ConsumeContext<UpdateUser> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}