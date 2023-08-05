using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Text;
using System.Diagnostics;

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

            Codec = audio.codec;
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

        private AwcFile _awcFile;

        private readonly Dictionary<string, AwcItemEntry> m_audio = new();
        public AwcAudContainer()
        {
            _audio = new();
            _awcFile = new();
        }
        public async Task<AwcItemEntry?> AddAudio(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show(string.Format("Unable to find file {0} at path {0}", Path.GetFileName(path), path, "Unable to find file", MessageBoxButton.OK, MessageBoxImage.Error));
                return null;
            }
            if (m_audio.ContainsKey(Path.GetFileNameWithoutExtension(path)))
            {
                MessageBox.Show(string.Format("Audio entry with name {0} already exists!", Path.GetFileNameWithoutExtension(path)), "Audio already exists", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            App app = (App)Application.Current;
            string path = Path.Combine(app.Output, app.ResourceName, app.SoundpackName);
            GenerateAWCFile(Path.Combine(path, string.Format("{0}_sounds.awc.xml", app.ResourceName)), new AwcOptions()
            {
                streamFormat = false,
                chunkIndices = true,
                entries = m_audio.Values.ToList()
            });
            GenerateAWCSounds(Path.Combine(path, string.Format("{0}_sounds", app.ResourceName)));
        }


        private void GenerateAWCFile(string path, AwcOptions options) {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            XmlDocument doc = new();
            XmlNode AWC = doc.CreateElement("AudioWaveContainer");
            XmlElement version = doc.CreateElement("Version");
            version.SetAttribute("value", "1");
            AWC.AppendChild(version);

            if (options.chunkIndices)
            {
                XmlElement chunk = doc.CreateElement("ChunkIndices");
                chunk.SetAttribute("value", "True");
                AWC.AppendChild(chunk);
            }

            if (options.streamFormat)
            {
                XmlElement multiChannel = doc.CreateElement("MultiChannel");
                multiChannel.SetAttribute("value", "True");
                AWC.AppendChild(multiChannel);
            }

            XmlNode streams = doc.CreateElement("Streams");
            //Add streamformat, data and seektable item entry
            if (options.streamFormat)
            {
                XmlNode item = doc.CreateElement("Item");
                XmlNode Chunks = doc.CreateElement("Chunks");
                {
                    XmlNode itemNode = doc.CreateElement("Item");

                    XmlElement type = doc.CreateElement("Type");
                    XmlElement blockSize = doc.CreateElement("BlockSize");
                    blockSize.SetAttribute("value", "524288");
                    type.InnerText = "streamformat";

                    itemNode.AppendChild(type);
                    itemNode.AppendChild(blockSize);
                    Chunks.AppendChild(itemNode);
                }

                {
                    XmlNode itemNode = doc.CreateElement("Item");
                    XmlNode type = doc.CreateElement("Type");
                    type.InnerText = "data";
                    itemNode.AppendChild(type);
                    Chunks.AppendChild(itemNode);
                }

                {
                    XmlNode itemNode = doc.CreateElement("Item");
                    XmlNode type = doc.CreateElement("Type");
                    type.InnerText = "seektable";
                    itemNode.AppendChild(type);
                    Chunks.AppendChild(itemNode);
                }
                item.AppendChild(Chunks);
                streams.AppendChild(item);
            }

            foreach (AwcItemEntry entry in options.entries)
            {
                XmlNode itemNode = doc.CreateElement("Item");
                {
                    XmlElement name = doc.CreateElement("Name");
                    name.InnerText = entry.Name;
                    itemNode.AppendChild(name);
                }

                {
                    XmlElement filename = doc.CreateElement("FileName");
                    filename.InnerText = Path.GetFileNameWithoutExtension(entry.FileName) + ".wav";
                    itemNode.AppendChild(filename);
                }

                XmlNode chunks = doc.CreateElement("Chunks");
                if (!options.streamFormat)
                {
                    {
                        XmlNode typeItem = doc.CreateElement("Item");
                        XmlNode type = doc.CreateElement("Type");
                        type.InnerText = "peak";
                        typeItem.AppendChild(type);
                        chunks.AppendChild(typeItem);
                    }

                    {
                        XmlNode typeItem = doc.CreateElement("Item");
                        XmlNode type = doc.CreateElement("Type");
                        type.InnerText = "data";
                        typeItem.AppendChild(type);
                        chunks.AppendChild(typeItem);
                    }
                }

                XmlNode itemEntry = doc.CreateElement(options.streamFormat ? "StreamFormat" : "Item");
                if (!options.streamFormat)
                {
                    XmlElement format = doc.CreateElement("Type");
                    format.InnerText = "format";
                    itemEntry.AppendChild(format);
                }

                {
                    XmlElement codec = doc.CreateElement("Codec");
                    codec.InnerText = "ADPCM"; // Streamformat requires ADPCM to function
                    itemEntry.AppendChild(codec);
                }

                {
                    XmlElement samples = doc.CreateElement("Samples");
                    samples.SetAttribute("value", entry.Samples.ToString());
                    itemEntry.AppendChild(samples);
                }

                {
                    XmlElement sampleRate = doc.CreateElement("SampleRate");
                    sampleRate.SetAttribute("value", entry.SampleRate.ToString());
                    itemEntry.AppendChild(sampleRate);
                }

                {
                    XmlElement headRoom = doc.CreateElement("Headroom");
                    headRoom.SetAttribute("value", entry.Headroom.ToString());
                    itemEntry.AppendChild(headRoom);
                }

                //TODO: Verify if streamFormat can compile or even use these options
                if (!options.streamFormat)
                {
                    {
                        XmlElement playBegin = doc.CreateElement("PlayBegin");
                        playBegin.SetAttribute("value", entry.PlayBegin.ToString());
                        itemEntry.AppendChild(playBegin);
                    }
                    {
                        XmlElement playEnd = doc.CreateElement("PlayEnd");
                        playEnd.SetAttribute("value", entry.PlayEnd.ToString());
                        itemEntry.AppendChild(playEnd);
                    }
                    {
                        XmlElement loopBegin = doc.CreateElement("LoopBegin");
                        loopBegin.SetAttribute("value", entry.LoopBegin.ToString());
                        itemEntry.AppendChild(loopBegin);
                    }
                    {
                        XmlElement loopEnd = doc.CreateElement("LoopEnd");
                        loopEnd.SetAttribute("value", entry.LoopEnd.ToString());
                        itemEntry.AppendChild(loopEnd);
                    }
                    {
                        XmlElement loopPoint = doc.CreateElement("LoopPoint");
                        loopPoint.SetAttribute("value", entry.LoopPoint.ToString());
                        itemEntry.AppendChild(loopPoint);
                    }
                    {
                        XmlElement peakUnk = doc.CreateElement("Peak");
                        peakUnk.SetAttribute("unk", entry.Peak.ToString());
                        itemEntry.AppendChild(peakUnk);
                    }
                    chunks.AppendChild(itemEntry);
                    itemNode.AppendChild(chunks);
                }
                else
                    itemNode.AppendChild(itemEntry);
                streams.AppendChild(itemNode);
            }
            AWC.AppendChild(streams);
            doc.AppendChild(AWC);

            XmlWriterSettings settings = new()
            {
                Async = true,
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            };
            using XmlWriter writer = XmlWriter.Create(path, settings);
            doc.Save(writer);
        }

        private void GenerateAWCSounds(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach(AwcItemEntry entry in m_audio.Values.ToList())
                _audio.ConvertAudioToWav(entry.FileLocation, path);
            Process.Start("explorer.exe", Path.Combine(path, ".."));
        }
    }
}
