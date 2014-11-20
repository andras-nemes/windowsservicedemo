using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain
{
	public class JobUrl
	{
		public Uri Uri { get; set; }
		public int HttpResponseCode { get; set; }
		public string HttpContent { get; set; }
		public bool Started { get; set; }
		public bool Finished { get; set; }
		public TimeSpan TotalResponseTime { get; set; }
	}
}
