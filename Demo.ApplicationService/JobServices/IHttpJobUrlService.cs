using Demo.ApplicationService.Messaging;
using Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService
{
	public interface IHttpJobUrlService
	{
		Task<JobUrlProcessResponse> CarryOutSingleJobUrl(Uri uri);
	}
}
