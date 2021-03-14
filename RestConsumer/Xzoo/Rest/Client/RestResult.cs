using System.Net;

namespace Xzoo.Rest.Client
{
    internal class RestResult<T> : IRestResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public T ResponseBody { get; }
        public bool Succeed { get; }
        public RestResult(HttpStatusCode statusCode, bool succeed, T responseData)
        {
            this.StatusCode = statusCode;
            this.Succeed = succeed;
            this.ResponseBody = responseData;
        }
    }
}