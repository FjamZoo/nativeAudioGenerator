using CodeWalker.GameFiles;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NativeAudioGen.Audio
{
    public struct AwcEntry
    {
        public string FileLocation;
        public Types.AWCItem data;

        public AwcEntry(string path, AudioInformation audio)
        {
            FileLocation = path;
            data = new()
            {
                Name = Path.GetFileNameWithoutExtension(path),
                File = Path.GetFileNameWithoutExtension(path) + ".wav",
                Codec = Types.AWCCodec.ADPCM,
                Samples = audio.samples,
                SampleRate = audio.sampleRate
            };
        }
    }

    public struct AwcOptions
    {
        public bool streamFormat;
        public bool chunkIndices;

        public List<AwcEntry> entries;
    }

    public class AwcAudContainer
    {

        private Audio _audio;
        private readonly Dictionary<string, AwcEntry> m_audio = new();
        public AwcAudContainer()
        {
            _audio = new();
        }
        public async Task<AwcEntry?> AddAudio(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Unable to find file {0} at path {0}", name, path, "Unable to find file", MessageBoxButton.OK, MessageBoxImage.Error));
                return null;
            }
            if (m_audio.ContainsKey(name))
            {
                MessageBox.Show(string.Format("Audio entry with name {0} already exists!", name), "Audio already exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            AudioInformation? info = await _audio.GetAudioInformation(path);
            if (info == null)
                return null;
            AwcEntry entry = new(path, (AudioInformation)info);
            m_audio.Add(Path.GetFileNameWithoutExtension(path), entry);
            return entry;
        }

        public void UpdateAudio(string name, AwcEntry entry)
        {
            if (!m_audio.ContainsKey(name)) 
                return;
            m_audio[name] = entry;
        }

        public AwcEntry? GetAudio(string name)
        {
            if (m_audio.TryGetValue(name, out AwcEntry entry)) return entry;
            return null;
        }

        public List<string> GetAudioNames()
        {
            List<string> _names = new();
            foreach(string key in m_audio.Keys)
                _names.Add(key);
            return _names;
        }

        public void RemoveAudio(string name)
        {
            if (m_audio.ContainsKey(name))
                m_audio.Remove(name);
        }

        public void BuildAwc()
        {
            if (m_audio.Count <= 0) 
                return;
            App app = (App)Application.Current;

            Resource resource = new(app.ResourceName);
            GenerateAWCSounds(Path.Combine(resource.packPath, app.ResourceName + "_sounds"));
            resource.BuildAWC(new AwcOptions()
            {
                streamFormat = false,
                chunkIndices = true,
                entries = m_audio.Values.ToList()
            });
            resource.OpenFolder();
        }
        private void GenerateAWCSounds(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach(AwcEntry entry in m_audio.Values.ToList())
                _audio.ConvertAudioToWav(entry.FileLocation, path);
        }
    }
}
