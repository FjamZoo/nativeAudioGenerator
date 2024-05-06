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
        public string Title { get; set; } = "";
        public string InputWidth { get; set; } = "";
        public string InputHeight { get; set; } = "";
        public bool ReadOnly {
            get => textBox.IsReadOnly;
            set
            {
                textBox.IsReadOnly = value;
            }
        }

        public string Text
        {
            get => textBox.Text;
            set 
            {
                if(value != textBox.Text)
                {
                    textBox.Text = value;
                    PropertyChangeNotify();
                }
            }

        }

        public TextBox textBox;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void PropertyChangeNotify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TextInput()
        {
            InitializeComponent();
            DataContext = this;
            textBox = box;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
