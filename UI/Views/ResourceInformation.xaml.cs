using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NativeAudioGen
{

    struct AudioItem
    {
        public string Name;
    }

    /// <summary>
    /// Interaction logic for ResourceInformation.xaml
    /// </summary>
    public partial class ResourceInformation : UserControl
    {
        public ResourceInformation()
        {
            InitializeComponent();

            audioList.Items.Clear();
            audioList.Items.Add("test");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            audioList.Items.Add("test");
            audioList.Items.Add("test");
            audioList.Items.Add("test");
            audioList.Items.Add("test");
            audioList.Items.Add("test");
            audioList.Items.Add("test");
            audioList.Items.Add("test");

            Console.WriteLine(e.Source.ToString());
        }
    }
}
