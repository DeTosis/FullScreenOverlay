using FullScreenOverlay.MVVM.Supplimentary;
using FullScreenOverlay.MVVM.View;
using System;

namespace FullScreenOverlay.MVVM.ViewModel;
public class VM_MainFooter : ViewModelBase {
    private bool isInEditMode = false;
    public bool IsInEditMode {
        get { return isInEditMode; }
        set { 
            isInEditMode = value;
            OnPropertyChanged();
            OnCheckBoxChecked();
        }
    }
    private void OnCheckBoxChecked() {
        OnEditModeChanged(IsInEditMode);
    }

    public static event EventHandler<bool> EditModeChanged;
    private static void OnEditModeChanged(bool isInEditMode) {
        var handler = EditModeChanged;
        handler?.Invoke(null, isInEditMode);
    }
}
