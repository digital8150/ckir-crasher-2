using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ckir_crasher_2.Classes
{
    internal class SignWithPHP
    {
        //members
        private static string PHP_LOGIN = "http://dev.codingbot.kr/rc/idpw.php?id=";
        private static string PHP_REG = "http://dev.codingbot.kr/rc/regidpw.php?id=";
        private static string PHP_CERT = "http://dev.codingbot.kr/rc/approve.php?id=";
        private static string PHP_addapprove = "http://dev.codingbot.kr/rc/addapprove.php?id=";
        private static string PHP_check_key_avb = "http://dev.codingbot.kr/rc/check_key_avb.php?id=";
        private static string PHP_check_key_used = "http://dev.codingbot.kr/rc/check_key_used.php?id=";
        private static string PHP_add_key_used = "http://dev.codingbot.kr/rc/add_key_used.php?id=";

        public static short error_type_key = -1;


        

        //Key Activation Modules
        public static bool check_isKeyAvailable(string serial)
        {
            WebClient webclient = new WebClient();

            string url = webclient.DownloadString(PHP_check_key_avb + serial);
            string[] parts = url.Split('|');

            if (url.Contains("OK"))
            {
                //approved key
                url = webclient.DownloadString(PHP_check_key_used + serial);
                parts = url.Split('|');

                if (url.Contains("OK"))
                {
                    //already used
                    error_type_key = 2;
                    webclient.Dispose();
                    return false;
                }
                else
                {
                    //available key
                    webclient.Dispose();
                    return true;
                }

            }
            else
            {
                //invalid key
                error_type_key = 1;
                webclient.Dispose();
                return false;
            }

        }
        public static bool add_key_used(string serial)
        {
            WebClient webclient = new WebClient();
            string url = webclient.DownloadString(PHP_add_key_used + serial);
            string[] parts = url.Split('|');

            if (url.Contains("OK"))
            {
                webclient.Dispose();
                return true;
            }
            else
            {
                webclient.Dispose();
                return false;
            }


        }
        public static bool add_approve(string serial)
        {
            serial = SHA256Hash(serial);
            WebClient webclient = new WebClient();

            string url = webclient.DownloadString(PHP_addapprove + serial);
            string[] parts = url.Split('|');
            if (url.Contains("OK"))
            {
                webclient.Dispose();
                return true;
            }
            else
            {
                webclient.Dispose();
                return false;
            }

        }


        //Signon Modules
        public static bool login_with_php(string login_info)
        {
            login_info = SHA256Hash(login_info);
            WebClient webclient = new WebClient();

            string url = webclient.DownloadString(PHP_LOGIN + login_info);
            string[] parts = url.Split('|');

            if (url.Contains("OK"))
            {
                webclient.Dispose();
                return true;
            }
            else
            {
                webclient.Dispose();
                return false;
            }
        }
        public static bool cert_with_php(string login_info)
        {
            login_info = SHA256Hash(login_info);
            WebClient webclient = new WebClient();

            string url = webclient.DownloadString(PHP_CERT + login_info);
            string[] parts = url.Split('|');

            if (url.Contains("OK"))
            {
                webclient.Dispose();
                return true;

            }
            else
            {
                webclient.Dispose();
                return false;
            }
        }
        public static bool register_with_php(string reg_info)
        {
            reg_info = SHA256Hash(reg_info);
            WebClient webclient = new WebClient();

            string url = webclient.DownloadString(PHP_REG + reg_info);
            string[] parts = url.Split('|');

            if (url.Contains("성공적"))
            {
                webclient.Dispose();
                return true;

            }
            else
            {
                webclient.Dispose();
                return false;
            }
        }

        //SHA-256 Feature
        public static string SHA256Hash(string data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
    }
}
