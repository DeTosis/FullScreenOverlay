using FullScreenOverlay.MVVM.Model;
using FullScreenOverlay.MVVM.Supplimentary;
using FullScreenOverlay.MVVM.View;
using FullScreenOverlay.MVVM.View.Body;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FullScreenOverlay.MVVM.ViewModel.Body.MainBody;
public partial class VM_MainBody : ViewModelBase {

    private Point dragStartPoint;
    bool isMouseHeld = false;
    double cellH = 100;
    double cellW = 100;
    List<BodyContentItem> selectedItems = new();
    private ObservableCollection<BgRectangle> rects = new();

    private int drawOffset = 52; // FOR SOME REASON ????

    public ObservableCollection<BgRectangle> Rects {
        get { return rects; }
        set { rects = value; }
    }
    // *** DEBUG ***
    public VM_MainBody() {
        PopulateGrid(190);

        ItemGridH = cellH * ItemGridRows;
        ItemGridW = cellW * ItemGridColumns;
    }

    private void PopulateGrid(int count) {
        ItemGridElements.Clear();

        for (int i = 0; i < count; i++) {
            BodyContentItem bci = new();
            bci.Height = cellH;
            bci.Width = cellW;

            Point topLeft = new Point(0,0);

            ItemGridElements.Add(bci);
        }
    }

    internal void SelectionCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        if (!isInEditMode) return;

        Canvas selectionCanvas = (Canvas)sender;

        dragStartPoint = e.GetPosition(selectionCanvas);
        SelectionBoxLeft = dragStartPoint.X;
        SelectionBoxTop = dragStartPoint.Y;
        SelectionBoxW = 0;
        SelectionBoxH = 0;
        SelectionBoxVisibility = Visibility.Visible;

        selectionCanvas.CaptureMouse();
        isMouseHeld = true;
    }

    internal void SelectionCanvasMouseMove(object sender, MouseEventArgs e) {
        if (!isInEditMode) return;
        if (!isMouseHeld) return;

        Canvas selectionCanvas = (Canvas)sender;
        Point currentPoint = e.GetPosition(selectionCanvas);

        double posX = Math.Min(currentPoint.X, dragStartPoint.X);
        double posY = Math.Min(currentPoint.Y, dragStartPoint.Y);
        double width = Math.Abs(currentPoint.X - dragStartPoint.X);
        double height = Math.Abs(currentPoint.Y - dragStartPoint.Y);

        SelectionBoxLeft = posX;
        SelectionBoxTop = posY;
        SelectionBoxW = width;
        SelectionBoxH = height;

        Rect selectionRectangle = new Rect(posX, posY, width, height);
        foreach (BodyContentItem item in ItemGridElements) {
            var itemPos = item.TransformToAncestor(selectionCanvas).Transform(new Point(0, 0));
            Rect itemRect = new Rect(itemPos, new Size(item.ActualWidth, item.ActualHeight));

            try {
                VM_BodyContentItem? bcidc = item.DataContext as VM_BodyContentItem;
                if (bcidc == null) continue;

                if (selectionRectangle.IntersectsWith(itemRect)) {
                    bcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#00a624");
                    item.Tag = true;
                    if (!selectedItems.Contains(item))
                        selectedItems.Add(item);
                } else {
                    if (!bcidc.IsCollectionSet) {
                        bcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#a60000");
                    }
                }
            }catch { continue; }
        }
    }
    internal void SelectionCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
        if (!isInEditMode) return;
        isMouseHeld = false;

        Canvas selectionCanvas = (Canvas)sender;

        bool isCollectionOverlap = false;
        foreach (var i in selectedItems) {
            var bcidc = i.DataContext as VM_BodyContentItem;

            if (bcidc.IsCollectionSet) {
                isCollectionOverlap = true;

                foreach (var x in selectedItems) {          
                    var xbcidc = x.DataContext as VM_BodyContentItem;

                    if(!xbcidc.IsCollectionSet)
                        xbcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#a60000");
                }
                break;
            }
        }

        Point topLeft = new(int.MaxValue,int.MaxValue);
        Point bottomRight = new(-1,-1);

        //GRID CONFERMED
        if (!isCollectionOverlap) {
            foreach (var i in selectedItems) {
                var bcidc = i.DataContext as VM_BodyContentItem;
                bcidc.IsCollectionSet = true;

                Point currentP = i.TransformToAncestor(Window.GetWindow(Application.Current.MainWindow)).Transform(new Point(0, 0));
                topLeft.X = Math.Min(topLeft.X, currentP.X);
                topLeft.Y = Math.Min(topLeft.Y, currentP.Y);

                bottomRight.X = Math.Max(bottomRight.X, currentP.X);
                bottomRight.Y = Math.Max(bottomRight.Y, currentP.Y);
            }

            topLeft.X -= 3;
            topLeft.Y -= drawOffset;

            bottomRight.X += cellH - 5;
            bottomRight.Y += cellW - drawOffset - 2;

            var rect = new Rect(topLeft,bottomRight);
            BgRectangle rectVM = new(rect);

            Rects.Add(rectVM);
        }

        selectedItems.Clear();
        selectionCanvas.ReleaseMouseCapture();

        SelectionBoxVisibility = Visibility.Collapsed;
    }
}
