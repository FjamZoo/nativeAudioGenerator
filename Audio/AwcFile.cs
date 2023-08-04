using FFMpegCore.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace NativeAudioGen.Audio
{

    public struct AwcOptions
    {
        public bool streamFormat;
        public bool chunkIndices;

        public List<AwcItemEntry> entries;
    }

    public class AwcFile
    {

        public AwcFile() { }


        public void GenerateFile(string fileName, AwcOptions options)
        {
            XmlDocument doc = new();
            XmlNode AWC = doc.CreateElement("AudioWaveContainer");
            XmlElement version = doc.CreateElement("Version");
            version.SetAttribute("value", "1");
            AWC.AppendChild(version);

            if (options.chunkIndices)
            {
                XmlElement chunk = doc.CreateElement("ChunkIndices");
                chunk.SetAttribute("value", options.chunkIndices ? "True" : "False");
                AWC.AppendChild(chunk);
            }

            if (options.streamFormat) {
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

            foreach (AwcItemEntry entry in options.entries) {
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


                XmlNode streamFormat = doc.CreateElement("StreamFormat");

                if (options.streamFormat)
                {
                    {
                        XmlElement codec = doc.CreateElement("Codec");
                        codec.InnerText = "ADPCM"; // Streamformat requires ADPCM to function
                        streamFormat.AppendChild(codec);
                    }

                    {
                        XmlElement samples = doc.CreateElement("Samples");
                        samples.SetAttribute("value", entry.Samples.ToString());
                        streamFormat.AppendChild(samples);
                    }

                    {
                        XmlElement sampleRate = doc.CreateElement("SampleRate");
                        sampleRate.SetAttribute("value", entry.SampleRate.ToString());
                        streamFormat.AppendChild(sampleRate);
                    }

                    {
                        XmlElement headRoom = doc.CreateElement("Headroom");
                        headRoom.SetAttribute("value", entry.Headroom.ToString());
                        streamFormat.AppendChild(headRoom);
                    }
                }

                itemNode.AppendChild(streamFormat);
                streams.AppendChild(itemNode);
            }
            AWC.AppendChild(streams);
            doc.AppendChild(AWC);
            return;
        }
    }
}
