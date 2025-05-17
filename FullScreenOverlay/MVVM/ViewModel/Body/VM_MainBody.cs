using FullScreenOverlay.MVVM.Supplimentary;
using FullScreenOverlay.MVVM.View.Body;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace FullScreenOverlay.MVVM.ViewModel.Body;
public class VM_MainBody : ViewModelBase {

    // *** DEBUG ***
    public VM_MainBody() {
        PopulateGrid(190);
    }

    private void PopulateGrid(int count) {
        ItemGridElements.Clear();

        for (int i = 0; i < count; i++) {
            BodyContentItem bci = new();
            bci.Height = 100;
            bci.Width = 100;

            ItemGridElements.Add(bci);
        }
    }

    // *** END_DEBUG ***

    private Point dragStartPoint;
    private ObservableCollection<BodyContentItem> itemGridElements = new();
    public ObservableCollection<BodyContentItem> ItemGridElements {
        get { return itemGridElements; }
        set {
            itemGridElements = value;
            OnPropertyChanged();
        }
    }

    private bool isInEditMode = true; //dbg
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

    bool isMouseHeld = false;
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

            VM_BodyContentItem bcidc = item.DataContext as VM_BodyContentItem;
            if (selectionRectangle.IntersectsWith(itemRect)) {
                bcidc.BorderColor = (SolidColorBrush)new BrushConverter().ConvertFromString("#00a624");
                item.Tag = true;
                if (!selectedItems.Contains(item))
                    selectedItems.Add(item);
            } 
        }
    }

    List<BodyContentItem> selectedItems = new();

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

        if (!isCollectionOverlap) {
            foreach (var i in selectedItems) {
                var bcidc = i.DataContext as VM_BodyContentItem;
                bcidc.IsCollectionSet = true;
            }
        }

        selectedItems.Clear();

        selectionCanvas.ReleaseMouseCapture();

        SelectionBoxVisibility = Visibility.Collapsed;
    }
}
