namespace Identity.Business.Db
{
    using Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class Setup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthServerRepository, AuthServerRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();

            return services;
        }
    }
}