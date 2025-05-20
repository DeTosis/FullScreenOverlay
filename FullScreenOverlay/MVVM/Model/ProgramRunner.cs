using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace FullScreenOverlay.MVVM.Model; 
public static class ProgramRunner {
    public static bool TryRunProgram(string path, out string exception) {
        exception = "";
        try {
            var pStartInfo = new ProcessStartInfo() {
                FileName = path,
                UseShellExecute = true
            };
            Process p = new() {
                StartInfo = pStartInfo
            };
            p.Start();
            return true;
        }catch (Exception e) {
            exception = e.Message;
            return false;
        }
    }
}
