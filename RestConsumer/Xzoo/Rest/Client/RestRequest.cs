using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xzoo.Rest.Client
{
    public class RestRequest : AbstractRestRequest
    {
        public RestRequest(string url, RestMethod method, IDictionary<string, string> payloadObject = null, IDictionary<string, string> customHeaders = null, ICollection<string> cleanHeaders = null)
            :base(url, method, payloadObject , customHeaders, cleanHeaders)
        {
        }

        public override void AssemblePayload()
        {
            if (this.PayloadObject != null && this.PayloadObject.Count >0)
            {
                Payload = JsonConvert.SerializeObject(this.PayloadObject);
            }
        }
    }
}