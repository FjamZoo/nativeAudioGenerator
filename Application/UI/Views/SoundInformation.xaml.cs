using System;
using System.IO;
using System.Windows;
using NativeAudioGen.Audio;

namespace NativeAudioGen.UI.Views
{
    public partial class SoundInformation : Window
    {

        private App _app;

        private string path { get; set; } = string.Empty;

        private AwcEntry awcEntry { get; set; }

        public SoundInformation()
        {
            _app = (App)System.Windows.Application.Current;
            InitializeComponent();
            awcEntry = new AwcEntry();
        }

        public SoundInformation(string path, AwcEntry awcEntry)
        {
            _app = (App)System.Windows.Application.Current;
            this.path = path;
            this.awcEntry = awcEntry;
            InitializeComponent();
            UpdateFields();
        }

        private void UpdateFields()
        {
            SoundName.Text = Path.GetFileNameWithoutExtension(path);
            Title = string.Format("Editing {0}...", SoundName.Text);
            FileName.Text = Path.GetFileName(path);
            SamplesText.Text = awcEntry.data.Samples.ToString();
            SampleRateText.Text = awcEntry.data.SampleRate.ToString();
            DurationText.Text = TimeSpan.FromSeconds(awcEntry.data.Samples / awcEntry.data.SampleRate).ToString();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AwcEntry newEntry = awcEntry;
            newEntry.data.Headroom = Convert.ToInt32(HeadroomEntry.Text);
            newEntry.data.PlayBegin = Convert.ToInt32(PlayBeginEntry.Text);
            newEntry.data.PlayEnd = Convert.ToInt32(PlayEndEntry.Text);
            newEntry.data.LoopBegin = Convert.ToInt32(LoopBeginEntry.Text);
            newEntry.data.LoopEnd = Convert.ToInt32(LoopEndEntry.Text);
            newEntry.data.LoopPoint = Convert.ToInt32(LoopPointEntry.Text);
            _app.AudContainer.UpdateAudio(SoundName.Text, newEntry);
            MessageBox.Show("Successfully updated entry", "Success");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(string.Format("Are you sure you want to delete {0}?", SoundName.Text), "Delete " + SoundName.Text + "?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _app.AudContainer.RemoveAudio(SoundName.Text);
                this.Close();
            }
        }
    }
}
