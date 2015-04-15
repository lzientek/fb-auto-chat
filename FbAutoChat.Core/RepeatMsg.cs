using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FbAutoChat.Core
{
    [Serializable]
    public class RepeatMsg
    {
        public string UserId { get; set; }
        public string Message { get; set; }

        [XmlIgnore]
        public TimeSpan Interval { get; set; }
        public string Name { get; set; }

        [XmlElement("IntervalTicks")]
        public long IntervalTicks
        {
            get { return Interval.Ticks; }
            set { Interval = new TimeSpan(value); }
        }
        public const string RepeatMsgXmlFile = "RepeatMsg.xml";
        
        public static void Save(IEnumerable<RepeatMsg> repeatMsgs)
        {
            var t = new Thread(() =>
            {
                XmlSerializer xs = new XmlSerializer(typeof(RepeatMsg[]));
                using (StreamWriter wr = new StreamWriter(RepeatMsgXmlFile))
                {
                    xs.Serialize(wr, repeatMsgs.ToArray());
                }
            });
            t.Start();
        }


        public static IEnumerable<RepeatMsg> Load()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RepeatMsg[]));
                using (StreamReader rd = new StreamReader(RepeatMsgXmlFile))
                {
                    var result = xs.Deserialize(rd) as IEnumerable<RepeatMsg>;
                    if (result != null) { return result; }
                }
            }
            catch (Exception){}

            return new List<RepeatMsg>();
        }

    }
}
