using NativeAudioGen.Audio;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace NativeAudioGen
{
    public partial class App : System.Windows.Application
    {
        private AwcAudContainer audContainer;
        private string output = "";
        private string resourceName = "";

        public App()
        {
            audContainer = new AwcAudContainer();
        }

        public AwcAudContainer AudContainer
        {
            get => audContainer;
        }

        //TODO: Messagebox indiciating why value change was rejected
        public string Output
        {
            get => output;
            set {
                if(Directory.Exists(value))
                    output = value;
            }
        }

        public string ResourceName
        {
            get => resourceName;
            set
            {
               resourceName = value;
            }
        }

        public string SoundpackName = "";


        public static void ShowNonBlockingDialog(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
            => new Thread(() => MessageBox.Show(text, caption, buttons, icon)).Start();
    }
}
