using Demo.ApplicationService.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService
{
	public interface IHttpJobService
	{
		InsertHttpJobResponse InsertHttpJob(InsertHttpJobRequest request);
		GetHttpJobResponse GetHttpJob(GetHttpJobRequest request);
		UpdateHttpJobResponse UpdateHttpJob(UpdateHttpJobRequest request);
		GetHttpJobsResponse GetNewHttpJobs();
	}
}
