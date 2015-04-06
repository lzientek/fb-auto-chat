using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FbSpammer.Helper
{
    public static class WindowHelper
    {
        public static MainWindow GetMainWindow()
        {
            return Application.Current.MainWindow as MainWindow;
        }
    }
}
