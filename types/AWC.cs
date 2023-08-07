using System.Xml;

namespace NativeAudioGen.Types
{
    public enum AWCCodec
    {
        PCM = 0,
        ADPCM = 4
    }

    public struct AWCMarker
    {

    }

    public class AWCItem
    {
        public string Name;
        public string File;

        public AWCCodec Codec;
        public int Samples;
        public int SampleRate;
        public int Headroom = -200;

        public int PlayBegin = 0;
        public int PlayEnd = 0;

        public int LoopBegin = 0;
        public int LoopEnd = 0;
        public int LoopPoint = -1;
        public int Peak = 0;

        public XmlNode GenerateXML(XmlDocument doc, bool streamFormat)
        {
            XmlNode item = doc.CreateElement("Item");
            {
                XmlElement name = doc.CreateElement("Name");
                name.InnerText = Name;
                item.AppendChild(name);
            }

            {
                XmlElement filename = doc.CreateElement("FileName");
                filename.InnerText = File;
                item.AppendChild(filename);
            }


            XmlNode chunks = doc.CreateElement("Chunks");
            if (!streamFormat)
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


            XmlNode entry = doc.CreateElement(streamFormat ? "StreamFormat" : "Item");
            if (!streamFormat)
            {
                XmlElement format = doc.CreateElement("Type");
                format.InnerText = "format";
                entry.AppendChild(format);
            }

            {
                XmlElement codec = doc.CreateElement("Codec");
                codec.InnerText = streamFormat ? "ADPCM" : Codec.ToString(); // Streamformat requires ADPCM to function
                entry.AppendChild(codec);
            }

            {
                XmlElement samples = doc.CreateElement("Samples");
                samples.SetAttribute("value", Samples.ToString());
                entry.AppendChild(samples);
            }

            {
                XmlElement sampleRate = doc.CreateElement("SampleRate");
                sampleRate.SetAttribute("value", SampleRate.ToString());
                entry.AppendChild(sampleRate);
            }

            {
                XmlElement headRoom = doc.CreateElement("Headroom");
                headRoom.SetAttribute("value", Headroom.ToString());
                entry.AppendChild(headRoom);
            }


            if (!streamFormat)
            {
                {
                    XmlElement playBegin = doc.CreateElement("PlayBegin");
                    playBegin.SetAttribute("value", PlayBegin.ToString());
                    entry.AppendChild(playBegin);
                }
                {
                    XmlElement playEnd = doc.CreateElement("PlayEnd");
                    playEnd.SetAttribute("value", PlayEnd.ToString());
                    entry.AppendChild(playEnd);
                }
                {
                    XmlElement loopBegin = doc.CreateElement("LoopBegin");
                    loopBegin.SetAttribute("value", LoopBegin.ToString());
                    entry.AppendChild(loopBegin);
                }
                {
                    XmlElement loopEnd = doc.CreateElement("LoopEnd");
                    loopEnd.SetAttribute("value", LoopEnd.ToString());
                    entry.AppendChild(loopEnd);
                }
                {
                    XmlElement loopPoint = doc.CreateElement("LoopPoint");
                    loopPoint.SetAttribute("value", LoopPoint.ToString());
                    entry.AppendChild(loopPoint);
                }
                {
                    XmlElement peakUnk = doc.CreateElement("Peak");
                    peakUnk.SetAttribute("unk", Peak.ToString());
                    entry.AppendChild(peakUnk);
                }
                chunks.AppendChild(entry);
                item.AppendChild(chunks);
            }       
            else
                item.AppendChild(entry);
            return item;
        }
    }
}