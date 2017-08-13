using ISiteMapGenerator.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ISiteMapGenerator.Services.Models
{
    [XmlRoot("url", Namespace = SiteMapNamespaces.SITEMAP)]
    public class SiteMapNodeModel : IHasUrl
    {
        public SiteMapNodeModel() { }

        public SiteMapNodeModel(string url)
        {
            Url = url;
        }

        [XmlElement("loc", Order = 1)]
        public string Url { get; set; }

        [XmlElement("image", Order = 2, Namespace = SiteMapNamespaces.IMAGE)]
        public List<SiteMapImageModel> SiteMapImage { get; set; }

        [XmlElement("lastmod", Order = 3)]
        public DateTime? LastModificationDate { get; set; }

        [XmlElement("changefreq", Order = 4)]
        public ChangeFrequency? ChangeFrequency { get; set; }

        [XmlElement("priority", Order = 5)]
        public decimal? Priority { get; set; }

        public bool ShouldSerializeUrl()
        {
            return Url != null;
        }
        public bool ShouldSerializeImageDefinition()
        {
            return SiteMapImage != null;
        }
        public bool ShouldSerializeLastModificationDate()
        {
            return LastModificationDate != null;
        }
        public bool ShouldSerializeChangeFrequency()
        {
            return ChangeFrequency != null;
        }
        public bool ShouldSerializePriority()
        {
            return Priority != null;
        }
    }
}
