using Identity.Business.Mappers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Business.Mappers
{
    public static class Setup
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddTransient<IAuthServerMapper, AuthServerMapper>();
            services.AddTransient<IGroupMapper, GroupMapper>();
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IUserTypeMapper, UserTypeMapper>();

            return services;
        }
    }
}