using ItServiceApp.Business.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ItServiceApp.Extensions
{
    public static class AppRepositories
    {
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<AddressRepo>();
            services.AddScoped<SubscriptionRepo>();
            services.AddScoped<SubscriptionTypeRepo>();
            return services;
        }
    }
}
