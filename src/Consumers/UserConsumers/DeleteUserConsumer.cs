using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class DeleteUserConsumer : IConsumer<DeleteUser>
    {
        public Task Consume(ConsumeContext<DeleteUser> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}