using NativeAudioGen.Audio;

namespace NativeAudioGen
{
    public partial class App : System.Windows.Application
    {
        private AwcAudContainer audContainer;

        public App()
        {
            audContainer = new AwcAudContainer();
        }


        public AwcAudContainer AudContainer
        {
            get => audContainer;
        }
        
    }
}
