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
using System.ComponentModel;
using System.Threading;

namespace WPFComponentTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double _deltaValue = 0;
        private bool _stop;
        private double _min;
        private double _max;

        public double DeltaValue
        {
            get { return _deltaValue; }
            set
            {
                _deltaValue = value;
                OnPropertyChanged("DeltaValue");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            _min = gvGalvo.Min;
            _max = gvGalvo.Max;
            Thread t = new Thread(new ThreadStart(Update));
            t.Start();
        }

        private void Update()
        {
            _stop = false;
            DeltaValue = _min -1;
            while (!_stop)
            {
                if(DeltaValue < _min)
                    while (DeltaValue <= _max && !_stop)
                        IncrementDelta();
                else
                if (DeltaValue > _max) 
                    while (DeltaValue >= _min && !_stop)
                        DecrementDelta();
            }
        }

        private void ThreadSleep(int time = 10)
        {
            Thread.Sleep(time);
        }

        private void DecrementDelta()
        {
            DeltaValue--;
            ThreadSleep();
        }

        private void IncrementDelta()
        {
            DeltaValue++;
            ThreadSleep();
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _stop = true;
        }
    }
}
