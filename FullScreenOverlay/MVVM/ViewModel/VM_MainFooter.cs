using FullScreenOverlay.MVVM.Supplimentary;
using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace FullScreenOverlay.MVVM.ViewModel; 
public class VM_MainFooter : ViewModelBase{
    public RelayCommand CheckBoxChecked => new RelayCommand(execute => OnCheckBoxChecked());
    private static void OnCheckBoxChecked() {
        OnEditModeChanged();
    }

    public static event EventHandler EditModeChanged;
    private static void OnEditModeChanged() {
        var handler = EditModeChanged;
        handler?.Invoke(null, null);
    }
}
