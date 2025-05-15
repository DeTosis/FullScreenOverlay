using FullScreenOverlay.MVVM.Supplimentary;
using System;

namespace FullScreenOverlay.MVVM.ViewModel; 
public class VM_MainWindow : ViewModelBase {
    public VM_MainWindow() {
		ScreenHeight = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight);
        ScreenWidth = Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth);
    }

    private int screenHeight;
	public int ScreenHeight {
		get { return screenHeight; }
		set { 
			screenHeight = value; 
			OnPropertyChanged(); 
		}
	}

	private int screenWidth;
	public int ScreenWidth {
		get { return screenWidth; }
		set { 
			screenWidth = value;
			OnPropertyChanged();
		}
	}
}
