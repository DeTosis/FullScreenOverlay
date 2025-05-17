using System;
using System.Windows;
using System.Windows.Interop;

namespace FullScreenOverlay.MVVM.View;
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        Loaded += (s, e) => {
            var hwnd = new WindowInteropHelper(this).Handle;
            OnMainWindowLoaded(hwnd);
        };
    }

    public event EventHandler<nint>? MainWindowLoaded;
    private void OnMainWindowLoaded(nint hwnd) {
        var handler = MainWindowLoaded;
        handler?.Invoke(this, hwnd);
    }
}