
using Microsoft.Extensions.Options;
using TaskTrackerApi.Configurations;
using TaskTrackerData;

namespace TaskTrackerApi.Services
{
    public class ApplicationConfigurationService : IApplicationConfigurationService
    {
        private ApplicationConfiguration _configuration;

        public ApplicationConfigurationService(IOptions<ApplicationConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public string GetDbConnection()
        {
            return _configuration.DbConnectionString;
        }
    }
}
