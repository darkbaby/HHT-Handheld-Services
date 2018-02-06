using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSBT_HHT_Services
{
    public class MyActionResult : IHttpActionResult
    {

        string _value;
        HttpStatusCode _status;
        HttpRequestMessage _request;

        public MyActionResult(string value, HttpStatusCode status, HttpRequestMessage request)
        {
            _value = value;
            _status = status;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(_value),
                RequestMessage = _request,
                StatusCode = _status
            };
            return Task.FromResult(response);
        }
    }
}
