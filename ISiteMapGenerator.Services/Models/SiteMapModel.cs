using ISiteMapGenerator.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ISiteMapGenerator.Services.Models
{
    [XmlRoot("urlset", Namespace = SiteMapNamespaces.SITEMAP)]
    public class SiteMapModel
    {
        private readonly IEnumerable<SiteMapNodeModel> _nodeList;
        public SiteMapModel() { }
        public SiteMapModel(IEnumerable<SiteMapNodeModel> sitemapNodes)
        {
            _nodeList = sitemapNodes;
        }

        [XmlElement("url")]
        public List<SiteMapNodeModel> Nodes
        {
            get { return _nodeList.ToList(); }
        }
    }
}
