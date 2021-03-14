using System.Net;

namespace Xzoo.Rest.Client
{
    public interface IRestResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public T ResponseBody { get; }
        public bool Succeed { get; }
    }
}