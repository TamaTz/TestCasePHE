using Microsoft.EntityFrameworkCore;
using Project_PHE.Data;

namespace Project_PHE.Configurations
{
    public class EnvironmentConfiguration
    {
        private IConfiguration? _configuration;
        private IServiceCollection? _services;

        public EnvironmentConfiguration Service(IServiceCollection services)
        {
            _services = services;
            return this;
        }

        public EnvironmentConfiguration Configuration(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public EnvironmentConfiguration DbConnection()
        {
            var connectionString = _configuration.GetConnectionString("Database");

            _services!.AddDbContext<PheDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            return this;
        }
    }
}