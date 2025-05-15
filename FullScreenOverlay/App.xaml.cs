using FullScreenOverlay.MVVM.View;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace FullScreenOverlay;

public partial class App : Application {
    private static Window mainWindow;
    private static bool isWindowActive;

    private static bool canBeClosed;
    public static bool CanBeClosed {
        get { return canBeClosed; }
        set { canBeClosed = value; }
    }


    public void ApplicationStartup(object sender, StartupEventArgs e) {
        mainWindow = new MainWindow();

        using (Process process = Process.GetCurrentProcess())
        using (ProcessModule module = process.MainModule) {
            hHook = SetWindowsHookEx(
                        WH_KEYBOARD_LL,
                        _proc,
                        GetModuleHandle(module.ModuleName),
                        0);
        }
    }

    public static void UnHook() {
        UnhookWindowsHookEx(hHook);
    }

    public static void DeactivateWindow() {
        mainWindow.Hide();
        isWindowActive = false;
    }

    static bool ctrlPressed;
    static bool shiftPressed;
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0 && wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) {
            if (CanBeClosed) {
                CanBeClosed = false;
                DeactivateWindow();
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            var skb = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            if (skb.vkCode == VK_ESCAPE && isWindowActive) {
                CanBeClosed = false;
                DeactivateWindow();
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            //OpenUp
            if (!ctrlPressed)
                ctrlPressed = skb.vkCode == VK_LCONTROL;
            if (!shiftPressed)
                shiftPressed = skb.vkCode == VK_LSHIFT;
            bool activationKeyPressed = skb.vkCode == VK_Q;

            if (ctrlPressed && shiftPressed && activationKeyPressed) {
                if (!isWindowActive) {
                    isWindowActive = true;
                    
                    mainWindow.Activate();
                    mainWindow.Show();

                    //CanBeClosed = true;
                }
            }
        }

        if (nCode >= 0 && wParam == WM_KEYUP || wParam == WM_SYSKEYUP) {
            var skb = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            if (skb.vkCode == VK_LCONTROL || skb.vkCode == VK_RCONTROL) {
                ctrlPressed = false;
            }

            if (skb.vkCode == VK_LSHIFT || skb.vkCode == VK_RSHIFT) {
                ctrlPressed = false;
            }
        }

        return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    // ***

    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT {
        public uint vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        IntPtr dwExtraInfo;
    }

    // ***

    static IntPtr hHook;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // ***
    private const int VK_ESCAPE         = 0x1B;

    private const int VK_Q              = 0x51;

    private const int VK_LSHIFT         = 0xA0;
    private const int VK_RSHIFT         = 0xA1;
    private const int VK_LCONTROL       = 0xA2;
    private const int VK_RCONTROL       = 0xA3;

    private const int WM_KEYDOWN        = 0x0100;
    private const int WM_KEYUP          = 0x0101;

    private const int WM_SYSKEYDOWN     = 0x0104;
    private const int WM_SYSKEYUP       = 0x0105;

    private const int WH_KEYBOARD_LL    = 13;
    
    // ***
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn,
        IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
}

