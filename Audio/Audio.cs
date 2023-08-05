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

            //TODO: This could be cleaner i guess.
            if(information.Value.channels != 1)
            {
                _ = ff.OutputToFile(path, true, options => options
                    .WithAudioSamplingRate(information.Value.sampleRate)
                    .WithoutMetadata()
                    .WithAudioCodec("pcm_s16le")
                    .WithCustomArgument("-ac 1")
                    .WithCustomArgument("-fflags +bitexact -flags:v +bitexact -flags:a +bitexact")
                    .ForceFormat("wav")
                    .UsingMultithreading(true)
                ).ProcessAsynchronously();
                return;
            }

            if(information.Value.codec != "pcm_s16le")
            {
                _ = ff.OutputToFile(path, true, options => options
                .WithAudioCodec("pcm_s16le")
                .WithoutMetadata()
                .WithCustomArgument("-fflags +bitexact -flags:v +bitexact -flags:a +bitexact")
                .WithAudioSamplingRate(information.Value.sampleRate)
                .ForceFormat("wav")
                .UsingMultithreading(true)
                ).ProcessAsynchronously();
                return;
            }

            _ =ff.OutputToFile(path, true, options => options
            .WithoutMetadata()
            .WithCustomArgument("-fflags +bitexact -flags:v +bitexact -flags:a +bitexact")
            .UsingMultithreading(true)
            ).ProcessAsynchronously();
        }
    }
}
