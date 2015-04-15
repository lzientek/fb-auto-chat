using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FbChatApi;
using FbSpammer.Annotations;
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
            FbApi_FriendLoad(null, null);
            WindowHelper.GetMainWindow().FbApi.UserConnector.FriendsLoaded += FbApi_FriendLoad;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        #region dependencies
        public string SelectedUserId
        {
            get
            {
                return (string)GetValue(SelectedUserIdProperty);
            }
            set
            {
                SetValue(SelectedUserIdProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedUserIdProperty =
            DependencyProperty.Register("SelectedUserId", typeof(string), typeof(FbUserSelectUC), new FrameworkPropertyMetadata(
            null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var uc = dependencyObject as FbUserSelectUC;
            if (uc != null)
            {
                uc.Model.SelectedUser = uc.Model.FbSmallUsers.FirstOrDefault(user => user.id == uc.SelectedUserId);
            }
        }

        #endregion

        void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedUser" && Model.SelectedUser != null)
            {
                SelectedUserId = Model.SelectedUser.id;
            }
        }

        void FbApi_FriendLoad(object sender, EventArgs args)
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
                        window.FbApi.UserConnector.Save();
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
