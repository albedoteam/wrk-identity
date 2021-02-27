using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Requests;
using MassTransit;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class ListUserTypesConsumer : IConsumer<ListUserTypes>
    {
        public Task Consume(ConsumeContext<ListUserTypes> context)
        {
            // context.RespondAsync
            throw new NotImplementedException();
        }
    }
}