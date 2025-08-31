using ckir_crasher_2.Classes;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ckir_crasher_2.Windows
{
    /// <summary>
    /// Interaction logic for Application_ez.xaml
    /// </summary>
    public partial class Application_ez : Window
    {
        private bool b_isAppRunning = false;
        private string temp_text = null;

        public Application_ez()
        {
            InitializeComponent();
            border_main.Height = 0;
            temp_text = textblock_notify.Text;
        }

        public Application_ez(bool b_isAppRunning)
        {
            InitializeComponent();
            border_main.Height = 0;
            temp_text = textblock_notify.Text;
            this.b_isAppRunning = b_isAppRunning;
        }
        //openup animation
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BorderController.Controll_with_animation_v(ref border_main, 600, HeightProperty);
            if (b_isAppRunning)
            {
                status_icon.Icon = FontAwesome6.EFontAwesomeIcon.Solid_Spinner;
                status_icon.Spin = true;
                status_icon.Foreground = new SolidColorBrush(Color.FromRgb(0x1a, 0xaa, 0x1a));
                status_icon.Icon = FontAwesome6.EFontAwesomeIcon.Solid_Lock;
                status_text.Text = "ON";
                status_text.Foreground = new SolidColorBrush(Color.FromRgb(0x1a, 0xaa, 0x1a));
                button_toggle.Background = new SolidColorBrush(Color.FromArgb(255, 245, 255, 245));
                button_dse.Color = Color.FromRgb(0xcc, 0xff, 0xcc);
                button_dse.Opacity = 100;
                status_icon.Spin = false;
                textblock_notify.Text = "브라우징 경험이 안전하게 보호됩니다.";
            }
            else
            {
                b_isAppRunning = false;
                status_icon.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                status_icon.Icon = FontAwesome6.EFontAwesomeIcon.Solid_UnlockKeyhole;
                status_text.Text = "OFF";
                status_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                button_dse.Color = Color.FromRgb(0x80, 0x80, 0x80);
                button_dse.Opacity = 0.8;
                textblock_notify.Text = temp_text;
            }
        }

        private void closeapp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dragmove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button_toggleFeature(object sender, RoutedEventArgs e)
        {
            //button_goadv disable
            button_goadv.IsEnabled = false;

            if (b_isAppRunning)
            {
                //app is running
                b_isAppRunning = false;
                status_icon.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                status_icon.Icon = FontAwesome6.EFontAwesomeIcon.Solid_UnlockKeyhole;
                status_text.Text = "OFF";
                status_text.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                button_dse.Color = Color.FromRgb(0x80, 0x80, 0x80);
                button_dse.Opacity = 0.8;
                textblock_notify.Text = temp_text;
                button_goadv.IsEnabled = true;
            }
            else
            {
                System.Threading.Thread thread_app = new System.Threading.Thread(thread_changeui);
                thread_app.Start();

            }
        }

        private void thread_changeui()
        {
            Dispatcher.Invoke(() =>
            {
                status_icon.Icon = FontAwesome6.EFontAwesomeIcon.Solid_Spinner;
                status_icon.Spin = true;
            });
            
            System.Threading.Thread thread_app = new System.Threading.Thread(thread_application);
            thread_app.Start();
            thread_app.Join();

            b_isAppRunning = true;

            Dispatcher.Invoke(() =>
            {
                status_icon.Foreground = new SolidColorBrush(Color.FromRgb(0x1a, 0xaa, 0x1a));
                status_icon.Icon = FontAwesome6.EFontAwesomeIcon.Solid_Lock;
                status_text.Text = "ON";
                status_text.Foreground = new SolidColorBrush(Color.FromRgb(0x1a, 0xaa, 0x1a));
                button_toggle.Background = new SolidColorBrush(Color.FromArgb(255, 245, 255, 245));
                button_dse.Color = Color.FromRgb(0xcc, 0xff, 0xcc);
                button_dse.Opacity = 100;
                status_icon.Spin = false;
                textblock_notify.Text = "브라우징 경험이 안전하게 보호됩니다.";
            });
            
        }

        private void thread_application()
        {
            int index = 0;

            string processName = "MaestroWebSvr";
            Process[] processes = Process.GetProcessesByName(processName);
            while (index < 3){
               
                
                foreach (Process process in processes)
                {
                    ProcessHelper.SuspendProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal("process suspended : " + processName);
                }

                processName = "MaestroWebAgent";
                processes = Process.GetProcessesByName(processName);

                foreach (Process process in processes)
                {
                    ProcessHelper.SuspendProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal("process suspended: " + processName);
                
                }

                System.Threading.Thread.Sleep(300);
                index++;
            }

            processName = "qukapttp";
            processes = Process.GetProcessesByName(processName);

            foreach (Process process in processes)
            {
                ProcessHelper.SuspendProcess(process.Id);
                HandyControl.Controls.Growl.InfoGlobal("process suspended: " + processName);
            }

            index = 0;
            while (index < 30)
            {
                processName = "nfowjxyfd";
                processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes)
                {
                    ProcessHelper.KillProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal("process killed : " + processName);
                }
                index++;
                Thread.Sleep(50);
            }

            index = 0;
            while (index < 30)
            {
                processName = "nhfneczzm";
                processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes)
                {
                    ProcessHelper.KillProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal("process killed : " + processName);
                }
                index++;
                Thread.Sleep(50);
            }
            HandyControl.Controls.Growl.InfoGlobal("보호가 시작되었습니다.");

            Dispatcher.Invoke(() => { button_goadv.IsEnabled = true; });
            

           

        }

        private void btn_goAdv(object sender, RoutedEventArgs e)
        {
            System.Windows.Window window_adv = new Application_adv(b_isAppRunning);
            window_adv.Show();
            this.Close();
        }

        
    }
}
