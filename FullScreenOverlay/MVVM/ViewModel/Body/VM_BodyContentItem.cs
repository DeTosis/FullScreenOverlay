using FullScreenOverlay.MVVM.Supplimentary;
using System.Windows.Media;

namespace FullScreenOverlay.MVVM.ViewModel.Body; 
public class VM_BodyContentItem : ViewModelBase {

    public VM_BodyContentItem() {
        BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#a60000");
    }

    private SolidColorBrush borderColor;
	public SolidColorBrush BorderColor {
		get { return borderColor; }
		set { 
			borderColor = value;
			OnPropertyChanged();
		}
	}

	private bool isCollectionSet = false;
	public bool IsCollectionSet {
		get { return isCollectionSet; }
		set { isCollectionSet = value; }
	}

}
