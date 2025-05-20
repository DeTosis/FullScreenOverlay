using FullScreenOverlay.MVVM.ViewModel.Body;
using System.Windows;
using System.Windows.Controls;

namespace FullScreenOverlay.MVVM.View.Body {
    public partial class BodyContentItem : UserControl {
        private VM_BodyContentItem? dataContext = null;
        bool cellBorderMouseHeld = false;
        bool cellGridMouseHeld = false;
        public BodyContentItem() {
            InitializeComponent();
            dataContext = CellBorder.DataContext as VM_BodyContentItem;
        }

        private void CellBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            cellBorderMouseHeld = true;
        }

        private void CellBorder_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (cellBorderMouseHeld && dataContext != null) {
                dataContext.AddDataProcess();
                cellBorderMouseHeld = false;
            }
        }

        private void ItemGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            cellGridMouseHeld = true;
        }

        private void ItemGrid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (cellGridMouseHeld) {
                dataContext.TryStartApplication();
                cellGridMouseHeld = false;
            }
        }
    }
}
