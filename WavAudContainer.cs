using CodeWalker.GameFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeAudioGen
{

    public struct AwcItemEntry
    {
        public string Name;
        public string FileName;

        public string Codec;
        public int Samples;
        public int SampleRate;
        public int Headroom;
        
        public int PlayBegin;
        public int PlayEnd;

        public int LoopBegin;
        public int LoopEnd;
        public int LoopCount;
        public int Peak;
    }


    public class WavAudContainer
    {
        public WavAudContainer() {
        }
    }
}
