using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace NativeAudioGen
{


    /// <summary>
    /// Interaction logic for ResourceInformation.xaml
    /// </summary>
    public partial class ResourceInformation : UserControl
    {

        public ResourceInformation()
        {
            InitializeComponent();

            audioList.Items.Clear();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(e.Source.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WinForms.OpenFileDialog dialog = new()
            {
                Filter = "Audio Files|*.mp3;*.wav;*.ogg;.mp4",
                Title = "Select audio"
            };

            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                string path = dialog.FileName;
                audioList.Items.Add(Path.GetFileNameWithoutExtension(path));
            }
        }
    }
}
