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

namespace TimerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            InitializeComponent();

            _model = new Model();
            this.DataContext = _model;
            this.ResizeMode = ResizeMode.NoResize;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button buttonSelected = sender as System.Windows.Controls.Button;

            if (buttonSelected.Name == "MMTimerStart_Button")
            {
                if (_model.MMTimerStart(true) == true)
                {
                    MMTimerStart_Button.IsEnabled = false;
                    MMTimerStop_Button.IsEnabled = true;
                }
            }
            else if (buttonSelected.Name == "MMTimerStop_Button")
            {
                if (_model.MMTimerStart(false) == true)
                {
                    MMTimerStart_Button.IsEnabled = true;
                    MMTimerStop_Button.IsEnabled = false;
                }
            }
            else if (buttonSelected.Name == "NETDispatchTimerStart_Button")
            {
                if (_model.NETDispatchTimerStart(true) == true)
                {
                    NETDispatchTimerStart_Button.IsEnabled = false;
                    NETDispatchTimerStop_Button.IsEnabled = true;
                }
            }
            else if (buttonSelected.Name == "NETDispatchTimerStop_Button")
            {
                if (_model.NETDispatchTimerStart(false) == true)
                {
                    NETDispatchTimerStart_Button.IsEnabled = true;
                    NETDispatchTimerStop_Button.IsEnabled = false;
                }
            }
            else if (buttonSelected.Name == "NETTimerTimerStart_Button")
            {
                if (_model.NETTimerTimerStart(true) == true)
                {
                    NETTimerTimerStart_Button.IsEnabled = false;
                    NETTimerTimerStop_Button.IsEnabled = true;
                }
            }
            else if (buttonSelected.Name == "NETTimerTimerStop_Button")
            {
                if (_model.NETTimerTimerStart(false) == true)
                {
                    NETTimerTimerStart_Button.IsEnabled = true;
                    NETTimerTimerStop_Button.IsEnabled = false;
                }
            }
            else if (buttonSelected.Name == "NETThreadSleepStart_Button")
            {
                if (_model.NetThreadSleepTimerStart(true) == true)
                {
                    NETThreadSleepStart_Button.IsEnabled = false;
                    NETThreadSleepStop_Button.IsEnabled = true;
                }
            }
            else if (buttonSelected.Name == "NETThreadSleepStop_Button")
            {
                if (_model.NetThreadSleepTimerStart(false) == true)
                {
                    NETThreadSleepStart_Button.IsEnabled = true;
                    NETThreadSleepStop_Button.IsEnabled = false;
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.StopTimers();
        }

    }
}
