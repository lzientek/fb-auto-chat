using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
            WindowHelper.GetMainWindow().FbApi.ConnectionEnd += FbApi_ConnectionEnd;

        }

        void FbApi_ConnectionEnd(object sender, EventArgs args)
        {
            Model.FbSmallUsers = new ObservableCollection<FbSmallUser>(
                 WindowHelper.GetMainWindow().FbApi.UserConnector.GetFriendsAsList());
        }

        private async void AddAuser(object sender, RoutedEventArgs e)
        {
            if (!Model.IsAdding)
            {
                Model.IsAdding = true;
            }
            else if (string.IsNullOrWhiteSpace(Model.AddUserId))
            {
                Model.IsAdding = false;
                MessageBox.Show(FbSpammer.Resources.UserNameOrIdNotValid);
            }
            else
            {
                try
                {
                    var window = WindowHelper.GetMainWindow();
                    var nu =
                        (await window.FbApi.UserConnector.GetUserAsync(Model.AddUserId))
                            .ToFbSmallUser();
                    if (!window.FbApi.UserConnector.Friends.ContainsKey(nu.id))
                    {
                        Model.FbSmallUsers.Add(nu);
                        window.FbApi.UserConnector.Friends.Add(nu.id, nu);
                    }
                    Model.SelectedUser = window.FbApi.UserConnector.Friends[nu.id];

                }
                catch (Exception ex)
                {
                    MessageBox.Show(FbSpammer.Resources.UserNameOrIdNotFound);
                }
                Model.IsAdding = false;
            }
        }
    }
}
