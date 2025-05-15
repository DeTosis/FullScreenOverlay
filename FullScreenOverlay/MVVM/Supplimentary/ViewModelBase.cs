using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FullScreenOverlay.MVVM.Supplimentary;
public class ViewModelBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string pName = "", object sender = null) {
        if (string.IsNullOrEmpty(pName))
            throw new System.Exception($"Empty property field [pName] from {sender}.");

        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(pName));
    }
}
