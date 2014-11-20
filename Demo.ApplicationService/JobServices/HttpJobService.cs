using Demo.ApplicationService.Messaging;
using Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService
{
	public class HttpJobService : IHttpJobService
	{
		private readonly IJobRepository _jobRepository;

		public HttpJobService(IJobRepository jobRepository)
		{
			if (jobRepository == null) throw new ArgumentNullException("Job repository!!");
			_jobRepository = jobRepository;
		}

		public InsertHttpJobResponse InsertHttpJob(InsertHttpJobRequest request)
		{
			InsertHttpJobResponse response = new InsertHttpJobResponse();
			HttpJob job = new HttpJob();
			job.StatusMessage = "Inserted";
			List<JobUrl> jobUrls = new List<JobUrl>();
			foreach (Uri uri in request.UrisToRun)
			{
				jobUrls.Add(new JobUrl() { Uri = uri });
			}
			job.UrlsToRun = jobUrls;
			try
			{
				Guid correlationId = _jobRepository.InsertNewHttpJob(job);
				response.JobCorrelationId = correlationId;
			}
			catch (Exception ex)
			{
				response.OperationException = ex;
			}

			return response;
		}

		public GetHttpJobResponse GetHttpJob(GetHttpJobRequest request)
		{
			GetHttpJobResponse response = new GetHttpJobResponse();

			try
			{
				response.Job = _jobRepository.FindBy(request.CorrelationId);
			}
			catch (Exception ex)
			{
				response.OperationException = ex;
			}

			return response;
		}

		public UpdateHttpJobResponse UpdateHttpJob(UpdateHttpJobRequest request)
		{
			UpdateHttpJobResponse response = new UpdateHttpJobResponse();			
			try
			{
				_jobRepository.Update(request.UpdatedHttpJob);
			}
			catch (Exception ex)
			{
				response.OperationException = ex;
			}
			return response;
		}

		public GetHttpJobsResponse GetNewHttpJobs()
		{
			GetHttpJobsResponse response = new GetHttpJobsResponse();
			try
			{
				response.HttpJobs = _jobRepository.GetUnhandledJobs();
			}
			catch (Exception ex)
			{
				response.OperationException = ex;
			}
			return response;
		}
	}
}
