using Demo.ApplicationService;
using Demo.ApplicationService.DatabaseConnectionService;
using Demo.ApplicationService.JobServices;
using Demo.Domain;
using Demo.Infrastructure.ConfigurationService;
using Demo.Infrastructure.DatabaseConnectionSettingsService;
using Demo.Infrastructure.HttpCommunication;
using Demo.Infrastructure.Logging;
using Demo.Repository.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WindowsService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			IHttpJobService httpJobService = BuildHttpJobService();
			IHttpJobExecutionService httpJobExecutionService = BuildHttpJobExecutionService(httpJobService);
			ServicesToRun = new ServiceBase[] 
            { 
				
                new HttpJobRunner(new FileBasedLoggingService(@"c:\logging\log.txt"), httpJobService, httpJobExecutionService)
            };
			ServiceBase.Run(ServicesToRun);
		}

		private static IHttpJobService BuildHttpJobService()
		{
			IConfigurationRepository configurationRepository = new ConfigFileConfigurationRepository();
			IDatabaseConnectionSettingsService dbConnectionSettingsService = new HttpJobDatabaseConnectionService(configurationRepository);
			IJobRepository jobRepository = new JobRepository(dbConnectionSettingsService);
			IHttpJobService httpJobService = new HttpJobService(jobRepository);
			return httpJobService;
		}

		private static IHttpJobExecutionService BuildHttpJobExecutionService(IHttpJobService httpJobService)
		{
			IHttpJobExecutionService httpJobExecutionService = new HttpJobExecutionService(httpJobService, new HttpJobUrlService(new HttpClientService()));
			return httpJobExecutionService;
		}

	}
}
