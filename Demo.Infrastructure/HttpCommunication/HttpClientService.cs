using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Infrastructure.HttpCommunication
{
	public class HttpClientService : IHttpService
	{
		public async Task<MakeHttpCallResponse> MakeHttpCallAsync(MakeHttpCallRequest request)
		{
			MakeHttpCallResponse response = new MakeHttpCallResponse();
			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.ExpectContinue = false;
				HttpRequestMessage requestMessage = new HttpRequestMessage(Translate(request.HttpMethod), request.Uri);
				if (!string.IsNullOrEmpty(request.PostPutPayload))
				{
					requestMessage.Content = new StringContent(request.PostPutPayload);
				}
				try
				{
					HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
					HttpStatusCode statusCode = responseMessage.StatusCode;
					response.HttpResponseCode = (int)statusCode;
					response.HttpResponse = await responseMessage.Content.ReadAsStringAsync();
					response.Success = true;
				}
				catch (Exception ex)
				{
					Exception inner = ex.InnerException;
					if (inner != null)
					{
						response.ExceptionMessage = inner.Message;
					}
					else
					{
						response.ExceptionMessage = ex.Message;
					}
				}
			}
			return response;
		}

		public HttpMethod Translate(HttpMethodType httpMethodType)
		{
			switch (httpMethodType)
			{
				case HttpMethodType.Delete:
					return HttpMethod.Delete;
				case HttpMethodType.Get:
					return HttpMethod.Get;
				case HttpMethodType.Head:
					return HttpMethod.Head;
				case HttpMethodType.Options:
					return HttpMethod.Options;
				case HttpMethodType.Post:
					return HttpMethod.Post;
				case HttpMethodType.Put:
					return HttpMethod.Put;
				case HttpMethodType.Trace:
					return HttpMethod.Trace;
			}

			return HttpMethod.Get;
		}
	}
}
