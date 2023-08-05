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

        private AwcItemEntry awcEntry { get; set; }

        public SoundInformation()
        {
            _app = (App)Application.Current;
            InitializeComponent();
            awcEntry = new AwcItemEntry();
        }

        public SoundInformation(string path, AwcItemEntry awcEntry)
        {
            _app = (App)Application.Current;
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
            SamplesText.Text = awcEntry.Samples.ToString();
            SampleRateText.Text = awcEntry.SampleRate.ToString();
            DurationText.Text = TimeSpan.FromSeconds(awcEntry.Samples / awcEntry.SampleRate).ToString();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AwcItemEntry newEntry = awcEntry;
            newEntry.Headroom = Convert.ToInt32(HeadroomEntry.Text);
            newEntry.PlayBegin = Convert.ToInt32(PlayBeginEntry.Text);
            newEntry.PlayEnd = Convert.ToInt32(PlayEndEntry.Text);
            newEntry.LoopBegin = Convert.ToInt32(LoopBeginEntry.Text);
            newEntry.LoopEnd = Convert.ToInt32(LoopEndEntry.Text);
            newEntry.LoopPoint = Convert.ToInt32(LoopPointEntry.Text);
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
