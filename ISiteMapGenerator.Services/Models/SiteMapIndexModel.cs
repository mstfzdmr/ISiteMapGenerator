using ISiteMapGenerator.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ISiteMapGenerator.Services.Models
{
    [XmlRoot("sitemapindex", Namespace = SiteMapNamespaces.SITEMAP)]
    public class SiteMapIndexModel
    {
        private IEnumerable<SiteMapIndexNodeModel> _nodeList;

        public SiteMapIndexModel() { }

        public SiteMapIndexModel(IEnumerable<SiteMapIndexNodeModel> sitemapIndexNodes)
        {
            _nodeList = sitemapIndexNodes;
        }

        [XmlElement("sitemap")]
        public List<SiteMapIndexNodeModel> Nodes
        {
            get { return _nodeList.ToList(); }
        }
    }
}
