using System.IO;
using Newtonsoft.Json;

namespace FbChatApi
{
    public class FbUser
    {
        private string _profilePicture;
        public string Id { get; set; }
        public string First_Name { get; set; }
        public string Gender { get; set; }
        public string Last_Name { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public string ProfilePicture
        {
            get
            {
                if (_profilePicture == null) { _profilePicture = GetProfilePicture(); }
                return _profilePicture;
            }
            set { _profilePicture = value; }
        }

        private string GetProfilePicture()
        {
            var web = new FbWebRequest();
            var req = web.CreateGraphGetRequest("/" + Id + "?fields=picture");
            var resp = req.GetResponse();
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                string val = reader.ReadToEnd();
                var json = JsonConvert.DeserializeObject<FbPicture>(val);
                return json.picture.data.url;
            }
        }
    }
    public class Data
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }

    public class Picture
    {
        public Data data { get; set; }
    }

    public class FbPicture
    {
        public Picture picture { get; set; }
        public string id { get; set; }
    }
}