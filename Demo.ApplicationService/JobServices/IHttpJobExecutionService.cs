using Demo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.JobServices
{
	public interface IHttpJobExecutionService
	{
		Task Execute(HttpJob httpJob);
	}
}
