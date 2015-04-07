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

namespace FbSpammer.UserControls
{
    /// <summary>
    /// Logique d'interaction pour FbUserSelectUC.xaml
    /// </summary>
    public partial class FbUserSelectUC : UserControl
    {
        public UserSelectViewModel Model { get { return ((UserSelectViewModel)Resources["Model"]); } }

        public FbUserSelectUC()
        {
            InitializeComponent();
            WindowHelper.GetMainWindow().FbApi.ConnectionEnd+=FbApi_ConnectionEnd;
            
        }

        void FbApi_ConnectionEnd(object sender, EventArgs args)
        {
            Model.FbSmallUsers = new ObservableCollection<FbSmallUser>(
                 WindowHelper.GetMainWindow().FbApi.UserConnector.GetFriendsAsList());
        }
    }
}
