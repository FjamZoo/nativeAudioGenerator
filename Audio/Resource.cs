using CodeWalker.GameFiles;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

namespace NativeAudioGen.Audio
{
    class Resource
    {

        private App _app;
        private static string[] _manifest = {
            "fx_version 'cerulean'",
            "game 'gta5'",
            ""
        };

        public string Name { get; set; }
        public string path { get; set; }

        public string dataPath { get; set; }
        public string packPath { get; set; }

        public string soundPath { get; set; }

        public Resource(string name)
        {
            _app = (App)Application.Current;
            Name = name;
            path = Path.Combine(_app.Output, name);
            dataPath = Path.Combine(path, "data");
            packPath = Path.Combine(path, _app.SoundpackName);
            soundPath = Path.Combine(packPath, name + "_sounds");
            InitResource();
        }

        void InitResource()
        {
            if(!Directory.Exists(path)) Directory.CreateDirectory(path);
            if(!Directory.Exists(packPath)) Directory.CreateDirectory(packPath);
            if(!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
            if (!Directory.Exists(soundPath)) Directory.CreateDirectory(soundPath);
            using(StreamWriter sw = new(Path.Combine(path, "fxmanifest.lua")))
            {
                foreach (string entry in _manifest) 
                    sw.WriteLine(entry);
            }
        }

        public void GenerateNametable()
        {
            string[] files = Directory.GetFiles(soundPath, "*.wav");
            if(files.Length == 0) return;
            Stream content = File.Open(Path.Combine(packPath, "awc.nametable"), FileMode.Create);
            BinaryWriter writer = new(content);
            foreach (string file in files)
            {
                byte[] buf = Encoding.ASCII.GetBytes(Path.GetFileNameWithoutExtension(file) + char.MinValue);
                writer.Write(buf);
            }
            writer.Close();
            content.Close();
        }

        public void BuildAWC(AwcOptions options)
        {
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
                    codec.InnerText = options.streamFormat ? "ADPCM" : entry.Codec; // Streamformat requires ADPCM to function
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
            using (XmlWriter writer = XmlWriter.Create(Path.Combine(packPath, Name + "_sounds.awc.xml"), settings)){
                doc.Save(writer);
            }
            GenerateNametable();
            try
            {
                AwcFile awc = new();
                awc.ReadXml(AWC, soundPath);
                byte[] file = awc.Save();
                File.WriteAllBytes(Path.Combine(packPath, Name + "_sounds.awc"), file);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Unable to build AWC automatically", "Codewalker error: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return;
        }

        public void OpenFolder() => Process.Start("explorer.exe", path);
    }
}
