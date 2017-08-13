using ISiteMapGenerator.Services.Infrastructure;
using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISiteMapGenerator.Services.Engine
{
    public class XmlNamespaceProvider : IXmlNamespaceProvider
    {
        public IEnumerable<XmlSerializerNamespaceModel> GetNamespaces(IEnumerable<SiteMapNodeModel> nodes)
        {
            IEnumerable<XmlSerializerNamespaceModel> namespaces = null;

            if (nodes.Any(node => node.SiteMapImage != null))
            {
                namespaces = new List<XmlSerializerNamespaceModel>
                {
                    new XmlSerializerNamespaceModel
                    {
                        Prefix = SiteMapNamespaces.IMAGE_PREFIX,
                        Namespace = SiteMapNamespaces.IMAGE
                    }
                };
            }

            return namespaces;
        }
    }
}
