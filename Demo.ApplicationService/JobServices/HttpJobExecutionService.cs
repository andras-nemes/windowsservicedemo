using Demo.ApplicationService.Messaging;
using Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.JobServices
{
	public class HttpJobExecutionService : IHttpJobExecutionService
	{
		private readonly IHttpJobService _httpJobService;
		private readonly IHttpJobUrlService _httpJobUrlService;

		public HttpJobExecutionService(IHttpJobService httpJobService, IHttpJobUrlService httpJobUrlService)
		{
			if (httpJobService == null) throw new ArgumentNullException("HttpJobService");
			if (httpJobUrlService == null) throw new ArgumentNullException("HttpJobUrlService");
			_httpJobService = httpJobService;
			_httpJobUrlService = httpJobUrlService;
		}

		public async Task Execute(HttpJob httpJob)
		{
			httpJob.Started = true;
			httpJob.StatusMessage = string.Format("Starting job {0}.", httpJob.CorrelationId);
			_httpJobService.UpdateHttpJob(new UpdateHttpJobRequest(httpJob));
			TimeSpan totalTime = new TimeSpan(0, 0, 0);
			foreach (JobUrl jobUrl in httpJob.UrlsToRun)
			{
				jobUrl.Started = true;
				httpJob.StatusMessage = string.Concat("Starting url ", jobUrl.Uri);
				_httpJobService.UpdateHttpJob(new UpdateHttpJobRequest(httpJob));
				JobUrlProcessResponse jobUrlProcessResponse = await _httpJobUrlService.CarryOutSingleJobUrl(jobUrl.Uri);				
				jobUrl.Finished = true;
				jobUrl.HttpContent = jobUrlProcessResponse.HttpContent.Length > 30 ? jobUrlProcessResponse.HttpContent.Substring(0, 30) : jobUrlProcessResponse.HttpContent;
				jobUrl.HttpResponseCode = jobUrlProcessResponse.HttpResponseCode;
				jobUrl.TotalResponseTime = jobUrlProcessResponse.TotalResponseTime;
				httpJob.StatusMessage = string.Concat("Finished url ", jobUrl.Uri);
				_httpJobService.UpdateHttpJob(new UpdateHttpJobRequest(httpJob));
				totalTime += jobUrlProcessResponse.TotalResponseTime;
			}
			httpJob.Finished = true;
			httpJob.TotalJobDuration = totalTime;
			httpJob.StatusMessage = string.Format("Job {0} finished.", httpJob.CorrelationId);
			_httpJobService.UpdateHttpJob(new UpdateHttpJobRequest(httpJob));
		}		
	}
}
