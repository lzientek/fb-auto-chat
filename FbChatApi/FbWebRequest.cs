using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace FbChatApi
{
    public class FbWebRequest
    {
        private string _FacebookCom;

        public CookieContainer Container { get; set; }


        public FbWebRequest()
        {
            Container = new CookieContainer();
            _FacebookCom = "https://www.facebook.com/";
        }

        #region persistance ne fonctione pas

        public const string CookieXml = "Cookie.xml";
        
        public void Save()
        {
            
            var t = new Thread(() =>
            {
                XmlSerializer xs = new XmlSerializer(typeof(Cookie[]));
                using (StreamWriter wr = new StreamWriter(CookieXml))
                {
                    xs.Serialize(wr, Container.GetCookies(new Uri(_FacebookCom)).OfType<Cookie>().ToArray());
                }
            });
            t.Start();
        }
        
        private void Load()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(CookieCollection));
                using (StreamReader rd = new StreamReader(CookieXml))
                {
                    var result = xs.Deserialize(rd) as CookieCollection;
                    if (result != null)
                    {
                        Container.Add(result);
                    }
                }
            }
            catch (Exception){}

        }
        #endregion

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
            client.Referer = _FacebookCom;
            client.Host = url.Replace("https://", "").Split('/')[0];
            client.Headers.Add("Origin", _FacebookCom);
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
