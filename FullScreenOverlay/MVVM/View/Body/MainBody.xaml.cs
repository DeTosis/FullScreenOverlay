using FullScreenOverlay.MVVM.ViewModel.Body.MainBody;
using System.Windows;
using System.Windows.Controls;

namespace FullScreenOverlay.MVVM.View.Body {
    public partial class MainBody : UserControl {
        public MainBody() {
            InitializeComponent();
            SetupMouseEvents();

        }


        private void SetupMouseEvents() {
            var dataC = (VM_MainBody)DataContext;

            SelectionCanvas.MouseLeftButtonDown += (s,e) => {
                dataC.SelectionCanvasMouseLeftButtonDown(SelectionCanvas, e);
            };

            SelectionCanvas.MouseMove += (s,e) => {
                dataC.SelectionCanvasMouseMove(SelectionCanvas, e);
            };

            SelectionCanvas.MouseLeftButtonUp += dataC.SelectionCanvasMouseLeftButtonUp;
            
        }
    }
}
