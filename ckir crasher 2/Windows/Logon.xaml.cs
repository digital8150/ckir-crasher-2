using ckir_crasher_2.Classes;
using HandyControl.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;


namespace ckir_crasher_2.Windows
{
    /// <summary>
    /// Interaction logic for Logon.xaml
    /// </summary>
    public partial class Logon : System.Windows.Window
    {
        //aes info
        static string aes_key = "AXe8YwuIn1zxt3FPWTZFlAa14EHdPAdN9FaZ9RQWihc="; //44자
        static string aes_iv = "bsxnWolsAyO7kCfWuyrnqg=="; //24자

        //appinfo
        const string APP_VERSION = "2.0.5-net4.7.2";
        const string APP_VERSION_CHECK = "http://dev.codingbot.kr/ckir/latest_version.txt";
        const string APP_UPDATER_SOURCE = "http://dev.codingbot.kr/rc/ccupdater.exe";

        public Logon()
        {
            InitializeComponent();
            border_main.Height = 0;
            LogHelper.Init();
            LogHelper.Write("프로그램 구성 : 로그온 모듈 구성 성공");
        }

        public void windowsloaded(object sender, RoutedEventArgs e)
        {
            BorderController.Controll_with_animation_v(ref border_main, 600, HeightProperty);
            
            //idpw autologon
            idbox.Text = iniFileHelper.getValue("logon", "s_savedid").ToString();
            try
            {
                // Decrypt the bytes to a string.
                pwbox.Password = DecryptAES(iniFileHelper.getValue("logon", "s_savedpw"));
            } catch (Exception ex)
            {
                Growl.ErrorGlobal($"자동 로그인에 실패했습니다. : {ex.ToString()}");
                LogHelper.Write($"오류 : 자동 로그인에 실패했습니다 : {ex.ToString()}");
            }

            textblock_version_indicatior.Text = $"applicationVersion : {APP_VERSION.ToString()}";

            if (iniFileHelper.getValue("online", "b_EnableOnline") == "true")
            {
                checkupdate();
            }
            
        }

        //showpopup
        private void show_popup(string content)
        {
            popup_content.Text = content;
            BorderController.Controll_with_animation_v(ref popup_border, 300, HeightProperty);
        }
        private void hide_popup(object sender, RoutedEventArgs e)
        {
            BorderController.Controll_with_animation_v(ref popup_border, 0, HeightProperty);
        }

        //checkupdate
        private void checkupdate()
        {
            if (System.IO.File.Exists("ccupdater.exe"))
            {

                foreach (Process proc in Process.GetProcessesByName("ccupdater"))
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                try
                {
                    System.IO.File.Delete("ccupdater.exe");
                } catch(Exception ex)
                {

                }
                
            }

            WebClient webclient = new WebClient();
            string latest_version = webclient.DownloadString(APP_VERSION_CHECK);
            if (latest_version != APP_VERSION)
            {
                //업데이트 필요!!!
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileCompleted += DownloadCompleted;
                    client.DownloadFileAsync(new Uri(APP_UPDATER_SOURCE), "ccupdater.exe");
                }

            }
        }
        private void DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Process.Start("ccupdater.exe");
        }


        //Exit app
        public void ExitApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Logon Button Click
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string login_info = idbox.Text + pwbox.Password;
            bool b_isLogin = SignWithPHP.login_with_php(login_info);
            bool b_isCert = SignWithPHP.cert_with_php(login_info);

            if (b_isCert && b_isLogin)
            {
                //login success
                iniFileHelper.changeValue("logon", "s_savedid", idbox.Text); // saveid 
                try
                {
                    iniFileHelper.changeValue("logon", "s_savedpw", EncryptAES(pwbox.Password));
                }catch (Exception ex)
                {
                    Growl.ErrorGlobal($"자동 로그인 저장에 실패했습니다  : {ex} ");
                    LogHelper.Write($"오류 : 자동 로그인 저장에 실패했습니다 : {ex}");
                }

                System.Windows.Window window_application = new Application_ez();
                window_application.Show();
                this.Close();
            }
            else if (b_isLogin)
            {
                //login success but cert fail
                System.Windows.Window window_activation = new Activation(idbox.Text, pwbox.Password);
                window_activation.Show();
            }
            else
            {
                //login fail
                show_popup("아이디, 비밀번호를 확인하세요");
                LogHelper.Write($"오류 : 아이디, 비밀번호를 확인하세요");
            }
        }

        //Register Button Click
        private void show_register(object sender, RoutedEventArgs e)
        {
            BorderController.Controll_with_animation(ref border_reg, 400, WidthProperty);
        }
        private void close_register(object sender, RoutedEventArgs e)
        {
            BorderController.Controll_with_animation(ref border_reg, 0, WidthProperty);
        }
        private void register(object sender, RoutedEventArgs e)
        {
            string reg_info = register_id.Text + register_pw.Password;
            if (register_pw.Password != register_pwc.Password)
            {
                show_popup("비밀번호 확인이 올바르지 않습니다");
                LogHelper.Write($"오류 : 비밀번호 확인이 올바르지 않습니다");
                return;
            }

            if (SignWithPHP.login_with_php(reg_info))
            {
                show_popup("이미 존재하는 회원정보 입니다.");
                LogHelper.Write($"오류 : 이미 존재하는 회원정보 입니다.");
                return;
            }

            if (register_id.Text == "" || register_pw.Password == "" || register_pwc.Password == "")
            {
                show_popup("비어있는 칸이 있습니다.");
                return;
            }

            bool b_isReg = SignWithPHP.register_with_php(reg_info);
            if (b_isReg)
            {
                BorderController.Controll_with_animation(ref border_reg, 0, WidthProperty);
                show_popup("성공적으로 회원가입 했습니다.");
                LogHelper.Write($"정보 : 성공적으로 회원가입 했습니다");
            }
            else
            {
                show_popup("서버와 통신에 실패했습니다.");
                LogHelper.Write($"오류 : 서버와 통신에 실패했습니다.");
            }

        }

        //dragmove
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //AES Encrypt feature
        public static string EncryptAES(string plainText)
        {
            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = Convert.FromBase64String(aes_key);
                aes.IV = Convert.FromBase64String(aes_iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptAES(string encryptedText)
        {
            string decrypted = null;
            byte[] cipher = Convert.FromBase64String(encryptedText);

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256; //AES256
                aes.BlockSize = 128;
                aes.Key = Convert.FromBase64String(aes_key);
                aes.IV = Convert.FromBase64String(aes_iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }
    }
}