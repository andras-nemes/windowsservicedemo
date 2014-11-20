using Demo.Infrastructure.ConfigurationService;
using Demo.Infrastructure.DatabaseConnectionSettingsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.DatabaseConnectionService
{
	public class HttpJobDatabaseConnectionService : IDatabaseConnectionSettingsService
	{
		private readonly IConfigurationRepository _configurationRepository;

		public HttpJobDatabaseConnectionService(IConfigurationRepository configurationRepository)
		{
			if (configurationRepository == null) throw new ArgumentNullException("IConfigurationRepository");
			_configurationRepository = configurationRepository;
		}

		public string GetDatabaseConnectionString()
		{
			return _configurationRepository.GetConnectionString(Constants.HttpJobsConnectionStringKeyName);
		}

		public string GetDatabaseName()
		{
			return _configurationRepository.GetConfigurationValue<string>(Constants.HttpJobsDatabaseNameSettingKeyName);
		}
	}
}
