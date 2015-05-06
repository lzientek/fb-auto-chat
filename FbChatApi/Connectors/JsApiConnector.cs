using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FbChatApi
{
    public class JsApiConnector
    {
        private const string LocalServerUrl = "http://localhost:1337";
        private int _reqCounter = 6;
        public string UserId { get; set; }
        public string Clientid { get; set; }
        public string Ttstamp { get; set; }
        public string FbDtsg { get; set; }
        public string Rev { get; set; }

        public JsApiConnector(FbWebRequest webRequest)
        {
        }

        public async Task<WebResponse> SendMessage(string msg, string threadId)
        {
            JsonSerializer jsWriter = new JsonSerializer();
            var stream = new StringWriter();
            jsWriter.Serialize(stream, new { message = msg, threadId = threadId });
            var post = await CreatePostRequestAsync(LocalServerUrl + "/sendMessage", stream.GetStringBuilder().ToString());
            return await post.GetResponseAsync();
        }


        public async Task<WebResponse> SetLogin(string email, string password)
        {
            JsonSerializer jsWriter=new JsonSerializer();
            var stream  = new StringWriter();
            jsWriter.Serialize(stream,new{email=email,password=password} );
            var post = await CreatePostRequestAsync(LocalServerUrl + "/set", stream.GetStringBuilder().ToString());
            return await post.GetResponseAsync();

        }

        private HttpWebRequest CreateRequest(string url)
        {
            var client = WebRequest.CreateHttp(url);
            client.ContentType = "application/x-www-form-urlencoded";
            client.Host = url.Replace("https://", "").Split('/')[0];
            return client;
        }
        public async Task<HttpWebRequest> CreatePostRequestAsync(string url, IEnumerable<HtmlInput> formInputs)
        {
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            foreach (var input in formInputs)
            {
                outgoingQueryString.Add(input.Name, input.Value);
            }

            string postdata = outgoingQueryString.ToString();

            return await CreatePostRequestAsync(url,postdata);
        }
        public async Task<HttpWebRequest> CreatePostRequestAsync(string url, string body)
        {
            var client = CreateRequest(url);
            client.Method = WebRequestMethods.Http.Post;

            using (var postStream = new StreamWriter(client.GetRequestStream()))
            {
                await postStream.WriteAsync(body);
                postStream.Flush();

            }

            return client;
        }
    }
}
