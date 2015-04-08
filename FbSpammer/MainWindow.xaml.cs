using System;
using System.Windows;
using FbAutoChat.Core;
using FbSpammer.ViewModels;

namespace FbSpammer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {

        public MainViewModel Model { get { return ((MainViewModel)Resources["Model"]); } }

        public FbChatApi.FbChatApi FbApi { get; set; }
        public MainWindow()
        {
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



     
    }

}
