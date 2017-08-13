using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;

namespace ISiteMapGenerator.Services.Engine
{
    public interface IXmlNamespaceProvider
    {
        IEnumerable<XmlSerializerNamespaceModel> GetNamespaces(IEnumerable<SiteMapNodeModel> nodes);
    }
}
