using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FbChatApi;
using FbSpammer.Helper;
using FbSpammer.ViewModels;

namespace FbSpammer.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class RepeatMsgPage : UserControl
    {
        public RepeatMsgViewModel Model { get { return ((RepeatMsgViewModel)Resources["Model"]); } }

        public RepeatMsgPage()
        {
            InitializeComponent();
            WindowHelper.GetMainWindow().Closing += Window_Closing;
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Model.Save();
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs e)
        {
            var obj = new RepeatMsgModel()
            {
                Interval = TimeSpanUc.TimeSpan,
                Message = MsgTextBox.Text,
                Name = NameTextBox.Text,
                UserId = ToFbUser.SelectedUserId
            };
            Model.RepeatMsgs.Add(obj);
            RepeatMsgListBox.SelectedItem = obj;
        }

        private void AddOnClick(object sender, RoutedEventArgs e)
        {
            var newObj = new RepeatMsgModel {Name = FbSpammer.Resources.Unamed};

            int i = 0;
            while (Model.RepeatMsgs.Any(m => newObj.Name == m.Name))
            {
                i++;
                newObj.Name = string.Format("{0}({1})", FbSpammer.Resources.Unamed, i);
            }
            Model.RepeatMsgs.Add(newObj);
            RepeatMsgListBox.SelectedItem = newObj;
        }

        private void DeleteOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.RepeatMsgs.Remove((RepeatMsgModel) RepeatMsgListBox.SelectedItem);
            }
            catch (Exception) { }
        }
    }
}
