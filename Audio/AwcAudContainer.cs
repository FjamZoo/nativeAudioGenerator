using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;

namespace NativeAudioGen.Audio
{
    public struct AwcItemEntry
    {
        public string FileLocation;
        public string Name;
        public string FileName;

        public string Codec;
        public int Samples;
        public int SampleRate;
        public int Headroom = -200;

        public int PlayBegin = 0;
        public int PlayEnd = 0;

        public int LoopBegin = 0;
        public int LoopEnd = 0;
        public int LoopPoint = -1;
        public int Peak = 0;


        public AwcItemEntry(string path, AudioInformation audio)
        {
            FileLocation = path;
            Name = Path.GetFileNameWithoutExtension(path);
            FileName = Path.GetFileName(path);

            Codec = "ADPCM";
            Samples = audio.samples;
            SampleRate = audio.sampleRate;
        }
    }

    public struct AwcOptions
    {
        public bool streamFormat;
        public bool chunkIndices;

        public List<AwcItemEntry> entries;
    }

    public class AwcAudContainer
    {

        private Audio _audio;
        private readonly Dictionary<string, AwcItemEntry> m_audio = new();
        public AwcAudContainer()
        {
            _audio = new();
        }
        public async Task<AwcItemEntry?> AddAudio(string path)
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
            AwcItemEntry entry = new(path, (AudioInformation)info);
            m_audio.Add(Path.GetFileNameWithoutExtension(path), entry);
            return entry;
        }

        public void UpdateAudio(string name, AwcItemEntry entry)
        {
            if (!m_audio.ContainsKey(name)) 
                return;
            m_audio[name] = entry;
        }

        public AwcItemEntry? GetAudio(string name)
        {
            if (m_audio.TryGetValue(name, out AwcItemEntry entry)) return entry;
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
            foreach(AwcItemEntry entry in m_audio.Values.ToList())
                _audio.ConvertAudioToWav(entry.FileLocation, path);
        }
    }
}
