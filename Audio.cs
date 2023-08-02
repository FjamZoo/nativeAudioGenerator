using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;

namespace NativeAudioGen
{
    public struct AudioInformation
    {
        public AudioInformation(int _samples, int _sampleRate, TimeSpan _duration, string _codec, int _channels) {
           samples = _samples;
           sampleRate = _sampleRate;
           duration = _duration;
           codec = _codec;
           channels = _channels;
        }

        public int samples;
        public int sampleRate;
        public TimeSpan duration;
        public string codec;
        public int channels;
    }

    public class Audio
    {

        public Audio() { }


        public static async Task<AudioInformation?> GetAudioInformation(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }
            var info = await FFProbe.AnalyseAsync(file);
            if (info == null || info.PrimaryAudioStream == null)
            {
                return null;
            }

            return new(
                (int)Math.Round(info.Duration.TotalSeconds * info.PrimaryAudioStream.SampleRateHz),
                info.PrimaryAudioStream.SampleRateHz,
                info.Duration,
                info.PrimaryAudioStream.CodecName,
                info.PrimaryAudioStream.Channels
           );
        }


        public static async void ConvertAudioToWav(string file)
        {
            if (!File.Exists(file))
                return;
            AudioInformation? information = await GetAudioInformation(file);
            if (information == null)
                return;

            var output = FFMpegArguments
                .FromFileInput(file)
                .OutputToFile(Path.ChangeExtension(file, ".wav"), false, options => options
                .WithAudioCodec("pcm_s16le")
                .WithAudioBitrate(16)
                .WithAudioSamplingRate(information.Value.sampleRate));
        }
    }
}
