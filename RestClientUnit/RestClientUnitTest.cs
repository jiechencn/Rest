using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using Xzoo.Rest.Client;

namespace RestTestSpace
{

    class PostBean
    {
        public int UserId;
        public int Id;
        public string Title;
        public string Body;
    }

    [TestClass]
    public class RestClientUnitTest
    {
        [TestMethod]
        public void HttpsGetMethod_1()
        {
            Exception exception = null;
            string url = "https://jsonplaceholder.typicode.com/posts";
            RestMethod method = RestMethod.GET;

            AbstractRestRequest apiRequest = new RestRequest(url, method);
            RestConsumer<ICollection<PostBean>> consumer = new RestConsumer<ICollection<PostBean>>(apiRequest);
            IRestResult<ICollection<PostBean>> apiResult = consumer.Consume(out exception);

            Assert.IsTrue(apiResult.Succeed);
            Assert.AreEqual(100, apiResult.ResponseBody.Count);

        }

        [TestMethod]
        public void HttpsPostMethod_1()
        {
            Exception exception = null;
            string url = "https://jsonplaceholder.typicode.com/posts";

            RestMethod method = RestMethod.POST;

            IDictionary<string, string> payloads = new Dictionary<string, string>
            {
                { "userId", "1234" },
                { "title", "post-title-1" },
                { "body","post-body-1" }
             };

            AbstractRestRequest apiRequest = new RestRequest(url, method, payloads);
            RestConsumer<PostBean> consumer = new RestConsumer<PostBean>(apiRequest);
            IRestResult<PostBean> apiResult = consumer.Consume(out exception);

            Assert.IsTrue(apiResult.Succeed);
            Assert.AreEqual(1234, apiResult.ResponseBody.UserId);
        }

        [TestMethod]
        public void HttpsPutMethod_1()
        {
            Exception exception = null;
            string url = "https://jsonplaceholder.typicode.com/posts/12";

            RestMethod method = RestMethod.PUT;

            IDictionary<string, string> payloads = new Dictionary<string, string>
            {
                { "userId", "5678" },
                { "title", "put-title-1" },
                { "body","put-body-1" }
             };

            AbstractRestRequest apiRequest = new RestRequest(url, method, payloads);
            RestConsumer<PostBean> consumer = new RestConsumer<PostBean>(apiRequest);
            IRestResult<PostBean> apiResult = consumer.Consume(out exception);

            Assert.IsTrue(apiResult.Succeed);
            Assert.AreEqual(12, apiResult.ResponseBody.Id);
            Assert.AreEqual(5678, apiResult.ResponseBody.UserId);
        }

        [TestMethod]
        public void HttpsDeleteMethod_1()
        {
            Exception exception = null;
            string url = "https://jsonplaceholder.typicode.com/posts/12";
            RestMethod method = RestMethod.DELETE;

            AbstractRestRequest apiRequest = new RestRequest(url, method);
            RestConsumer<PostBean> consumer = new RestConsumer<PostBean>(apiRequest);
            IRestResult<PostBean> apiResult = consumer.Consume(out exception);

            Assert.IsTrue(apiResult.Succeed);
            Assert.AreEqual(0, apiResult.ResponseBody.Id);
        }

        [TestMethod]
        public void HttpsGetMethod_WrongUrl()
        {
            Exception exception = null;
            string url = "https://foo.bar/posts";
            RestMethod method = RestMethod.GET;

            AbstractRestRequest apiRequest = new RestRequest(url, method);
            RestConsumer<ICollection<PostBean>> consumer = new RestConsumer<ICollection<PostBean>>(apiRequest);
            IRestResult<ICollection<PostBean>> apiResult = consumer.Consume(out exception);

            Assert.AreEqual("Socket error", exception.Message);

        }
    }
}
