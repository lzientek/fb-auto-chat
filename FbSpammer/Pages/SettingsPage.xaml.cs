using System;
using System.Collections.Generic;
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
using FbSpammer.ViewModels;

namespace FbSpammer.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {

        public SettingsViewModel Model { get { return ((SettingsViewModel) Resources["Model"]); } }

        public SettingsPage()
        {
            InitializeComponent();
            PasswordBox.Password = Model.Password;
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            Model.Password = PasswordBox.Password;
            Model.Save();
        }
    }
}
