using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain;

namespace Demo.ApplicationService.Messaging
{
	public class GetHttpJobsResponse : BaseResponse
	{
		public IEnumerable<HttpJob> HttpJobs { get; set; }
	}
}
