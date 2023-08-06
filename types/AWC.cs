using System;

namespace NativeAudioGen.Types
{
    enum AWCCodec
    {
        PCM = 0,
        ADPCM = 4
    }

    struct AWCEntry
    {

        public string Name;
        public string Path;

        public AWCCodec Codec;
        public int Samples;
        public int SampleRate;
        public int Headroom = -200;

        public int PlayBegin = 0;
        public int PlayEnd = 0;

        public int LoopBegin = 0;
        public int LoopEnd = 0;
        public int LoopPoint = -1;
        public int Peak = 0;

    }
}