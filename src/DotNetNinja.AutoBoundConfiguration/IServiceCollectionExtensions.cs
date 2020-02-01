using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetNinja.AutoBoundConfiguration
{
    public static class IServiceCollectionExtensions
    {
        public static AutoBoundConfigurationBuilder AddAutoBoundConfigurations(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            return new AutoBoundConfigurationBuilder(services, configuration);
        }
    }
}