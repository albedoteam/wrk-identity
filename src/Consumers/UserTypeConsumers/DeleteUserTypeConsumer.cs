using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class DeleteUserTypeConsumer : IConsumer<DeleteUserType>
    {
        public Task Consume(ConsumeContext<DeleteUserType> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}