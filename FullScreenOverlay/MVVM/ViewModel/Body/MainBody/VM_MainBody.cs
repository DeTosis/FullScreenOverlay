using FullScreenOverlay.MVVM.Model;
using FullScreenOverlay.MVVM.Supplimentary;
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
    private bool isMouseHeld = false;
    private double cellSize = 80;
    private double drawOffset;

    private ObservableCollection<BgRectangle> rects = new();
    List<BodyContentItem> selectedItems = new();

    public ObservableCollection<BgRectangle> Rects {
        get { return rects; }
        set { rects = value; }
    }

    public VM_MainBody() {
        PopulateGrid();
    }

    private void PopulateGrid() {
        int cellCount = -1;
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;

        int cellCountVertical = Convert.ToInt32(Math.Round(screenHeight / cellSize, MidpointRounding.ToZero)) - 1;
        int cellCountHorizontal = Convert.ToInt32(Math.Round(screenWidth / cellSize, MidpointRounding.ToZero));

        cellCount = cellCountHorizontal * cellCountVertical;

        ItemGridElements.Clear();

        double positionX = 0d;
        double positionY = 0d;

        for (int i = 0; i < cellCount; i++) {
            if (i % cellCountVertical == 0 && i != 0) {
                positionY++;
                positionX = 0;
            } else if (i != 0) {
                positionX++;
            }

            BodyContentItem bci = new();
            SellectionCell cell = new(bci, cellSize) {
                Top = positionX * cellSize,
                Left = positionY * cellSize,
            };

            ItemGridElements.Add(cell);
        }
    }

    internal void SelectionCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        if (!isInEditMode) return;
        Canvas selectionCanvas = (Canvas)sender;

        var itemPos = selectionCanvas.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
        drawOffset = itemPos.Y;

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
        ProcessSellection(selectionCanvas, selectionRectangle);
    }

    private void ProcessSellection(Canvas selectionCanvas, Rect selectionRectangle) {
        foreach (var item in ItemGridElements) {
            var itemPos = item.DisplayItem.TransformToAncestor(selectionCanvas).Transform(new Point(0, 0));
            Rect itemRect = new Rect(itemPos, new Size(item.DisplayItem.ActualWidth, item.DisplayItem.ActualHeight));

            try {
                VM_BodyContentItem? bcidc = item.DisplayItem.DataContext as VM_BodyContentItem;
                if (bcidc == null) continue;

                if (selectionRectangle.IntersectsWith(itemRect)) {
                    bcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#8D86C9");
                    item.DisplayItem.Tag = true;
                    if (!selectedItems.Contains(item.DisplayItem))
                        selectedItems.Add(item.DisplayItem);
                } else {
                    if (!bcidc.IsCollectionSet) {
                        bcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#131316");
                        try {
                            selectedItems.Remove(item.DisplayItem);
                        } catch { continue; }
                    }
                }
            } catch { continue; }
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

                    if (!xbcidc.IsCollectionSet)
                        xbcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#a60000");
                }
                break;
            }
        }

        Point topLeft = new(int.MaxValue, int.MaxValue);
        Point bottomRight = new(-1, -1);

        //GRID CONFERMED
        if (!isCollectionOverlap && selectedItems.Count > 0) {
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

            bottomRight.X += cellSize - 5;
            bottomRight.Y += cellSize - drawOffset - 2;

            var rect = new Rect(topLeft, bottomRight);
            BgRectangle rectVM = new(rect);

            Rects.Add(rectVM);
        }

        selectedItems.Clear();
        selectionCanvas.ReleaseMouseCapture();

        SelectionBoxVisibility = Visibility.Collapsed;
    }
}
