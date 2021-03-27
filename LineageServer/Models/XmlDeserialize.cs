using LineageServer.Interfaces;
using System.IO;
using System.Xml.Serialization;

namespace LineageServer.Models
{
    class XmlDeserialize : IXmlDeserialize
    {
        public T Deserialize<T>(string xml)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml));
        }
    }
}
