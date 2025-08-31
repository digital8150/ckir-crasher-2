using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ckir_crasher_2.Classes
{
    internal class ProcessHelper
    {

        // Import the necessary Windows API functions
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        private enum ThreadAccess : int
        {
            SUSPEND_RESUME = 0x0002
        }

        public static void SuspendProcess(int processId)
        {
            try
            {
                Process targetProcess = Process.GetProcessById(processId);
                foreach (ProcessThread thread in targetProcess.Threads)
                {
                    IntPtr threadHandle = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                    if (threadHandle != IntPtr.Zero)
                    {
                        SuspendThread(threadHandle);
                        CloseHandle(threadHandle);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                HandyControl.Controls.Growl.ErrorGlobal(ex.ToString());
            }
            
        }

        public static void ResumeProcess(int processId)
        {
            Process targetProcess = Process.GetProcessById(processId);
            foreach (ProcessThread thread in targetProcess.Threads)
            {
                IntPtr threadHandle = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (threadHandle != IntPtr.Zero)
                {
                    ResumeThread(threadHandle);
                    CloseHandle(threadHandle);
                }
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);


        public static void KillProcess(Process process)
        {
            try
            {
                process.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                HandyControl.Controls.Growl.ErrorGlobal(ex.ToString());
            }
        }

        // Import the necessary Windows API function
        [DllImport("kernel32.dll")]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        public static void KillProcess(int processId)
        {
            try
            {
                Process targetProcess = Process.GetProcessById(processId);
                IntPtr processHandle = targetProcess.Handle;
                TerminateProcess(processHandle, 0);
                targetProcess.WaitForExit(); // Wait for the process to exit (optional)
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                HandyControl.Controls.Growl.ErrorGlobal(ex.ToString());
            }
            
        }

    }
}