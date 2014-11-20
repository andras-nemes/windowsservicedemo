using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Domain;

namespace Demo.ApplicationService.Messaging
{
	public class GetHttpJobResponse : BaseResponse
	{
		public HttpJob Job { get; set; }
	}
}
