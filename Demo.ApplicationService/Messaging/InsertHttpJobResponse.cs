using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.Messaging
{
	public class InsertHttpJobResponse : BaseResponse
	{
		public Guid JobCorrelationId { get; set; }
	}
}
