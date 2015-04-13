using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace FbChatApi
{
    public class UserConnector
    {
        public string UserId { get; set; }
        public Dictionary<string, FbSmallUser> Friends { get; set; }
        public FbWebRequest WebRequest { get; set; }

        public UserConnector(FbWebRequest webRequest)
        {
            WebRequest = webRequest;
            Friends = new Dictionary<string, FbSmallUser>();
            Load();
        }

        public void LoadSomeFriends(string htmlStartPage)
        {
            var val = Helper.GetFrom(htmlStartPage, "[\"ShortProfiles\",\"setMulti\",[],[", "]]");
            var dic = JsonConvert.DeserializeObject<Dictionary<string, FbSmallUser>>(val);
            foreach (var fbSmallUser in dic)
            {
                if (!Friends.ContainsKey(fbSmallUser.Key))
                {
                    Friends.Add(fbSmallUser.Key, fbSmallUser.Value);                    
                }
            }
            OnFriendsLoaded();
            Save();
        }

        public IEnumerable<FbSmallUser> GetFriendsAsList()
        {
            return Friends.Where(pair => pair.Key != UserId).Select(pair => pair.Value).OrderBy(user => user.name);
        }

        public async Task<FbUser> GetActualUserAsync()
        {
            return await GetUserAsync(UserId);
        }

        public async Task<FbUser> GetUserAsync(string idOrName)
        {
            var req = WebRequest.CreateGraphGetRequest(idOrName);
            var rep = await req.GetResponseAsync();
            using (StreamReader reader = new StreamReader(rep.GetResponseStream()))
            {
                string val = await reader.ReadToEndAsync();
                var json = JsonConvert.DeserializeObject<FbUser>(val);
                return json;
            }
        }

        public const string FriendXmlFile = "FriendsXml.xml";

        public void Save()
        {
            var t = new Thread(() =>
            {
                XmlSerializer xs = new XmlSerializer(typeof(FbSmallUser[]));
                using (StreamWriter wr = new StreamWriter(FriendXmlFile))
                {
                    xs.Serialize(wr, GetFriendsAsList().ToArray());
                }
            });
            t.Start();
        }

        public void Load()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(FbSmallUser[]));
                using (StreamReader rd = new StreamReader(FriendXmlFile))
                {
                    var result = xs.Deserialize(rd) as FbSmallUser[];
                    if (result != null)
                    {
                        Friends =
                            result.ToDictionary(user => user.id, user=>user);
                        OnFriendsLoaded();
                    }
                }
            }
            catch (Exception) { }

        }

        private void OnFriendsLoaded()
        {
            var handler = FriendsLoaded;
            if (handler != null) handler(this, null);
        }

        public event FriendsLoaded FriendsLoaded;
    }
    public delegate void FriendsLoaded(object sender, EventArgs args);
}