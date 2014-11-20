using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain
{
	public class HttpJob
	{
		public List<JobUrl> UrlsToRun { get; set; }
		public string StatusMessage { get; set; }
		public bool Started { get; set; }
		public bool Finished { get; set; }
		public TimeSpan TotalJobDuration { get; set; }
		public Guid CorrelationId { get; set; }

		public override string ToString()
		{
			string NL = Environment.NewLine;
			StringBuilder sb = new StringBuilder();
			sb.Append("Status report of job with correlation id: ").Append(CorrelationId).Append(NL);
			sb.Append("------------------------------------------").Append(NL);
			sb.Append("Started: ").Append(Started).Append(NL);
			sb.Append("Finished: ").Append(Finished).Append(NL);
			sb.Append("Status: ").Append(StatusMessage).Append(NL).Append(NL);

			foreach (JobUrl jobUrl in UrlsToRun)
			{
				sb.Append(NL);
				sb.Append("Url: ").Append(jobUrl.Uri).Append(NL);
				sb.Append("Started: ").Append(jobUrl.Started).Append(NL);
				sb.Append("Finished: ").Append(jobUrl.Finished).Append(NL);
				if (jobUrl.Finished)
				{
					sb.Append("Http response code: ").Append(jobUrl.HttpResponseCode).Append(NL);
					sb.Append("Partial content: ").Append(jobUrl.HttpContent).Append(NL);
					sb.Append("Response time: ").Append(Convert.ToInt32(jobUrl.TotalResponseTime.TotalMilliseconds)).Append(" ms").Append(NL);					
				}
			}

			if (Finished)
			{
				sb.Append("Total job duration: ").Append(Convert.ToInt32(TotalJobDuration.TotalMilliseconds)).Append(" ms. ").Append(NL);
			}
			sb.Append("------------------------------------------").Append(NL).Append(NL);

			return sb.ToString();
		}
	}
}
