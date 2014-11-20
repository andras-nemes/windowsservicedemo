using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain
{
	public interface IJobRepository
	{
		Guid InsertNewHttpJob(HttpJob httpJob);
		HttpJob FindBy(Guid correlationId);
		void Update(HttpJob httpJob);
		IEnumerable<HttpJob> GetUnhandledJobs();
	}
}
