using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.HttpCommunication
{
    public class MakeHttpCallResponse
    {
        public int HttpResponseCode { get; set; }
        public string HttpResponse { get; set; }
        public bool Success { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
