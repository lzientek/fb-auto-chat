using System;
using System.Windows;
using System.Windows.Controls;
using FbAutoChat.Core;
using FbSpammer.Helper;
using FbSpammer.SendChecker;
using FbSpammer.ViewModels;

namespace FbSpammer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainViewModel Model { get { return ((MainViewModel)Resources["Model"]); } }
        public SendComponent SendComponent { get; set; }
        public FbChatApi.FbChatApi FbApi { get; set; }
        public MainWindow()
        {
            SendComponent = new SendComponent();
            var usr = User.Load();
            FbApi = FbChatConnector.Load(usr);
            InitializeComponent();
            Connect();

        }

        public async void Connect()
        {

            var result = await FbApi.Login();
            if (result)
            {
                //MessageBox.Show(result ? "Connected" : "Error check internet or password and login");
                Model.ActualUser = await FbApi.UserConnector.GetActualUserAsync();
                Model.IsConnected = true;
            }
        }


        private void StartSend_OnClick(object sender, RoutedEventArgs e)
        {
            if (SendComponent.IsStarted)
            {
                SendComponent.Stop();
                (sender as Button).Content = "Start";
            }
            else
            {
                SendComponent.Clear();
                SendComponent.Add(RepeatMsgPage.Model.RepeatMsgs);

                SendComponent.Start();
                (sender as Button).Content = "Stop";
            }
        }
    }

}
