using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserConsumers
{
    public class ListUsersConsumer : IConsumer<ListUsers>
    {
        public Task Consume(ConsumeContext<ListUsers> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}