using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookItDesktop
{
    public class KeyboardHook
    {
        public delegate IntPtr HookDelegate(Int32 code, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        public static extern IntPtr SetWindowsHookEx (Int32 idHook, HookDelegate lpfn, IntPtr hmod,Int32 dwThreadId);
        [DllImport("User32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, Int32 nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        public static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);
    }
}
