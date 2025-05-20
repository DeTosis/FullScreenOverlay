using FullScreenOverlay.MVVM.ViewModel.Body;
using System.Windows.Controls;

namespace FullScreenOverlay.MVVM.View.Body {
    public partial class BodyContentItem : UserControl {
        private VM_BodyContentItem? dataContext = null;
        bool isMouseHeld = false;
        public BodyContentItem() {
            InitializeComponent();
            dataContext = CellBorder.DataContext as VM_BodyContentItem;
        }

        private void CellBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            isMouseHeld = true;
        }

        private void CellBorder_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (isMouseHeld && dataContext != null) {
                dataContext.AddDataProcess();
            }
        }
    }
}
