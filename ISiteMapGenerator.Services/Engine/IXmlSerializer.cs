using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;

namespace ISiteMapGenerator.Services.Engine
{
    interface IXmlSerializer
    {
        string Serialize<T>(T data, IEnumerable<XmlSerializerNamespaceModel> xmlSerializerNamespaceModels);
    }
}
