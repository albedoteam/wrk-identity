namespace Identity.Business.Services
{
    using Accounts;
    using IdentityServers;
    using IdentityServers.Abstractions;
    using IdentityServers.Providers.Okta;
    using Microsoft.Extensions.DependencyInjection;

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
                .AddScoped<OktaUserTypeProvider>()
                .AddScoped<IUserTypeProvider, OktaUserTypeProvider>(s => s.GetService<OktaUserTypeProvider>());

            return services;
        }
    }
}