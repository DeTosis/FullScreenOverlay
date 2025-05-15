using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FullScreenOverlay.MVVM.View;
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        Loaded += (s, e) => {
            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(hwnd);
            source.AddHook(WndProc);
        };
    }

    // LL HOTKEY SUPPRESSING
    private IntPtr WndProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled) {
        if (msg == WM_SYSKEYDOWN) {
            if (wParam == VK_F4 && GetKeyState(VK_MENU) != 0) {
                handled = true;
                return IntPtr.Zero;
            }
        }

        if (msg == WM_WINDOWPOSCHANGING) {
            var wp = Marshal.PtrToStructure<WINDOWPOS>(lParam);
            wp.flags |= SWP_NOMOVE | SWP_NOSIZE;
            Marshal.StructureToPtr(wp, lParam, false);
            
            handled = true;
            return IntPtr.Zero;
        }

        if (msg == WM_CLOSE) {
            App.DeactivateWindow();
            handled = true;
            return IntPtr.Zero;
        }

        return IntPtr.Zero;
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

    private const int VK_MENU               = 0x12;
    private const int VK_F4                 = 0x73;

    private const int WM_SYSKEYDOWN         = 0x0104;
    private const int WM_KEYDOWN            = 0x0100;
    private const int WM_WINDOWPOSCHANGING  = 0x0046;

    private const int SWP_NOMOVE            = 0x0002;
    private const int SWP_NOSIZE            = 0x0001;
    private const int WM_CLOSE              = 0x0010;

    [DllImport("user32.dll")]
    private static extern short GetKeyState(int nVirtKey);
}