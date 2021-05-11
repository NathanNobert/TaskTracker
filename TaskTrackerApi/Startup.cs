
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using TaskTrackerApi.Configurations;
using TaskTrackerApi.Services;
using AutoMapper;

[assembly: FunctionsStartup(typeof(TaskTrackerApi.Startup))]

namespace TaskTrackerApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Registering Configurations (IOptions pattern)
            builder
                .Services
                .AddOptions<ApplicationConfiguration>()
                .Configure<IConfiguration>((environmentSettings, configuration) =>
                {
                    configuration
                    .GetSection("EnvironmentSetting")
                    .Bind(environmentSettings);
                });



            // Registering services
            //builder.Services.AddMemoryCache();
            builder.Services.AddAutoMapper(typeof(Startup));

            builder
                .Services
                .AddSingleton<TaskTrackerData.IApplicationConfigurationService, ApplicationConfigurationService>();

        }
    }
}