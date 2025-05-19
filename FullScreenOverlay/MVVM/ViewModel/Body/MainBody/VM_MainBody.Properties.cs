using FullScreenOverlay.MVVM.View.Body;
using System.Collections.ObjectModel;
using System.Windows;

namespace FullScreenOverlay.MVVM.ViewModel.Body.MainBody {
    public partial class VM_MainBody {

        private ObservableCollection<BodyContentItem> itemGridElements = new();
        public ObservableCollection<BodyContentItem> ItemGridElements {
            get { return itemGridElements; }
            set {
                itemGridElements = value;
                OnPropertyChanged();
            }
        }

        private bool isInEditMode = true;
        public bool IsInEditMode {
            get { return isInEditMode; }
            set {
                isInEditMode = value;
                OnPropertyChanged();
            }
        }

        private int itemGridRows = 10;
        public int ItemGridRows {
            get { return itemGridRows; }
            set {
                itemGridRows = value;
                OnPropertyChanged();
            }
        }

        private int itemGridColumns = 19;
        public int ItemGridColumns {
            get { return itemGridColumns; }
            set {
                itemGridColumns = value;
                OnPropertyChanged();
            }
        }

        private double selectionBoxLeft;
        public double SelectionBoxLeft {
            get { return selectionBoxLeft; }
            set {
                selectionBoxLeft = value;
                OnPropertyChanged();
            }
        }

        private double selectionBoxTop;
        public double SelectionBoxTop {
            get { return selectionBoxTop; }
            set {
                selectionBoxTop = value;
                OnPropertyChanged();
            }
        }

        private double selectionBoxW;
        public double SelectionBoxW {
            get { return selectionBoxW; }
            set {
                selectionBoxW = value;
                OnPropertyChanged();
            }
        }

        private double selectionBoxH;
        public double SelectionBoxH {
            get { return selectionBoxH; }
            set {
                selectionBoxH = value;
                OnPropertyChanged();
            }
        }

        private Visibility selectionBoxVisibility;
        public Visibility SelectionBoxVisibility {
            get { return selectionBoxVisibility; }
            set {
                selectionBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        private double itemGridH;
        public double ItemGridH {
            get { return itemGridH; }
            set {
                itemGridH = value;
                OnPropertyChanged();
            }
        }

        private double itemGridW;
        public double ItemGridW {
            get { return itemGridW; }
            set {
                itemGridW = value;
                OnPropertyChanged();
            }
        }
    }
}
