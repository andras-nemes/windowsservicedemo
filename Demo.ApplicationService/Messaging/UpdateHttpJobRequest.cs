using Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.Messaging
{
	public class UpdateHttpJobRequest
	{
		private HttpJob _updatedJob;
		public UpdateHttpJobRequest(HttpJob updatedJob)
		{
			_updatedJob = updatedJob;
		}

		public HttpJob UpdatedHttpJob
		{
			get
			{
				return _updatedJob;
			}
		}

	}
}
