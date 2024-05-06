using System;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;

namespace NativeAudioGen.Audio
{
    public struct AudioInformation
    {
        public AudioInformation(int _samples, int _sampleRate, TimeSpan _duration, string _codec, int _channels)
        {
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

        public Audio()
        {
            GlobalFFOptions.Configure(new FFOptions { BinaryFolder = "C:\\Program Files\\ffmpeg\\bin" });
        }

        public async Task<AudioInformation?> GetAudioInformation(string file)
        {
            if (!File.Exists(file))
                return null;
            var info = await FFProbe.AnalyseAsync(file);

            return new(
                (int)Math.Round(info.Duration.TotalSeconds * info.PrimaryAudioStream.SampleRateHz),
                info.PrimaryAudioStream.SampleRateHz,
                info.Duration,
                info.PrimaryAudioStream.CodecName,
                info.PrimaryAudioStream.Channels
           );
        }

        public AudioInformation? GetAudioInformationSync(string file)
        {
            if (!File.Exists(file))
                return null;
            var info = FFProbe.Analyse(file);

            return new AudioInformation(
                (int)Math.Round(info.Duration.TotalSeconds * info.PrimaryAudioStream.SampleRateHz),
                info.PrimaryAudioStream.SampleRateHz,
                info.Duration,
                info.PrimaryAudioStream.CodecName,
                info.PrimaryAudioStream.Channels
           );
        }

        public async void ConvertAudioToWav(string file, string outputPath)
        {
            if (!File.Exists(file))
                return;
            AudioInformation? information = GetAudioInformationSync(file);
            if (information == null)
                return;
            string path = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(file) + ".wav");
            FFMpegArguments ff = FFMpegArguments
                .FromFileInput(file);

            _ =ff.OutputToFile(path, true, opt =>
            {
               opt.WithAudioSamplingRate(information.Value.sampleRate)
               .WithoutMetadata()
               .WithCustomArgument("-fflags +bitexact -flags:v +bitexact -flags:a +bitexact")
               .WithAudioCodec("pcm_s16le")
               .ForceFormat("wav")
               .UsingMultithreading(true);
                if (information.Value.channels != 1)
                    opt.WithCustomArgument("-ac 1");
            }).ProcessAsynchronously();
        }
    }
}
