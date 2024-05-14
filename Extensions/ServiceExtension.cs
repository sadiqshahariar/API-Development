using Newroz_Home_Task.Repositories.Interface;
using Newroz_Home_Task.Repositories.Repository;

namespace Newroz_Home_Task.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection DIService(this IServiceCollection services)
        {
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            return services;
        }
    }
}
