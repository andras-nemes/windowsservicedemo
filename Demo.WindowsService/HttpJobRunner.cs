using Demo.ApplicationService;
using Demo.ApplicationService.JobServices;
using Demo.ApplicationService.Messaging;
using Demo.Domain;
using Demo.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Demo.WindowsService
{
	public partial class HttpJobRunner : ServiceBase
	{
		private readonly ILoggingService _loggingService;
		private readonly IHttpJobService _httpJobService;
		private readonly IHttpJobExecutionService _httpJobExecutionService;
		private readonly string NL = Environment.NewLine;
		private readonly Timer _jobCollectionTimer;

		public HttpJobRunner(ILoggingService loggingService, IHttpJobService httpJobService, IHttpJobExecutionService httpJobExecutionService)
		{
			InitializeComponent();
			if (loggingService == null) throw new ArgumentNullException("LoggingService");
			if (httpJobService == null) throw new ArgumentNullException("HttpJobService");
			if (httpJobExecutionService == null) throw new ArgumentNullException("HttpJobExecutionService");
			_loggingService = loggingService;
			_httpJobService = httpJobService;
			_httpJobExecutionService = httpJobExecutionService;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			_jobCollectionTimer = new Timer(10000);
			_jobCollectionTimer.Elapsed += _jobCollectionTimer_Elapsed;
		}

		async void _jobCollectionTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				GetHttpJobsResponse getAllNewHttpJobsResponse = _httpJobService.GetNewHttpJobs();
				if (getAllNewHttpJobsResponse.OperationException != null) throw getAllNewHttpJobsResponse.OperationException;
				IEnumerable<HttpJob> newJobs = getAllNewHttpJobsResponse.HttpJobs;
				if (newJobs.Count() > 0)
				{
					LogNewJobs(newJobs);
					foreach (HttpJob httpJob in newJobs)
					{
						await Task.Factory.StartNew(async () => await RunSingleHttpJob(httpJob));
					}
				}
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		private async Task RunSingleHttpJob(HttpJob httpJob)
		{
			LogStartOfNewJob(httpJob);
			try
			{
				await _httpJobExecutionService.Execute(httpJob);
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
			LogEndOfNewJob(httpJob);
		}

		private void LogStartOfNewJob(HttpJob newJob)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("About to start job ").Append(newJob.CorrelationId);
			_loggingService.LogInfo(this, sb.ToString());
		}

		private void LogEndOfNewJob(HttpJob newJob)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Finished running job ").Append(newJob.CorrelationId);
			_loggingService.LogInfo(this, sb.ToString());
		}

		private void LogNewJobs(IEnumerable<HttpJob> newJobs)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Found the following new jobs: ");
			foreach (HttpJob httpJob in newJobs)
			{
				sb.Append(httpJob.CorrelationId).Append(", ");
			}
			_loggingService.LogInfo(this, sb.ToString());
		}

		private void LogException(Exception exception)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Exception caught in HttpJobRunner:")
				.Append(NL).Append("Exception message: ").Append(exception.Message)
				.Append(NL).Append("Exception stacktrace: ").Append(exception.StackTrace);
			_loggingService.LogError(this, sb.ToString(), exception);
		}

		void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			e.SetObserved();
			AggregateException aggregateException = (e.Exception as AggregateException).Flatten();
			aggregateException.Handle(ex =>
			{				
				return true;
			});
			List<Exception> inners = aggregateException.InnerExceptions.ToList();
			StringBuilder sb = new StringBuilder();
			sb.Append("Unhandled exception caught in HttpJobRunner by the generic catch-all handler.")
				.Append(NL);
			if (inners != null && inners.Count > 0)
			{
				foreach (Exception inner in inners)
				{
					sb.Append("Exception: ").Append(inner.Message).Append(NL).Append("Stacktrace: ").Append(NL)
						.Append(inner.StackTrace).Append(NL);
					if (inner.InnerException != null)
					{
						Exception innerInner = inner.InnerException;
						sb.Append("Inner exception has also an inner exception. Message: ")
							.Append(innerInner.Message).Append(". ").Append(NL)
							.Append("Stacktrace: ").Append(innerInner.StackTrace);
							
					}
					sb.Append(NL).Append(NL);
				}
			}
			else
			{
				sb.Append("No inner exceptions.").Append(NL);
				sb.Append("Plain message: ").Append(aggregateException.Message).Append(NL);
				sb.Append("Stacktrace: ").Append(aggregateException.StackTrace).Append(NL);
			}

			_loggingService.LogFatal(this, sb.ToString());
		}

		protected override void OnStart(string[] args)
		{
			_loggingService.LogInfo(this, "HttpJobRunner starting up");
			_jobCollectionTimer.Start();
		}		

		protected override void OnStop()
		{
			_loggingService.LogInfo(this, "HttpJobRunner stopping");
		}

		protected override void OnShutdown()
		{
			_loggingService.LogInfo(this, "HttpJobRunner shutting down");
		}

		protected override void OnContinue()
		{
			_loggingService.LogInfo(this, "HttpJobRunner continuing");
			_jobCollectionTimer.Start();
		}

		protected override void OnPause()
		{
			_loggingService.LogInfo(this, "HttpJobRunner pausing");
			_jobCollectionTimer.Start();
		}		
	}
}
