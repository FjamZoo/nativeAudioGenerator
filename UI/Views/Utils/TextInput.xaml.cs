using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace NativeAudioGen.UI.Views.Utils
{
    /// <summary>
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class TextInput : UserControl, INotifyPropertyChanged
    {
        private string InputVal { get; set; } = "";

        public string Title { get; set; } = "";
        public string InputWidth { get; set; } = "";
        public string InputHeight { get; set; } = "";
        public bool ReadOnly { get; set; } = false;
        public string Text
        {
            get => InputVal;
            set 
            {
                if(value != InputVal)
                {
                    InputVal = value;
                    PropertyChangeNotify();
                }
            }

        }

        public TextInput()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void PropertyChangeNotify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
