using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ckir_crasher_2.Classes
{
    internal class LogHelper
    {
        private static string logfile_dir = "C:\\secretapps\\";
        private static string logfile_name = "secret.logs";
        private static string logfile_comp = logfile_dir + logfile_name;

        public static void Init()
        {
            if (!Directory.Exists(logfile_dir)) Directory.CreateDirectory(logfile_dir);
            if (!File.Exists(logfile_comp)) File.Create(logfile_comp);
        }

        public static void Write(string content)
        {
            File.AppendAllText(logfile_comp, $"[{DateTime.Now.ToString()}] {content}\n");
        }

        public static void Write(string content, ref TextBox logbox)
        {
            Write(content);
            logbox.Text = Read();
        }

        public static string Read()
        {
            return File.ReadAllText(logfile_comp);
        }

        public static void Clear()
        {
            File.WriteAllText(logfile_comp, "");
        }




    }
}
