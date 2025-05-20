using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace FullScreenOverlay.MVVM.ViewModel.Body {
    public partial class VM_BodyContentItem {
        public enum DataFormats {
            None,
            Application,
            WebSite,
            FileOrFolder,
            Widget //WIP
        }

        private SolidColorBrush borderColor;
        public SolidColorBrush BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush backGround;
        public SolidColorBrush BackGround {
            get { return backGround; }
            set { 
                backGround = value;
                OnPropertyChanged();
            }
        }

        private Visibility cellSeparationV = Visibility.Hidden;
        public Visibility CellSeparationV {
            get { return cellSeparationV; }
            set { 
                cellSeparationV = value;
                OnPropertyChanged();
            }
        }


        private bool isCollectionSet = false;
        public bool IsCollectionSet {
            get { return isCollectionSet; }
            set { 
                isCollectionSet = value; 
                OnPropertyChanged();
            }
        }

        private string fileSource;
        public string FileSource {
            get { return fileSource; }
            set { 
                fileSource = value; 
                OnPropertyChanged();
            }
        }

        private string displayFileName;
        public string DisplayFileName {
            get { return displayFileName; }
            set { 
                displayFileName = value;
                OnPropertyChanged();
            }
        }


        private ImageSource fileIcon;
        public ImageSource FileIcon {
            get { return fileIcon; }
            set { 
                fileIcon = value;
                OnPropertyChanged();
            }
        }

        private bool isInEditMode = false;
        public bool IsInEditMode {
            get { return isInEditMode; }
            set { 
                isInEditMode = value;
                OnPropertyChanged();
            }
        }


        private DataFormats cellDataFormat = DataFormats.None;
        public DataFormats CellDataFormat {
            get { return cellDataFormat; }
            set { cellDataFormat = value; }
        }

        private Visibility editModeCellVisibility = Visibility.Visible;
        public Visibility EditModeCellVisibility {
            get { return editModeCellVisibility; }
            set { 
                editModeCellVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
