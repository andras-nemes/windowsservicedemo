using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.Messaging
{
	public abstract class BaseResponse
	{
		public Exception OperationException { get; set; }
	}
}
