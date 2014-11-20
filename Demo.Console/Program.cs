using Demo.ApplicationService;
using Demo.ApplicationService.DatabaseConnectionService;
using Demo.ApplicationService.JobServices;
using Demo.ApplicationService.Messaging;
using Demo.Domain;
using Demo.Infrastructure.ConfigurationService;
using Demo.Infrastructure.DatabaseConnectionSettingsService;
using Demo.Infrastructure.HttpCommunication;
using Demo.Repository.MongoDb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.ConsoleConsumer
{
	class Program
	{
		static void Main(string[] args)
		{			
			List<Uri> uris = EnterUrlJobs();
			IHttpJobService httpJobService = BuildHttpJobService();
			InsertHttpJobResponse insertResponse = InsertHttpJob(uris, httpJobService);
			MonitorJob(insertResponse.JobCorrelationId, httpJobService);

			Console.WriteLine("Main finishing, press any key to exit...");			
			Console.ReadKey();
		}			

		private static List<Uri> EnterUrlJobs()
		{
			HttpJob httpJob = new HttpJob();
			List<Uri> uris = new List<Uri>();
			Console.WriteLine("Enter a range of URLs. Leave empty and press ENTER when done.");
			Console.Write("Url 1: ");
			string url = Console.ReadLine();
			uris.Add(new Uri(url));
			int urlCounter = 2;
			while (!string.IsNullOrEmpty(url))
			{
				Console.Write("Url {0}: ", urlCounter);
				url = Console.ReadLine();
				if (!string.IsNullOrEmpty(url))
				{
					uris.Add(new Uri(url));
					urlCounter++;
				}
			}

			return uris;
		}

		private static InsertHttpJobResponse InsertHttpJob(List<Uri> uris, IHttpJobService httpJobService)
		{
			InsertHttpJobRequest insertRequest = new InsertHttpJobRequest(uris);
			InsertHttpJobResponse insertResponse = httpJobService.InsertHttpJob(insertRequest);
			return insertResponse;
		}

		private static void MonitorJob(Guid correlationId, IHttpJobService httpJobService)
		{
			GetHttpJobRequest getJobRequest = new GetHttpJobRequest() { CorrelationId = correlationId };
			GetHttpJobResponse getjobResponse = httpJobService.GetHttpJob(getJobRequest);
			bool jobFinished = getjobResponse.Job.Finished;
			while (!jobFinished)
			{
				Console.WriteLine(getjobResponse.Job.ToString());
				getjobResponse = httpJobService.GetHttpJob(getJobRequest);
				jobFinished = getjobResponse.Job.Finished;
				if (!jobFinished)
				{
					Thread.Sleep(2000);
					Console.WriteLine();
				}
			}
			getjobResponse = httpJobService.GetHttpJob(getJobRequest);
			Console.WriteLine(getjobResponse.Job.ToString());
		}
		
		private static IHttpJobService BuildHttpJobService()
		{
			IConfigurationRepository configurationRepository = new ConfigFileConfigurationRepository();
			IDatabaseConnectionSettingsService dbConnectionSettingsService = new HttpJobDatabaseConnectionService(configurationRepository);
			IJobRepository jobRepository = new JobRepository(dbConnectionSettingsService);
			IHttpJobService httpJobService = new HttpJobService(jobRepository);
			return httpJobService;
		}			
	}
}
