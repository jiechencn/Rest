using System.Collections.Generic;

namespace Xzoo.Rest.Client
{
    // 暂时没用
    public interface IRestRequest
    {
        public string Url { get; }
        public RestMethod Method { get; }
        public string Payload { get; }
        public  IDictionary<string, string> PayloadObject { get; }
        public IDictionary<string, string> CustomHeaders { get; }
        public ICollection<string> CleanHeaders { get; }
    }
}