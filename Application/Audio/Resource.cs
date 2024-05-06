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
            _app = (App)System.Windows.Application.Current;
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
            foreach (AwcEntry entry in options.entries)
                streams.AppendChild(entry.data.GenerateXML(doc, options.streamFormat));
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
