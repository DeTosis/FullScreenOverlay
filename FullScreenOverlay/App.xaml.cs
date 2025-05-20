using FullScreenOverlay.MVVM.View;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FullScreenOverlay;

public partial class App : Application {
    private static MainWindow mainWindow;
    private static bool isWindowActive;

    private static bool canBeClosed;
    private static bool funcKey0;
    private static bool funcKey1;
    static bool shiftPressed;

    public static bool CanBeClosed {
        get { return canBeClosed; }
        set { canBeClosed = value; }
    }

    public void ApplicationStartup(object sender, StartupEventArgs e) {
        var p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
        if (p.Length > 1) {
            Process.GetCurrentProcess().Kill();
        }

        mainWindow = new MainWindow();
        mainWindow.MainWindowLoaded += OnMainWindowLoaded;

        using (Process process = Process.GetCurrentProcess())
        using (ProcessModule module = process.MainModule) {
            hHook = SetWindowsHookEx(
                        WH_KEYBOARD_LL,
                        _proc,
                        GetModuleHandle(module.ModuleName),
                        0);
        }
    }

    private void OnMainWindowLoaded(object? sender, nint e) {
        HwndSource source = HwndSource.FromHwnd(e);
        source.AddHook(WndProc);
    }

    // LL HOTKEY SUPPRESSING
    private IntPtr WndProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled) {
        if (msg == WM_KILLFOCUS && CanBeClosed) {
            //TODO: losse focus ony when other application is focused | currently: ineer windows can close overlay too
            DeactivateWindow();
            CanBeClosed = false;
        }

        if (msg == WM_SYSKEYDOWN) {
            if (wParam == VK_F4 && GetAsyncKeyState(VK_MENU) != 0) {
                handled = true;
                return IntPtr.Zero;
            }
        }

        if (msg == WM_WINDOWPOSCHANGING) {
            var wp = Marshal.PtrToStructure<WINDOWPOS>(lParam);
            wp.flags |= SWP_NOMOVE | SWP_NOSIZE;
            Marshal.StructureToPtr(wp, lParam, true);

            handled = true;
            return IntPtr.Zero;
        }

        if (msg == WM_CLOSE) {
            DeactivateWindow();
            handled = true;
            return IntPtr.Zero;
        }

        return IntPtr.Zero;
    }

    public static void UnHook() {
        UnhookWindowsHookEx(hHook);
    }

    public static void DeactivateWindow() {
        isWindowActive = false;
        mainWindow.Hide();
    }

    private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode < 0) return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) {
            //if (CanBeClosed) {
            //    DeactivateWindow();
            //}

            var skb = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            if (skb.vkCode == VK_ESCAPE && isWindowActive) {
                CanBeClosed = false;
                DeactivateWindow();
            }

            //Show Overlay
            if (!funcKey0)
                funcKey0 = skb.vkCode == VK_LCONTROL;

            if (!funcKey1)
                funcKey1 = skb.vkCode == VK_LSHIFT;

            bool activationKeyPressed = skb.vkCode == VK_Q;

            if (funcKey0 && activationKeyPressed) {
                if (!isWindowActive) {
                    mainWindow.Show();
                    mainWindow.Activate();
                    mainWindow.Focus();

                    CanBeClosed = true;
                    isWindowActive = true;
                }
                return 1;
            }
        }

        if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP) {
            var skb = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

            if (skb.vkCode == VK_LCONTROL) {
                funcKey0 = false;
            }

            if (skb.vkCode == VK_LSHIFT) {
                funcKey1 = false;
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

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int flags;
    }

    // ***

    static IntPtr hHook;
    private static LowLevelKeyboardProc _proc = KeyboardHookCallback;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // ***

    private const int VK_ESCAPE = 0x1B;

    private const int VK_Q = 0x51;
    private const int VK_Z = 0x5A;
    private const int VK_LWIN = 0x5B;
    private const int VK_RWIN = 0x5C;

    private const int VK_LSHIFT = 0xA0;
    private const int VK_RSHIFT = 0xA1;
    private const int VK_LCONTROL = 0xA2;
    private const int VK_RCONTROL = 0xA3;

    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_SYSKEYUP = 0x0105;

    private const int WH_KEYBOARD_LL = 13;

    private const int VK_MENU = 0x12;
    private const int VK_F4 = 0x73;

    private const int WM_WINDOWPOSCHANGING = 0x0046;

    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const int WM_CLOSE = 0x0010;
    private const int WM_KILLFOCUS = 0x0008;

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

