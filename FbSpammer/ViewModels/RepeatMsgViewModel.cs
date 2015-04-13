using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FbAutoChat.Core;
using FbSpammer.Annotations;

namespace FbSpammer.ViewModels
{
    public class RepeatMsgViewModel
    {
        public ObservableCollection<RepeatMsgModel> RepeatMsgs { get; set; }
        public RepeatMsgViewModel()
        {
            RepeatMsgs = new ObservableCollection<RepeatMsgModel>(RepeatMsg.Load().ToRepeatMsgModel());
        }

        public void Save()
        {
            RepeatMsg.Save(RepeatMsgs.ToRepeatMsg());
        }

    }

    public class RepeatMsgModel : INotifyPropertyChanged
    {

        private string _userId;
        private string _message;
        private TimeSpan _interval;
        private string _name;

        #region properties

        public string UserId
        {
            get { return _userId; }
            set
            {
                if (value == _userId) return;
                _userId = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Interval
        {
            get { return _interval; }
            set
            {
                if (value.Equals(_interval)) return;
                _interval = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }


        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal static class RepeatMessageHelper
    {
        internal static IEnumerable<RepeatMsgModel> ToRepeatMsgModel(this IEnumerable<RepeatMsg> messages)
        {
            return messages.Select(m => new RepeatMsgModel
            {
                Interval = m.Interval,
                Message = m.Message,
                Name = m.Name,
                UserId = m.UserId
            });
        }
        internal static IEnumerable<RepeatMsg> ToRepeatMsg(this IEnumerable<RepeatMsgModel> messages)
        {
            return messages.Select(m => new RepeatMsg
            {
                Interval = m.Interval,
                Message = m.Message,
                Name = m.Name,
                UserId = m.UserId
            });
        }
    }
}
