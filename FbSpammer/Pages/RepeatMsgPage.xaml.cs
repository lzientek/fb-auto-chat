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
            
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            Model.Save();
        }

        private void AddOnClick(object sender, RoutedEventArgs e)
        {
            Model.RepeatMsgs.Add(new RepeatMsgModel());
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
