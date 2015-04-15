using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Btl.Controls;
using FbSpammer.Annotations;

namespace FbSpammer.UserControls
{
    /// <summary>
    /// Logique d'interaction pour TimeSpanUC.xaml
    /// </summary>
    public partial class TimeSpanUC : UserControl,INotifyPropertyChanged
    {
        public TimeSpanUC()
        {
            InitializeComponent();
        }


        public TimeSpan TimeSpan
        {
            get
            {
                return (TimeSpan)GetValue(TimeSpanProperty);
            }
            set
            {
                SetValue(TimeSpanProperty, value);
            }
        }

        public static readonly DependencyProperty TimeSpanProperty =
            DependencyProperty.Register("TimeSpan", typeof(TimeSpan?), typeof(TimeSpanUC), new FrameworkPropertyMetadata(
            null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,PropertyChangedCallback));

        private int _hours;
        private int _minutes;
        private int _secondes;

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as TimeSpanUC;
            var newVal = (TimeSpan) (dependencyPropertyChangedEventArgs.NewValue??new TimeSpan());
            if (control == null) { return;}
            control.Hours = newVal.Hours;
            control.Minutes = newVal.Minutes;
            control.Secondes = newVal.Seconds;
        }

        public int Hours
        {
            get { return _hours; }
            set
            {
                if (value == _hours) return;
                _hours = value;
                OnPropertyChanged();
            }
        }

        public int Minutes
        {
            get { return _minutes; }
            set
            {
                if (value == _minutes) return;
                _minutes = value;
                OnPropertyChanged();
            }
        }

        public int Secondes
        {
            get { return _secondes; }
            set
            {
                if (value == _secondes) return;
                _secondes = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            TimeSpan = new TimeSpan(_hours, _minutes, _secondes);
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
