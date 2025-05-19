using FullScreenOverlay.MVVM.View.Body;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FullScreenOverlay.MVVM.Model; 
public class SellectionCell : INotifyPropertyChanged {
	public SellectionCell(BodyContentItem displayItem, double size) {
		DisplayItem = displayItem;
		Size = size;
		DisplayItem.Height = size;
		DisplayItem.Width = size;
    }
    public double Size { get; set; }
    public double Top { get; set; }
    public double Left { get; set; }
    private BodyContentItem displayItem;
	public BodyContentItem DisplayItem {
		get { return displayItem; }
		set { 
			displayItem = value;
			OnPropertyChanged();
        }
	}

    public event PropertyChangedEventHandler? PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string? name = null) {
		var handler = PropertyChanged;
		handler?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
