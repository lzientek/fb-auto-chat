using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FbAutoChat.Core
{
    public class User
    {
        private const string UserXml = "user.xml";
        public string Mail { get; set; }
        public string Password { get; set; }

        public void Save()
        {
            var t = new Thread(() =>
            {
                XmlSerializer xs = new XmlSerializer(typeof(User));
                using (StreamWriter wr = new StreamWriter(UserXml))
                {
                    xs.Serialize(wr, this);
                }
            });
            t.Start();
        }

        public static User Load()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(User));
                using (StreamReader rd = new StreamReader(UserXml))
                {
                    var result = xs.Deserialize(rd) as User;
                    if (result != null) { return result; }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return new User();
        }
    }
}
