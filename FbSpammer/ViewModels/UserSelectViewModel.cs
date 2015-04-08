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
        private bool _isAdding;
        private string _addUserId;
        private FbSmallUser _selectedUser;

        public UserSelectViewModel()
        {
            IsAdding = false;
            FbSmallUsers = new ObservableCollection<FbSmallUser>();
        }

        public string AddUserId
        {
            get { return _addUserId; }
            set
            {
                if (value == _addUserId) return;
                _addUserId = value;
                OnPropertyChanged();
            }
        }

        public FbSmallUser SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (Equals(value, _selectedUser)) return;
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdding
        {
            get { return _isAdding; }
            set
            {
                if (value.Equals(_isAdding)) return;
                _isAdding = value;
                OnPropertyChanged();
            }
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
