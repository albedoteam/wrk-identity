namespace Identity.Business.Consumers.UserTypeConsumers
{
    using System;
    using System.Threading.Tasks;
    using AlbedoTeam.Identity.Contracts.Commands;
    using AlbedoTeam.Identity.Contracts.Events;
    using Db.Abstractions;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using Models;
    using MongoDB.Driver;
    using Services.Accounts;

    public class RemoveGroupFromUserTypeConsumer : IConsumer<RemoveGroupFromUserType>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<RemoveGroupFromUserTypeConsumer> _logger;
        private readonly IUserTypeRepository _userTypeRepository;

        public RemoveGroupFromUserTypeConsumer(
            IUserTypeRepository userTypeRepository,
            IAccountService accountService,
            ILogger<RemoveGroupFromUserTypeConsumer> logger)
        {
            _userTypeRepository = userTypeRepository;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RemoveGroupFromUserType> context)
        {
            if (!context.Message.UserTypeId.IsValidObjectId())
            {
                _logger.LogError("The UserType ID does not have a valid ObjectId format {UserTypeId}",
                    context.Message.UserTypeId);
                return;
            }

            if (!context.Message.GroupId.IsValidObjectId())
            {
                _logger.LogError("The Group ID does not have a valid ObjectId format {GroupId}",
                    context.Message.GroupId);
                return;
            }

            var account = await _accountService.GetAccount(context.Message.AccountId);
            var isAccountValid = account is { } && account.Enabled;
            if (!isAccountValid)
            {
                _logger.LogError("Account invalid for id {AccountId}", context.Message.AccountId);
                return;
            }

            var userType = await _userTypeRepository.FindById(context.Message.AccountId, context.Message.UserTypeId);
            if (userType is null)
            {
                _logger.LogError("UserType not found for id {UserTypeId}", context.Message.UserTypeId);
                return;
            }

            var contains = userType.PredefinedGroups.Contains(context.Message.GroupId);
            if (!contains)
            {
                _logger.LogInformation("UserType {UserTypeId} don't have group {GroupId}",
                    context.Message.UserTypeId, context.Message.GroupId);
                return;
            }

            userType.PredefinedGroups.Remove(context.Message.GroupId);

            var updateDefinition = Builders<UserType>.Update.Combine(
                Builders<UserType>.Update.Set(u => u.PredefinedGroups, userType.PredefinedGroups)
            );

            await _userTypeRepository.UpdateById(
                context.Message.AccountId,
                context.Message.UserTypeId,
                updateDefinition);

            await context.Publish<GroupRemovedFromUserType>(new
            {
                context.Message.AccountId,
                context.Message.UserTypeId,
                context.Message.GroupId,
                RemovedAt = DateTime.UtcNow
            });
        }
    }
}