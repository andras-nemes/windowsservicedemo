using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.HttpCommunication
{
    public interface IHttpService
    {
        Task<MakeHttpCallResponse> MakeHttpCallAsync(MakeHttpCallRequest request);
    }
}
