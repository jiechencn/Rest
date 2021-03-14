using System.Collections;
using System.Collections.Generic;

namespace Xzoo.Rest.Client
{
    public enum RestMethod
    {
        GET, // 查询
        POST, // 创建
        PUT,  // 更新
        DELETE // 删除
    }
    public abstract class AbstractRestRequest : IRestRequest
    {
        public string Url { get; private set; }
        public RestMethod Method { get; private set; }
        public string Payload { get; protected set; }
        public IDictionary<string, string> PayloadObject { get; private set; }
        public IDictionary<string, string> CustomHeaders { get; private set; }
        public ICollection<string> CleanHeaders { get; private set; }

        // 抽象方法，强制让子类实现
        public abstract void AssemblePayload();

        public AbstractRestRequest(string url, RestMethod method, IDictionary<string, string> payloadObject = null, IDictionary<string, string> customHeaders = null, ICollection<string> cleanHeaders = null)
        {
            Url = url;
            Method = method;
            PayloadObject = payloadObject;
            CustomHeaders = customHeaders;
            CleanHeaders = cleanHeaders;
            AssemblePayload();
        }
    }
}