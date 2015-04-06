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
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.3; WOW64; Trident/7.0)";
            client.Timeout = 60000;
            client.CookieContainer = Container;
            return client;
        }

        #region graph
        public HttpWebRequest CreateGraphGetRequest(string path)
        {
            var req = CreateGraphRequest(string.Format("https://graph.facebook.com/{0}", path));
            req.Method = WebRequestMethods.Http.Get;
            return req;
        }

        private HttpWebRequest CreateGraphRequest(string url)
        {
            var client = WebRequest.CreateHttp(url);
            client.Accept = "text/html, application/xhtml+xml, */*";
            client.Host = url.Replace("https://", "").Split('/')[0];
            client.UserAgent =
                "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.3; WOW64; Trident/7.0)";
            client.Timeout = 60000;
            client.CookieContainer = Container;
            return client;
        }


        #endregion
    }
}
