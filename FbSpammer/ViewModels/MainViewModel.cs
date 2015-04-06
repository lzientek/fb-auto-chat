using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FbChatApi;
using FbSpammer.Annotations;

namespace FbSpammer.ViewModels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        private FbUser _actualUser;
        private bool _isConnected =false;



        public FbUser ActualUser
        {
            get { return _actualUser; }
            set
            {
                if (Equals(value, _actualUser)) return;
                _actualUser = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (value.Equals(_isConnected)) return;
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
