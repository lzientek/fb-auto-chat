using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FbChatApi;
using FbSpammer.Annotations;
using FbSpammer.Helper;

namespace FbSpammer.ViewModels
{
    public class UserSelectViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<FbSmallUser> _fbSmallUsers;

        public UserSelectViewModel()
        {
            FbSmallUsers = new ObservableCollection<FbSmallUser>();
        }

        public ObservableCollection<FbSmallUser> FbSmallUsers
        {
            get { return _fbSmallUsers; }
            set
            {
                if (Equals(value, _fbSmallUsers)) return;
                _fbSmallUsers = value;
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
