using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace NativeAudioGen.UI.Views.Utils
{
    public partial class NumberInput : UserControl, INotifyPropertyChanged
    {
        public bool IsFloat { get; set; } = false;
        public string Title { get; set; } = "";
        public string InputWidth { get; set; } = "";
        public string InputHeight { get; set; } = "";
        public bool ReadOnly { get; set; } = false;
        public string Text
        {
            get => num.Text;
            set
            {
                if (value != num.Text)
                {
                    num.Text = value;
                    PropertyChangeNotify();
                }
            }

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void PropertyChangeNotify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NumberInput()
        {
            InitializeComponent();
            DataContext = this;

        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            bool handled = false;
            for(int i = 0; i < e.Text.Length; i++)
            {
                char c = e.Text[i];
                if (char.IsNumber(c) || (IsFloat && c == '.') || (c == '-' && i == 0))
                    continue;
                handled = true;
                break;
            }

            e.Handled = handled;
            base.OnPreviewTextInput(e);
        }
    }
}
