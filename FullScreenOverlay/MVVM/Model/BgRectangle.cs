using System.Windows;

namespace FullScreenOverlay.MVVM.Model {
    public class BgRectangle {
        public Rect Rect { get; set; }
        public BgRectangle(Rect rect) {
            Rect = rect;
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
