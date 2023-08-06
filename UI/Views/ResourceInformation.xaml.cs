using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using NativeAudioGen.Audio;
using WinForms = System.Windows.Forms;

namespace NativeAudioGen.UI.Views
{
    public partial class ResourceInformation : UserControl
    {

        protected SoundInformation? _soundInformation;
        protected App _app;
        private readonly ObservableCollection<string> _awcEntries = new();

        public ResourceInformation()
        {
            _app = (App)Application.Current;
            InitializeComponent();
            DataContext = this;
            audioList.ItemsSource = _awcEntries;
            ResourceInput.textBox.TextChanged += OnResourceNameChanged;
            SoundpackInput.textBox.TextChanged += OnSoundpackNameChanged;
        }

        protected bool CanAddAWCEntries()
        {
            if(_app.Output == "" || !Directory.Exists(_app.Output) || ResourceInput.Text == "" || SoundpackInput.Text == "")
                return false;
            return true;
        }

        protected void RefreshAudioList()
        {
            _awcEntries.Clear();
            List<string> entries = _app.AudContainer.GetAudioNames();
            for (int i = 0; i < entries.Count; i++)
                _awcEntries.Add(entries[i]);
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            object? item = e.AddedItems[0];
            if(item == null) return;
         
            AwcItemEntry? audio = _app.AudContainer.GetAudio(item.ToString());
            if(audio == null)
            {
                MessageBox.Show("Unable to retrive audio with name " + item.ToString(), "Missing entry", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_soundInformation != null) return;
            _soundInformation = new(audio.Value.FileLocation, audio.Value);
            _soundInformation.Show();
            _soundInformation.Closed += delegate
            {
                RefreshAudioList();
                _soundInformation = null;
            };
        }

        private void OnFolderPathChanged(object sender, PropertyChangedEventArgs e)
        {
            _app.Output = OutputFolder.Path;
            AddAudioButton.IsEnabled = CanAddAWCEntries();
        }

        private void OnResourceNameChanged(object sender, TextChangedEventArgs e)
        {
            _app.ResourceName = ResourceInput.textBox.Text;
            AddAudioButton.IsEnabled = CanAddAWCEntries();

        }

        private void OnSoundpackNameChanged(object sender, TextChangedEventArgs e)
        {
            _app.SoundpackName = SoundpackInput.Text;
            AddAudioButton.IsEnabled = CanAddAWCEntries();
        }

        private async void AddAWCButton_Click(object sender, RoutedEventArgs e)
        {
            WinForms.OpenFileDialog dialog = new()
            {
                Filter = "Audio Files|*.mp3;*.wav;*.ogg;.mp4",
                Multiselect = true,
                Title = "Select audio"
            };

            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                string[] paths = dialog.FileNames;
                foreach(string path in paths)
                {
                    _awcEntries.Add(Path.GetFileNameWithoutExtension(path));
                    _ = _app.AudContainer.AddAudio(path);
                    AddDat54Button.IsEnabled = true;
                }
            }
        }

        private void BuildResourceButton_Click(object sender, RoutedEventArgs e)
        {
            _app.AudContainer.BuildAwc();
        }

        private void ClearAWCButton_Click(object sender, RoutedEventArgs e)
        {
            AddDat54Button.IsEnabled = false;
        }

        private void AddDat54Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearDat54Button_Click(object sender, RoutedEventArgs e)
        {
            _app.AudContainer.BuildAwc();
        }
    }
}
