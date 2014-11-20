using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.Messaging
{
	public class JobUrlProcessResponse : BaseResponse
	{
		public int HttpResponseCode { get; set; }
		public string HttpContent { get; set; }
		public TimeSpan TotalResponseTime { get; set; }
	}
}
