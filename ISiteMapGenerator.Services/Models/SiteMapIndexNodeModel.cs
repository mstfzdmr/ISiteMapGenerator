using ISiteMapGenerator.Services.Infrastructure;
using System;
using System.Xml.Serialization;

namespace ISiteMapGenerator.Services.Models
{
    [XmlRoot("sitemap", Namespace = SiteMapNamespaces.SITEMAP)]
    public class SiteMapIndexNodeModel : IHasUrl
    {
        [XmlElement("loc", Order = 1)]
        public string Url { get; set; }

        [XmlElement("lastmod", Order = 2)]
        public DateTime? LastModificationDate { get; set; }
        public bool ShouldSerializeLastModificationDate()
        {
            return LastModificationDate != null;
        }
        public bool ShouldSerializeUrl()
        {
            return Url != null;
        }
    }
}
