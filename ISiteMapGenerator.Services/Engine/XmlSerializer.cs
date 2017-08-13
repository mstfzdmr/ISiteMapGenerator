using ISiteMapGenerator.Services.Infrastructure;
using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ISiteMapGenerator.Services.Engine
{
    public class XmlSerializer : IXmlSerializer
    {
        public string Serialize<T>(T data, IEnumerable<XmlSerializerNamespaceModel> xmlSerializerNamespaceModels)
        {
            var serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add("", SiteMapNamespaces.SITEMAP);

            List<XmlSerializerNamespaceModel> XmlSerializerNamespaces = xmlSerializerNamespaceModels != null
                ? xmlSerializerNamespaceModels.ToList()
                : Enumerable.Empty<XmlSerializerNamespaceModel>().ToList();

            if (XmlSerializerNamespaces.Any())
                XmlSerializerNamespaces.ForEach(item => serializerNamespaces.Add(item.Prefix, item.Namespace));

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(memoryStream, new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    NamespaceHandling = NamespaceHandling.OmitDuplicates
                }))
                {
                    xmlSerializer.Serialize(writer, data, serializerNamespaces);
                    writer.Flush();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return new StreamReader(memoryStream).ReadToEnd();
                }
            }
        }
    }
}
