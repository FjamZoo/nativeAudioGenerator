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

        private SoundInformation? _soundInformation;
        private App _app;
        private readonly ObservableCollection<string> _awcEntries = new();

        public string ResourceName { get; set; } = "";

        public string SoundPackname { get; set; } = "";
        public string Output { get; set; } = "";


        public ResourceInformation()
        {
            _app = (App)Application.Current;
            InitializeComponent();
            audioList.ItemsSource = _awcEntries;
        }


        private bool IsOutputVaild()
        {
            if(Output == "" || !Directory.Exists(Output))
            {
                return false;
            }
            return true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
                return;
            object? item = e.AddedItems[0];
            if(item == null)
                return;
            AwcItemEntry? audio = _app.AudContainer.GetAudio(item.ToString());
            if(audio == null)
            {
                MessageBox.Show("Unable to retrive audio with name " + item.ToString(), "Missing entry", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_soundInformation == null)
            {
                _soundInformation = new(audio.Value.FileLocation, audio.Value);
                _soundInformation.Show();
                _soundInformation.Closed += delegate
                {
                    RefreshAudioList();
                    _soundInformation = null;
                };
            }
        }


        protected void RefreshAudioList()
        {
            _awcEntries.Clear();
            List<string> entries = _app.AudContainer.GetAudioNames();
            for (int i = 0; i < entries.Count; i++) 
            {
                _awcEntries.Add(entries[i]);
            }
        }

        private void OnFolderPathChanged(object sender, PropertyChangedEventArgs e)
        {
            Output = OutputFolder.Path;
            AddAudioButton.IsEnabled = IsOutputVaild();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            WinForms.OpenFileDialog dialog = new()
            {
                Filter = "Audio Files|*.mp3;*.wav;*.ogg;.mp4",
                Title = "Select audio"
            };

            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                string path = dialog.FileName;
                _awcEntries.Add(Path.GetFileNameWithoutExtension(path));
                await _app.AudContainer.AddAudio(path);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _app.AudContainer.BuildAwc();
        }
    }
}
