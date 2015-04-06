using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using FbAutoChat.Core;
using FbSpammer.Annotations;

namespace FbSpammer.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        public SettingsViewModel()
        {
            var user = User.Load();
            Password = user.Password;
            Email = user.Mail;
        }

        private string _email;
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
            }
        }


        public void Save()
        {
            (new User {Mail = Email,Password = Password}).Save();
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
