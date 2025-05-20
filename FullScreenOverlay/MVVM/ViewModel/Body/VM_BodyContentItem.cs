using FullScreenOverlay.MVVM.Model;
using FullScreenOverlay.MVVM.Supplimentary;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace FullScreenOverlay.MVVM.ViewModel.Body;
public partial class VM_BodyContentItem : ViewModelBase {
    public VM_BodyContentItem() {
        VM_MainFooter.EditModeChanged += OnEditModeChanged;
    }

    private void OnEditModeChanged(object? sender, bool e) {
        IsInEditMode = e;

        if (IsInEditMode) {
            BackGround = HexToSolidColorBrushConverter.Convert("#13131640");
            if (IsCollectionSet) {
                BorderColor = HexToSolidColorBrushConverter.Convert("#8D86C9");
            } else {
                BorderColor = HexToSolidColorBrushConverter.Convert("#131316");
            }
        } else {
            if (IsCollectionSet) {
                CellSeparationV = Visibility.Visible;
            }
            BackGround = Brushes.Transparent;
            BorderColor = Brushes.Transparent;
        }
    }

    public void TryStartApplication() {
        if (IsInEditMode) return;
        if (!IsCollectionSet || string.IsNullOrEmpty(FileSource)) return;

        if (!ProgramRunner.TryRunProgram(FileSource, out var e)) {
            MessageBox.Show(e);
        }
    }

    public bool SetCellDataFormat(DataFormats dataFormat) {
        if (IsCollectionSet) {
            CellDataFormat = dataFormat;
            return true;
        }
        return false;
    }

    // Interrupt Overlay Closing process 
    public void AddDataProcess() {
        if (!IsCollectionSet) return;

        App.CanBeClosed = false;
        var fileD = new OpenFileDialog();

        fileD.CheckFileExists = true;
        fileD.CheckPathExists = true;

        if (fileD.ShowDialog(Application.Current.MainWindow) != null) {
            if (!string.IsNullOrEmpty(fileD.FileName)) {
                FileSource = fileD.FileName;
                string fileName = FileSource.Split("\\").Last();
                string fnwd = fileName.Split(".").First();

                DisplayFileName = fnwd;

                FileIcon = new IconHelper().GetIcon(FileSource, Properties.Resources.undefIcon);
            }
        }

        App.CanBeClosed = true;
    }
}
