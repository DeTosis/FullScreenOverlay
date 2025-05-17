using FullScreenOverlay.MVVM.Supplimentary;
using System;
using System.Threading;

namespace FullScreenOverlay.MVVM.ViewModel;
public class VM_MainHeader : ViewModelBase {
    public event EventHandler<DateTime>? DateTimeUpdate;
    private void OnDateTimeUpdate(DateTime dateTime) {
        HTime = dateTime.ToString();
    }

    public VM_MainHeader() {
        Thread timeT = new(() => {
            while (true) {
                Thread.Sleep(500);
                OnDateTimeUpdate(DateTime.Now);
            }
        });

        timeT.Start();
    }

    public string OverlayName { get; } = "DT | Overlay";
    private string hTime = "Debug";
    public string HTime {
        get { return hTime; }
        set {
            hTime = value;
            OnPropertyChanged();
        }
    }

}
