using Demo.ApplicationService.Messaging;
using Demo.Domain;
using Demo.Infrastructure.HttpCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApplicationService.JobServices
{
	public class HttpJobUrlService : IHttpJobUrlService
	{
		private readonly IHttpService _httpService;

		public HttpJobUrlService(IHttpService httpService)
		{
			if (httpService == null) throw new ArgumentNullException("HttpService");
			_httpService = httpService;
		}

		public async Task<JobUrlProcessResponse> CarryOutSingleJobUrl(Uri uri)
		{
			JobUrlProcessResponse response = new JobUrlProcessResponse();
			
			try
			{				
				MakeHttpCallRequest httpCallRequest = new MakeHttpCallRequest();
				httpCallRequest.HttpMethod = HttpMethodType.Get;
				httpCallRequest.Uri = uri;
				DateTime start = DateTime.UtcNow;
				MakeHttpCallResponse httpCallResponse = await _httpService.MakeHttpCallAsync(httpCallRequest);
				DateTime stop = DateTime.UtcNow;
				TimeSpan diff = stop - start;
				response.TotalResponseTime = diff;
				if (!string.IsNullOrEmpty(httpCallResponse.ExceptionMessage))
				{
					response.HttpContent = httpCallResponse.ExceptionMessage;
					response.HttpResponseCode = -1;
				}
				else
				{						
					response.HttpContent = httpCallResponse.HttpResponse;
					response.HttpResponseCode = httpCallResponse.HttpResponseCode;
				}				
			}
			catch (Exception ex)
			{
				response.OperationException = ex;
			}
			
			return response;
		}
	}
}
