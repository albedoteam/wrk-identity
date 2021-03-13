﻿using System.Linq;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Common;
using AlbedoTeam.Identity.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Responses;
using Identity.Business.Db.Abstractions;
using Identity.Business.Mappers.Abstractions;
using Identity.Business.Services.Accounts;
using Identity.Business.Services.IdentityServers.Abstractions;
using MassTransit;

namespace Identity.Business.Consumers.AuthServerConsumers
{
    public class CreateAuthServerConsumer : IConsumer<CreateAuthServer>
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityServerService _identityServer;
        private readonly IAuthServerMapper _mapper;
        private readonly IAuthServerRepository _repository;

        public CreateAuthServerConsumer(
            IAccountService accountService,
            IAuthServerMapper mapper,
            IAuthServerRepository repository,
            IIdentityServerService identityServer)
        {
            _accountService = accountService;
            _mapper = mapper;
            _repository = repository;
            _identityServer = identityServer;
        }

        public async Task Consume(ConsumeContext<CreateAuthServer> context)
        {
            var account = await _accountService.GetAccount(context.Message.AccountId);
            var isAccountValid = account is { } && account.Enabled;
            if (!isAccountValid)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InvalidOperation,
                    ErrorMessage = $"Account invalid for id {context.Message.AccountId}"
                });
                return;
            }

            var exists = (await _repository.FilterBy(context.Message.AccountId,
                a => a.Provider.Equals(context.Message.Provider) && !a.IsDeleted)).Any();

            if (exists)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.AlreadyExists,
                    ErrorMessage = "Auth Server already exists"
                });
                return;
            }

            var model = await _identityServer
                .AuthServerProvider(context.Message.Provider)
                .Create(account.Id, account.Name, account.DisplayName);

            if (model is null)
            {
                await context.RespondAsync<ErrorResponse>(new
                {
                    ErrorType = ErrorType.InternalServerError,
                    ErrorMessage = "It was not possible to create Auth Server on Provider"
                });
                return;
            }

            var authServer = await _repository.InsertOne(model);
            await context.RespondAsync(_mapper.MapModelToResponse(authServer));
        }
    }
}