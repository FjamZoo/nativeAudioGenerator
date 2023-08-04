using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
        public int LoopPoint = 0;
        public int Peak = 0;


        public AwcItemEntry(string path, AudioInformation audio)
        {
            FileLocation = path;
            Name = Path.GetFileNameWithoutExtension(path);
            FileName = Path.GetFileName(path);

            Codec = audio.codec;
            Samples = audio.samples;
            SampleRate = audio.sampleRate;
        }
    }


    public class AwcAudContainer
    {

        private Audio _audio;

        private AwcFile _awcFile;

        private readonly Dictionary<string, AwcItemEntry> m_audio = new();
        public AwcAudContainer()
        {
            _audio = new();
            _awcFile = new();
        }

        public async Task<AwcItemEntry?> AddAudio(string path)
        {
            if (m_audio.ContainsKey(Path.GetFileNameWithoutExtension(path)) || !File.Exists(path))
            {
                MessageBox.Show("containsKey: " + m_audio.ContainsKey(Path.GetFileNameWithoutExtension(path)) + " fileExists: " + !File.Exists(path), "test", MessageBoxButton.OK, MessageBoxImage.Error);
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
            {
                _names.Add(key);
            }
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

            _awcFile.GenerateFile("", new AwcOptions()
            {
                streamFormat = true,
                chunkIndices = true,
                entries = m_audio.Values.ToList()
            });
        }
    }
}
