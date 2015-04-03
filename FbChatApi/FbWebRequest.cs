using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FbChatApi
{
    public class FbWebRequest
    {

        public CookieContainer Container { get; set; }


        public FbWebRequest()
        {
            Container = new CookieContainer();
        }

        public  HttpWebRequest CreateGetRequest(string url)
        {
            var client = CreateRequest(url);
            client.Method = WebRequestMethods.Http.Get;
            
            return client;
        }
        public async Task<HttpWebRequest> CreatePostRequestAsync(string url,IEnumerable<HtmlInput> formInputs )
        {
            var client = CreateRequest(url);
            client.Method = WebRequestMethods.Http.Post;
            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            foreach (var input in formInputs)
            {
                outgoingQueryString.Add(input.Name, input.Value);
            }
            
            string postdata = outgoingQueryString.ToString();
            using(var postStream = new StreamWriter(client.GetRequestStream()))
            {
                await postStream.WriteAsync(postdata);
                postStream.Flush();
                
            }
            

            return client;
        }

        private HttpWebRequest CreateRequest(string url)
        {
            var client = WebRequest.CreateHttp(url);
            client.ContentType = "application/x-www-form-urlencoded";
            client.Referer = "https://www.facebook.com/";
            client.Host = url.Replace("https://", "").Split('/')[0];
            client.Headers.Add("Origin", "https://www.facebook.com");
            client.UserAgent =
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/600.3.18 (KHTML, like Gecko) Version/8.0.3 Safari/600.3.18";
            //client.Connection = "keep-alive";
            client.Timeout = 60000;
            client.CookieContainer = Container;
            return client;
        }
    }
}
