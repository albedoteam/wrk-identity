using System;
using System.Threading.Tasks;
using AlbedoTeam.Identity.Contracts.Commands;
using AlbedoTeam.Identity.Contracts.Events;
using Identity.Business.Db.Abstractions;
using Identity.Business.Models;
using Identity.Business.Services.Accounts;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Identity.Business.Consumers.UserTypeConsumers
{
    public class AddGroupToUserTypeConsumer : IConsumer<AddGroupToUserType>
    {
        private readonly IAccountService _accountService;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<AddGroupToUserTypeConsumer> _logger;
        private readonly IUserTypeRepository _userTypeRepository;

        public AddGroupToUserTypeConsumer(
            IUserTypeRepository userTypeRepository,
            IGroupRepository groupRepository,
            IAccountService accountService,
            ILogger<AddGroupToUserTypeConsumer> logger)
        {
            _userTypeRepository = userTypeRepository;
            _groupRepository = groupRepository;
            _accountService = accountService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AddGroupToUserType> context)
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

            var group = await _groupRepository.FindById(context.Message.AccountId, context.Message.GroupId);
            if (group is null)
            {
                _logger.LogError("Group not found for id {GroupId}", context.Message.GroupId);
                return;
            }

            var contains = userType.PredefinedGroups.Contains(context.Message.GroupId);
            if (contains)
            {
                _logger.LogInformation("UserType {UserTypeId} already have group {GroupId}",
                    context.Message.UserTypeId, context.Message.GroupId);
                return;
            }

            userType.PredefinedGroups.Add(context.Message.GroupId);

            var updateDefinition = Builders<UserType>.Update.Combine(
                Builders<UserType>.Update.Set(u => u.PredefinedGroups, userType.PredefinedGroups)
            );

            await _userTypeRepository.UpdateById(
                context.Message.AccountId,
                context.Message.UserTypeId,
                updateDefinition);

            await context.Publish<GroupAddedToUserType>(new
            {
                context.Message.AccountId,
                context.Message.UserTypeId,
                context.Message.GroupId,
                AddedAt = DateTime.UtcNow
            });
        }
    }
}