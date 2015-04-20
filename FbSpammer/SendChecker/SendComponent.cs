using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using FbSpammer.ViewModels;

namespace FbSpammer.SendChecker
{
    public class SendComponent
    {
        public List<SendMessageRepeat> Messages { get; set; }
        public bool IsSending { get; set; }
        private Timer _timer = new Timer(1000);


        public SendComponent()
        {
            Messages = new List<SendMessageRepeat>();
        }

        public void Start()
        {
            _timer.Elapsed += SendIfNeeded;
            _timer.Start();
        }

        public void Add(IEnumerable<RepeatMsgModel> msgModels)
        {
            foreach (var repeatMsgModel in msgModels)
            {
                Add(repeatMsgModel);
            }
        }
        public void Add(RepeatMsgModel msgModel)
        {
            Messages.Add(new SendMessageRepeat()
            {
                LastSend = new DateTime(1),
                Message = msgModel.Message,
                SendEvery = msgModel.Interval,
                UserId = msgModel.UserId,
                Id = msgModel.Id,
            });
        }

        public void Remove(RepeatMsgModel msgModel)
        {
            var m = Messages.FirstOrDefault(me => me.Id == msgModel.Id);
            if (m != null) { Messages.Remove(m); }
        }
        public void SendIfNeeded(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (IsSending) { return; }
            IsSending = true;
            Parallel.ForEach(Messages, async message =>
            {
                if (message.LastSend.Add(message.SendEvery) < DateTime.Now)
                {
                    await message.Send();
                }
            });
            IsSending = false;
        }

    }
}
