using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookItDesktop
{
    class RegisterKeyboardInput: IDisposable
    {
        private KeyboardHook.HookDelegate keyboardDelegate;
        public IntPtr keyBoardHandle;
        private const Int32 WH_KEYBOARD_LL = 13;
        private bool disposed;
        public event EventHandler<EventArgs> KeyBoardKeyPressed;
        public int pressedKey = 0;
        private const int WM_KEYDOWN = 0x0100;
        private const int CTRL_KEY = 162;
        private const int O_KEY = 81;
        private const int W_KEY = 87;
        private const int WM_KEYUP = 0x0101;
        private const int ESC_KEY = 27;
        private bool keyHeld = false;
        private bool keyHeldO = false;
        private bool keyHeldSaveO = false;
        public bool openSave = false;
        public bool openSearch = false;
        private Window closeWindow;
        public RegisterKeyboardInput()
        {
            keyboardDelegate = KeyboardHookDelegate;
            keyBoardHandle = KeyboardHook.SetWindowsHookEx(
                WH_KEYBOARD_LL, keyboardDelegate, IntPtr.Zero, 0);
        }

        public RegisterKeyboardInput(Window i)
        {
            closeWindow = i;
            keyboardDelegate = KeyboardCloseWindow;
            keyBoardHandle = KeyboardHook.SetWindowsHookEx(
                WH_KEYBOARD_LL, keyboardDelegate, IntPtr.Zero, 0);
        }

        private IntPtr KeyboardHookDelegate(
            Int32 Code, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                openSave = false;
                openSearch = false;
                keyCodeStructure keyStructure = (keyCodeStructure)Marshal.PtrToStructure(lParam, typeof(keyCodeStructure));
                if (keyStructure.vkCode == O_KEY && keyHeld)
                {
                    keyHeldO = true;
                    keyHeldSaveO = false;

                }
                   
                if (keyStructure.vkCode == W_KEY && keyHeld)
                {
                    keyHeldSaveO = true;
                    keyHeldO = false;
                }
                    
                
                if (Code < 0)
                {
                    return KeyboardHook.CallNextHookEx(
                        keyBoardHandle, Code, wParam, lParam);
                }

                if (KeyBoardKeyPressed != null && (keyHeldO || keyHeldSaveO)&& keyHeld)
                {
                   
                    if (keyHeldO)
                    {
                        keyHeld = false;
                        keyHeldO = false;
                        openSave = true;
                        openSearch = false;
                    }
                    if (keyHeldSaveO)
                    {
                        keyHeld = false;
                        keyHeldO = false;
                        openSave = false;
                        openSearch = true;
                    }
                    
                    KeyBoardKeyPressed(this, new EventArgs());

                }
                if (keyStructure.vkCode == CTRL_KEY)
                    keyHeld = true;
                return KeyboardHook.CallNextHookEx(
                    keyBoardHandle, Code, wParam, lParam);
            }

            else if(wParam == (IntPtr)WM_KEYUP)
            {
                keyCodeStructure keyStructure = (keyCodeStructure)Marshal.PtrToStructure(lParam, typeof(keyCodeStructure));
                if(keyStructure.vkCode == CTRL_KEY)
                {
                    keyHeldO = false;
                    keyHeld = false;
                    openSearch = false;
                    openSave = false;
                    keyHeldSaveO = false;
                }
                return KeyboardHook.CallNextHookEx(
                    keyBoardHandle, Code, wParam, lParam);
            }
            else
                 return KeyboardHook.CallNextHookEx(
                        keyBoardHandle, Code, wParam, lParam);
        }
        private IntPtr KeyboardCloseWindow(
           Int32 Code, IntPtr wParam, IntPtr lParam)

        {
            if (wParam == (IntPtr)WM_KEYUP)
            {
                keyCodeStructure keyStructure = (keyCodeStructure)Marshal.PtrToStructure(lParam, typeof(keyCodeStructure));
                if (Code < 0)
                {
                    return KeyboardHook.CallNextHookEx(
                        keyBoardHandle, Code, wParam, lParam);
                }
                if (KeyBoardKeyPressed != null && keyStructure.vkCode == ESC_KEY)
                {

                    KeyBoardKeyPressed(this, new EventArgs());
                    
                    //closeWindow.Close();
                    return KeyboardHook.CallNextHookEx(
                     keyBoardHandle, Code, wParam, lParam);

                }

                else
                    return KeyboardHook.CallNextHookEx(
                            keyBoardHandle, Code, wParam, lParam);
            }
            else
            {
                return KeyboardHook.CallNextHookEx(
                      keyBoardHandle, Code, wParam, lParam);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (keyBoardHandle != IntPtr.Zero)
                {
                    KeyboardHook.UnhookWindowsHookEx(
                        keyBoardHandle);
                }

                disposed = true;
            }
        }

        ~RegisterKeyboardInput()
        {
            Dispose(false);
        }
        public struct keyCodeStructure
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public IntPtr dwExtraInfo;
        }
    }
}
