using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace NativeAudioGen.UI.Views
{
    /// <summary>
    /// Interaction logic for FolderInput.xaml
    /// </summary>
    public partial class FolderInput : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        private void PropertyChangeNotify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Title { get; set; } = "";
        public string PathVal { get; set; } = "";
        public string Path
        {
            get => PathVal;
            set
            {
                if (value != PathVal)
                {
                    PathVal = value;
                    PropertyChangeNotify();
                }
            }
        }

        public FolderInput()
        {
            InitializeComponent();
            DataContext = this;
        }



        private void FolderInput_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
                Path = dialog.SelectedPath;
        }
    }
}
