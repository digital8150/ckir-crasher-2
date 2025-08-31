using ckir_crasher_2.Classes;
using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ckir_crasher_2.Windows
{
    /// <summary>
    /// Interaction logic for Application_adv.xaml
    /// </summary>
    public partial class Application_adv : System.Windows.Window
    {
        private bool b_isAppRunning = false;
        private string temp_text = null;

        public Application_adv()
        {
            InitializeComponent();
            border_main.Height = 0;
            temp_text = textblock_notify.Text;
            textbox_logbox.Text = LogHelper.Read();
        }

        //window startup animation
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshProcessList();
            viewchanger_hideall();
            viewchanger_hideall();
            BorderController.Controll_with_animation_v(ref border_main, 600, HeightProperty);
            LogHelper.Write("정보 : 고급 사용 모드가 구성되었습니다.", ref textbox_logbox);
            sidemenuitem_mainmenu.IsSelected = true;
            border_mainmenu.Visibility = Visibility.Visible;
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
                if(temp_text != null && temp_text != "")
                {
                    textblock_notify.Text = temp_text;
                }
                
            }

            Thread processrefresher = new Thread(RefreshProcessList_thread);
            processrefresher.IsBackground = true;
            processrefresher.Start();
            textbox_logbox.Text = LogHelper.Read();
        }
        public Application_adv(bool b_isAppRunning)
        {
            InitializeComponent();
            border_main.Height = 0;
            temp_text = textblock_notify.Text;
            this.b_isAppRunning=b_isAppRunning;
        }
        private void closeapp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //viewchanger
        private void viewchanger(object sender, HandyControl.Data.FunctionEventArgs<object> e)
        {
            SideMenuItem sideMenuItem = new SideMenuItem();
            sideMenuItem = (SideMenuItem)e.Info;
            viewchanger_hideall();
            switch (sideMenuItem?.Header?.ToString())
            {
                case "쉬운 사용":
                    border_mainmenu.Visibility = Visibility.Visible;
                    break;

                case "프로세스 관리자":
                    border_process.Visibility = Visibility.Visible;
                    break;

                case "로깅 중단":
                    border_stoplogger.Visibility = Visibility.Visible;
                    break;

                case "맘아이 무력화":
                    border_momidisable.Visibility = Visibility.Visible;
                    break;

                case "환경설정":
                    border_settings.Visibility = Visibility.Visible;
                    try
                    {
                        settings_onlineFeature.IsChecked = iniFileHelper.getValue_bool("online", "b_EnableOnline");
                        settings_appRefreshRate.Value = iniFileHelper.getValue("app", "i_refreshrate").ConvertToInt();
                       
                    }
                    finally
                    {

                    }
                    break;

                case "시스템 로그":
                    border_logs.Visibility = Visibility.Visible;
                    textbox_logbox.ScrollToEnd();
                    break;

                case "SECRET에 대하여":
                    border_info.Visibility = Visibility.Visible;
                    break;


            }
        }
        private void viewchanger_hideall()
        {
            border_mainmenu.Visibility = Visibility.Hidden;
            border_momidisable.Visibility = Visibility.Hidden;
            border_process.Visibility = Visibility.Hidden;
            border_settings.Visibility = Visibility.Hidden;
            border_stoplogger.Visibility = Visibility.Hidden;
            border_settings.Visibility = Visibility.Hidden;
            border_info.Visibility = Visibility.Hidden;
            border_logs.Visibility = Visibility.Hidden;
        }

        //ezmode feature
        private void button_toggleFeature(object sender, RoutedEventArgs e)
        {
            LogHelper.Write("정보 : 쉬운 사용 모드 시작", ref textbox_logbox);
            button_goez.IsEnabled = false;
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
                button_goez.IsEnabled = true;
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
            while (index < 3)
            {


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

            Dispatcher.Invoke(() => { button_goez.IsEnabled = true; });



        }
        private void btn_goAdv(object sender, RoutedEventArgs e)
        {
            System.Windows.Window window_adv = new Application_ez(b_isAppRunning);
            window_adv.Show();
            this.Close();
        }


        //ProcessKiller Featrues

        private List<Process> runningProcesses;
        private void RefreshProcessList()
        {
            int selectedIndex = processList.SelectedIndex;
            

            processList.Items.Clear();
            runningProcesses = new List<Process>(Process.GetProcesses());

            // Sort the runningProcesses list by process name alphabetically
            runningProcesses.Sort((p1, p2) => string.Compare(p1.ProcessName, p2.ProcessName));

            foreach (var process in runningProcesses)
            {
                processList.Items.Add($"{process.ProcessName} (PID: {process.Id})");
            }

            // Restore the previous selection and view position
            if (selectedIndex >= 0 && selectedIndex < processList.Items.Count)
            {
                processList.SelectedIndex = selectedIndex;
            }

            
        }
        private void RefreshProcessList(object sender, RoutedEventArgs e)
        {
            RefreshProcessList();
            LogHelper.Write("정보 : 프로세스 목록 수동 새로고침", ref textbox_logbox);
        }
        private void RefreshProcessList_thread()
        {
            while (true)
            {
                int interval;
                Dispatcher.Invoke(() => { RefreshProcessList(); textbox_logbox.Text = LogHelper.Read(); });
                try
                {
                    interval = iniFileHelper.getValue("app", "i_refreshrate").ConvertToInt();
                } catch (Exception ex)
                {
                    LogHelper.Write($"오류 : 환경설정 : {ex} ");
                    iniFileHelper.changeValue("app", "i_refreshrate", "2000");
                    interval = 2000;
                }

                

                Thread.Sleep(interval);
            }
        }
        private void killProcess_NET(object sender, RoutedEventArgs e)
        {
            if (processList.SelectedItems.Count > 0)
            {
                foreach (var selectedItem in processList.SelectedItems)
                {
                    var processInfo = selectedItem.ToString();
                    int startIndex = processInfo.IndexOf("(PID:") + 5;
                    int endIndex = processInfo.IndexOf(")", startIndex);
                    int processId = int.Parse(processInfo.Substring(startIndex, endIndex - startIndex));

                    Process process = runningProcesses.Find(p => p.Id == processId);
                    if (process != null)
                    {
                        try
                        {
                            process.Kill();
                            LogHelper.Write($"정보 : 프로세스 킬러 : 프로세스 킬 : {process.Id.ToString()}", ref textbox_logbox);
                        }
                        catch (Exception ex)
                        {
                            HandyControl.Controls.Growl.ErrorGlobal($"Error killing process with PID {processId}: {ex.Message}");
                            LogHelper.Write($"오류 : 프로세스 킬러 : {processId} : {ex.Message}", ref textbox_logbox);
                        }
                    }
                }

                RefreshProcessList();
            }
            
        }
        private void killProcess_WIN(object sender, RoutedEventArgs e)
        {
            if (processList.SelectedItems.Count > 0)
            {
                foreach (var selectedItem in processList.SelectedItems)
                {
                    var processInfo = selectedItem.ToString();
                    int startIndex = processInfo.IndexOf("(PID:") + 5;
                    int endIndex = processInfo.IndexOf(")", startIndex);
                    int processId = int.Parse(processInfo.Substring(startIndex, endIndex - startIndex));

                    Process process = runningProcesses.Find(p => p.Id == processId);
                    if (process != null)
                    {
                        try
                        {
                            ProcessHelper.KillProcess(process.Id);
                            LogHelper.Write($"정보 : 프로세스 킬러 : 프로세스 킬(WinAPI) : {process.Id.ToString()}", ref textbox_logbox);
                        }
                        catch (Exception ex)
                        {
                            HandyControl.Controls.Growl.ErrorGlobal($"Error killing process with PID {processId}: {ex.Message}");
                            LogHelper.Write($"오류 : 프로세스 킬러 : {processId} : {ex.Message}", ref textbox_logbox);
                        }
                    }
                }

                RefreshProcessList();
            }
        }
        private void suspendProcess(object sender, RoutedEventArgs e)
        {
            if (processList.SelectedItems.Count > 0)
            {
                foreach (var selectedItem in processList.SelectedItems)
                {
                    var processInfo = selectedItem.ToString();
                    int startIndex = processInfo.IndexOf("(PID:") + 5;
                    int endIndex = processInfo.IndexOf(")", startIndex);
                    int processId = int.Parse(processInfo.Substring(startIndex, endIndex - startIndex));

                    Process process = runningProcesses.Find(p => p.Id == processId);
                    if (process != null)
                    {
                        try
                        {
                            ProcessHelper.SuspendProcess(process.Id);
                            Growl.InfoGlobal($"프로세스 일시중단  {process.ProcessName} @ PID {processId} ");
                            LogHelper.Write($"정보 : 프로세스 킬러 : 프로세스 중단(WinAPI) : {process.Id.ToString()}", ref textbox_logbox);
                        }
                        catch (Exception ex)
                        {
                            HandyControl.Controls.Growl.ErrorGlobal($"Error killing process with PID {processId}: {ex.Message}");
                            LogHelper.Write($"오류 : 프로세스 킬러 : {processId} : {ex.Message}", ref textbox_logbox);
                        }
                    }
                }

                RefreshProcessList();
            }
        }


        //Maestro Web disabler
        private void task_maestro_disable(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(task_maestro_disable);
            thread.IsBackground = true;
            thread.Start();
        }
        private void task_maestro_disable()
        {
            Dispatcher.Invoke(() =>
            {
                status_icon_stoplog.Icon = FontAwesome6.EFontAwesomeIcon.Solid_Spinner;
                status_icon_stoplog.Spin = true;
            });

            int index = 0;
            int repeat = 15;
            Dispatcher.Invoke(() =>
            {
                try
                {
                    repeat = textbox_stoplog_repeat.Text.ConvertToInt();
                } catch (Exception ex)
                {
                    Growl.ErrorGlobal($"{ex.ToString()}");
                }
                
            });
            
            
            while (index < repeat)
            {
                string target1 = null , target2 = null, target3 = null;
                Dispatcher.Invoke(() =>
                {

                    target1 = textbox_stoplog_target1.Text;
                    target2 = textbox_stoplog_target2.Text;
                    target3 = textbox_stoplog_target3.Text;
                });

                string processName = target1;
                Process[] processes = Process.GetProcessesByName(processName);
                Dispatcher.Invoke(() => { LogHelper.Write($"정보 : 마에스트로 킬러 : {processName} ( {index + 1} /  {repeat}  )", ref textbox_logbox); });
                foreach (Process process in processes)
                {
                    ProcessHelper.SuspendProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal($"process suspended : {processName} ( {index+1} /  {repeat}  ) ");
                }

                processName = target2;
                processes = Process.GetProcessesByName(processName);
                Dispatcher.Invoke(() => { LogHelper.Write($"정보 : 마에스트로 킬러 : {processName} ( {index + 1} / {repeat} )", ref textbox_logbox); });
                foreach (Process process in processes)
                {
                    ProcessHelper.SuspendProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal($"process suspended : {processName} ( {index + 1} /  {repeat}  ) ");

                }

                processName = target3;
                processes = Process.GetProcessesByName(processName);
                Dispatcher.Invoke(() => { LogHelper.Write($"정보 : 마에스트로 킬러 : {processName} ( {index + 1} / {repeat} )", ref textbox_logbox); });
                foreach (Process process in processes)
                {
                    ProcessHelper.SuspendProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal($"process suspended : {processName} ( {index + 1} / {repeat} ) ");

                }

                System.Threading.Thread.Sleep(100);
                index++;
            }


            Dispatcher.Invoke(() =>
            {
                status_icon_stoplog.Icon = FontAwesome6.EFontAwesomeIcon.Solid_CircleCheck;
                status_icon_stoplog.Spin = false;
                button_dse_stoplog.Color = Color.FromRgb(0xcc, 0xff, 0xcc);
                button_dse_stoplog.Opacity = 100;
            });


        }


        //Momi disabler
        private void task_momi_disable(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(task_momi_disable);
            thread.IsBackground = true;
            thread.Start();
        }
        private void task_momi_disable()
        {
            Dispatcher.Invoke(() =>
            {
                status_icon_momi.Icon = FontAwesome6.EFontAwesomeIcon.Solid_Spinner;
                status_icon_momi.Spin = true;
            });

            int index = 0;
            int repeat = 15;
            Dispatcher.Invoke(() =>
            {
                try
                {
                    repeat = textbox_momi_repeat.Text.ConvertToInt();
                }
                catch (Exception ex)
                {
                    Growl.ErrorGlobal($"{ex.ToString()}");
                }

            });


            while (index < repeat)
            {
                string target1 = null, target2 = null, main = null;
                Dispatcher.Invoke(() =>
                {

                    target1 = textbox_momi_target1.Text;
                    target2 = textbox_momi_target2.Text;
                    main = textbox_momi_main.Text;
                });

                string processName = main;
                Process[] processes = Process.GetProcessesByName(processName);
                Dispatcher.Invoke(() => { LogHelper.Write($"정보 : 맘아이 킬러 : {processName} ( {index + 1} / {repeat} ) suspend", ref textbox_logbox); });
                foreach (Process process in processes)
                {
                    ProcessHelper.SuspendProcess(process.Id);
                    HandyControl.Controls.Growl.InfoGlobal($"process suspended : {processName} ( {index + 1} / {repeat} ) ");

                }

                int index2 = 0;
                
                while (index2 < 30)
                {
                    processName = target1;
                    processes = Process.GetProcessesByName(processName);
                    foreach (Process process in processes)
                    {
                        ProcessHelper.KillProcess(process.Id);
                        HandyControl.Controls.Growl.InfoGlobal("process killed : " + processName);
                    }
                    index2++;
                    
                }
                Dispatcher.Invoke(() => { LogHelper.Write($"정보 : 맘아이 킬러 : {processName} ( {index + 1} / {repeat} ) kill", ref textbox_logbox); });
                index2 = 0;
                
                while (index2 < 30)
                {
                    processName = target2;
                    processes = Process.GetProcessesByName(processName);
                    foreach (Process process in processes)
                    {
                        ProcessHelper.KillProcess(process.Id);
                        HandyControl.Controls.Growl.InfoGlobal("process killed : " + processName);
                    }
                    index2++;
                    
                }
                Dispatcher.Invoke(() => { LogHelper.Write($"정보 : 맘아이 킬러 : {processName} ( {index + 1} / {repeat} ) kill", ref textbox_logbox); });

                System.Threading.Thread.Sleep(50);
                index++;
            }


            Dispatcher.Invoke(() =>
            {
                status_icon_momi.Icon = FontAwesome6.EFontAwesomeIcon.Solid_CircleCheck;
                status_icon_momi.Spin = false;
                button_dse_momi.Color = Color.FromRgb(0xcc, 0xff, 0xcc);
                button_dse_momi.Opacity = 100;
            });


        }

        //환경설정
        private void settings_cv_online(object sender, RoutedEventArgs e) //온라인 모드 변경
        {
            if (settings_onlineFeature.IsChecked == true)
            {
                iniFileHelper.changeValue("online", "b_EnableOnline", "true");
            }
            else
            {
                iniFileHelper.changeValue("online", "b_EnableOnline", "false");
            }
            
        }
        private void settings_cv_interval(object sender, RoutedPropertyChangedEventArgs<double> e) //인터벌 변경
        {
            iniFileHelper.changeValue("app", "i_refreshrate", ((int)settings_appRefreshRate.Value).ToString());
        }

        private void settings_appRefreshRate_PreviewPositionChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            iniFileHelper.changeValue("app", "i_refreshrate", ((int)settings_appRefreshRate.Value).ToString());
        }
    }
}
