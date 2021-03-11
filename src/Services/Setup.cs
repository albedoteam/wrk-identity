using Identity.Business.Services.Accounts;
using Identity.Business.Services.IdentityServers;
using Identity.Business.Services.IdentityServers.Abstractions;
using Identity.Business.Services.IdentityServers.Providers.Okta;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Business.Services
{
    public static class Setup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            // identity server service and factory
            services.AddScoped<IIdentityServerService, IdentityServerService>();
            services.AddScoped<IdentityProviderFactory>();

            // providers
            services
                .AddScoped<OktaAuthServerProvider>()
                .AddScoped<IAuthServerProvider, OktaAuthServerProvider>(s => s.GetService<OktaAuthServerProvider>());

            services
                .AddScoped<OktaGroupProvider>()
                .AddScoped<IGroupProvider, OktaGroupProvider>(s => s.GetService<OktaGroupProvider>());

            services
                .AddScoped<OktaUserProvider>()
                .AddScoped<IUserProvider, OktaUserProvider>(s => s.GetService<OktaUserProvider>());

            services
                .AddScoped<OktaUserTypeProvider>()
                .AddScoped<IUserTypeProvider, OktaUserTypeProvider>(s => s.GetService<OktaUserTypeProvider>());

            return services;
        }
    }
}