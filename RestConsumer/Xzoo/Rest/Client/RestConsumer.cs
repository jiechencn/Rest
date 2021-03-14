using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Xzoo.Rest.Client
{
    public class RestConsumer<T>
    {
        public static string KeyError = "Error";
        public RestConsumer(AbstractRestRequest apiRequest)
        {
            this.ApiRequest = apiRequest;
        }
        private AbstractRestRequest ApiRequest { get; set; }
        HttpWebRequest RestWebRequest { get; set; }
        public IRestResult<T> Consume()
        {
            string endpointUrl = ApiRequest.Url;
            Uri uri = new Uri(endpointUrl);
            RestWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            AssembleBaseHeaders();
            AssembleCustomHeaders();
            SendWebRequest();

            try
            {
                using (HttpWebResponse webResponse = RestWebRequest.GetResponse() as HttpWebResponse)
                {
                    return BuildResult(webResponse);
                }
            }
            catch (Exception ex)
            {
                
                throw new RestException("Socket error", ex);
            }
            finally
            {
                // remove authorization token from request.head to avoid logging
                CleanRequestHeaders();
                RestWebRequest.Abort();
            }
        }
        private IRestResult<T> BuildResult(HttpWebResponse webResponse)
        {
            if (webResponse == null)
            {
                throw new RestException("RestApi reponse is null");
            }

            // 泛型 默认值
            T responseData = default;
            bool succeed = false;

            using (Stream responseStream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string readContent = reader.ReadToEnd();

                // if http returns success
                if (webResponse.StatusCode < HttpStatusCode.Ambiguous && webResponse.StatusCode >= HttpStatusCode.OK)
                {
                    succeed = true;
                }
                responseData = JsonConvert.DeserializeObject<T>(readContent);
            }

            return new RestResult<T>(webResponse.StatusCode, succeed, responseData);
        }
        public void CleanRequestHeaders()
        {
            if (ApiRequest.CleanHeaders != null)
            {
                foreach (string item in ApiRequest.CleanHeaders)
                {
                    RestWebRequest.Headers.Remove(item);
                }
            }

        }

        // 提供虚方法，可以让子类重载，重设某些header
        public virtual void AssembleBaseHeaders()
        {

            RestWebRequest.Proxy = null;
            RestWebRequest.AllowAutoRedirect = false;

            RestWebRequest.KeepAlive = true;
            RestWebRequest.ServicePoint.SetTcpKeepAlive(
                true,
                (int)TimeSpan.FromSeconds(30).TotalMilliseconds,
                (int)TimeSpan.FromSeconds(5).TotalMilliseconds);
            RestWebRequest.ServicePoint.MaxIdleTime = 2100000;
            RestWebRequest.ServicePoint.UseNagleAlgorithm = false;
            RestWebRequest.ServicePoint.Expect100Continue = false;
            if (RestWebRequest.ServicePoint.ConnectionLimit < 200)
            {
                RestWebRequest.ServicePoint.ConnectionLimit = 200;
            }

            // set http entity headers
            RestWebRequest.Method = ApiRequest.Method.ToString();
            RestWebRequest.ContentType = "application/json";
            RestWebRequest.Accept = "application/json";
            RestWebRequest.Timeout = 2100000;
        }

        private void AssembleCustomHeaders()
        {
            // set customized headers
            if (ApiRequest.CustomHeaders != null)
            {
                foreach (var item in ApiRequest.CustomHeaders)
                {
                    RestWebRequest.Headers.Add(item.Key, item.Value);
                }
            }
        }

        private void SendWebRequest()
        {
            if (string.IsNullOrWhiteSpace(ApiRequest.Payload))
            {
                RestWebRequest.ContentLength = 0;
                return;
            }

            byte[] requestBytes = Encoding.UTF8.GetBytes(ApiRequest.Payload);
            RestWebRequest.ContentLength = requestBytes.Length;

            using (Stream requestStream = RestWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Flush();
                requestStream.Close();
            }
        }
    }
}
