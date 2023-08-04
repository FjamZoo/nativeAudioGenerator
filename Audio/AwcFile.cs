using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
            XmlDocument doc = new XmlDocument();

            XmlNode AWC = doc.CreateElement("AudioWaveContainer");
            doc.CreateElement("Version")
                .Attributes?
                .Append(doc.CreateAttribute("value", "1"))
                .AppendChild(AWC);
            doc.CreateElement("ChunkIndices")
                .Attributes?
                .Append(doc.CreateAttribute("value", options.chunkIndices.ToString()))
                .AppendChild(AWC);
            doc.CreateElement("MultiChannel")
                .Attributes?
                .Append(doc.CreateAttribute("value", options.streamFormat.ToString()))
                .AppendChild(AWC);

            XmlNode streams = doc.CreateElement("Streams");
            //Add streamformat, data and seektable item entry
            if (options.streamFormat)
            {
                XmlNode item = doc.CreateElement("Item");
                XmlNode Chunks = doc.CreateElement("Chunks");
                {
                    XmlNode type = doc.CreateElement("Type");
                    XmlNode blockSize = doc.CreateElement("BlockSize");
                    blockSize.Attributes.Append(doc.CreateAttribute("value", "524288"));
                    type.InnerText = "streamformat";

                    Chunks.AppendChild(doc.CreateElement("Item")
                        .AppendChild(type)
                        .AppendChild(blockSize));
                }

                {
                    XmlNode type = doc.CreateElement("Type");
                    type.InnerText = "data";
                    Chunks.AppendChild(doc.CreateElement("Item").AppendChild(type));
                }

                {
                    XmlNode type = doc.CreateElement("Type");
                    type.InnerText = "seektable";
                    Chunks.AppendChild(doc.CreateElement("Item").AppendChild(type));
                }

                item.AppendChild(Chunks);
                streams.AppendChild(item);
            }

            Console.WriteLine(doc.ToString());
        }
    }
}
