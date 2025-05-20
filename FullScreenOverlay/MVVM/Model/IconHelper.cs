using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FullScreenOverlay.MVVM.Model {
    public class IconHelper {
        public ImageSource? GetIcon(string filePath, byte[]? undefImg) {
            if (filePath == null) return null;
            Icon fileIco = Icon.ExtractAssociatedIcon(filePath);

            if (fileIco == null) {
                if (undefImg == null)
                    return null;

                var fileByte = undefImg;
                using (MemoryStream ms = new MemoryStream(fileByte)) {
                    using (Bitmap bmp = new Bitmap(ms)) {
                        IntPtr hIcon = bmp.GetHicon();

                        var imageSrc = Imaging.CreateBitmapSourceFromHIcon(
                            hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                        return imageSrc;
                    }
                }


            } else {
                var imageSrc = Imaging.CreateBitmapSourceFromHIcon(
                                    fileIco.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return imageSrc;
            }
        }
    }
}
