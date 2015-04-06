using System.Windows;
using FbAutoChat.Core;
using FbSpammer.ViewModels;

namespace FbSpammer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Elysium.Controls.Window
    {

        public MainViewModel Model { get { return ((MainViewModel)Resources["Model"]); } }

        public FbChatApi.FbChatApi FbApi { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Connect();
        }

        public async void Connect()
        {
            var usr = User.Load();
            FbApi = FbChatConnector.Load(usr);
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
