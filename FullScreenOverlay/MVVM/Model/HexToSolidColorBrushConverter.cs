using System.Windows.Media;

namespace FullScreenOverlay.MVVM.Model;
public static class HexToSolidColorBrushConverter {
    public static SolidColorBrush Convert(string hexValue) {
        if (string.IsNullOrEmpty(hexValue)) return null;
        var brushConvert = new BrushConverter().ConvertFromString(hexValue);
        if (brushConvert == null) return null;
        return (SolidColorBrush)brushConvert;
    }
}
