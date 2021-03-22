using AlbedoTeam.Accounts.Contracts.Requests;
using AlbedoTeam.Identity.Contracts.Events;
using AlbedoTeam.Sdk.DataLayerAccess;
using AlbedoTeam.Sdk.JobWorker.Configuration.Abstractions;
using AlbedoTeam.Sdk.MessageConsumer;
using Identity.Business.Consumers.AuthServerConsumers;
using Identity.Business.Consumers.GroupConsumers;
using Identity.Business.Consumers.UserTypeConsumers;
using Identity.Business.Db;
using Identity.Business.Mappers;
using Identity.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Business
{
    public class Startup : IWorkerConfigurator
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new IdentityServerOptions
            {
                OrgUrl = configuration.GetValue<string>("IdentityServer_OrgUrl"),
                PandorasClientId = configuration.GetValue<string>("IdentityServer_ClientId"),
                ApiUrl = configuration.GetValue<string>("IdentityServer_ApiUrl"),
                ApiKey = configuration.GetValue<string>("IdentityServer_ApiKey")
            });

            services.AddDataLayerAccess(db =>
            {
                db.ConnectionString = configuration.GetValue<string>("DatabaseSettings_ConnectionString");
                db.DatabaseName = configuration.GetValue<string>("DatabaseSettings_DatabaseName");
            });

            services.AddMappers();
            services.AddRepositories();
            services.AddServices();
            services.AddTransient<IJobRunner, JobConsumer>();

            services.AddBroker(
                configure => configure
                    .SetBrokerOptions(broker => broker.Host = configuration.GetValue<string>("Broker_Host")),
                consumers =>
                {
                    // auth servers
                    consumers
                        .Add<CreateAuthServerConsumer>()
                        .Add<DeleteAuthServerConsumer>()
                        .Add<UpdateAuthServerConsumer>()
                        .Add<GetAuthServerConsumer>()
                        .Add<ListAuthServersConsumer>()
                        .Add<ActivateAuthServerConsumer>()
                        .Add<DeactivateAuthServerConsumer>();

                    // groups
                    consumers
                        .Add<CreateGroupConsumer>()
                        .Add<DeleteGroupConsumer>()
                        .Add<UpdateGroupConsumer>()
                        .Add<GetGroupConsumer>()
                        .Add<ListGroupsConsumer>();

                    // user types
                    consumers
                        .Add<CreateUserTypeConsumer>()
                        .Add<DeleteUserTypeConsumer>()
                        .Add<UpdateUserTypeConsumer>()
                        .Add<GetUserTypeConsumer>()
                        .Add<ListUserTypesConsumer>()
                        .Add<AddGroupToUserTypeConsumer>()
                        .Add<RemoveGroupFromUserTypeConsumer>();
                },
                queues =>
                {
                    // auth server events
                    queues
                        .Map<AuthServerActivated>()
                        .Map<AuthServerDeactivated>();

                    // user type events
                    queues
                        .Map<GroupAddedToUserType>()
                        .Map<GroupRemovedFromUserType>();

                    // group events
                    queues
                        .Map<GroupDeleted>();
                },
                clients => clients
                    .Add<GetAccount>());
        }
    }
}