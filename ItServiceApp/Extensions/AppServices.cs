using ItServiceApp.Business.MapperProfiles;
using ItServiceApp.Business.Services.Email;
using ItServiceApp.Business.Services.Payment;
using ItServiceApp.InjectOrnek;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItServiceApp.Extensions
{
    public static class AppServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(options =>
            {
                options.AddProfile(typeof(PaymentProfile));
                options.AddProfile(typeof(AccountProfile));
                options.AddProfile<SubscriptionProfiles>();
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IPaymentService, IyzicoPaymentService>();
            services.AddScoped<IMyDependency, NewMyDependency>(); //loose coupling
            //services.AddTransient<EmailSender>();

            return services;
        }
    }
}