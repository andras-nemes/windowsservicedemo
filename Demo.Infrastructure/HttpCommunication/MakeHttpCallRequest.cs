using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.HttpCommunication
{
	public class MakeHttpCallRequest
	{
		public Uri Uri { get; set; }
		public HttpMethodType HttpMethod { get; set; }
		public string PostPutPayload { get; set; }
	}
}
