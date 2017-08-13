using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ISiteMapGenerator.Services.Engine
{
    public interface IActionResultFactory
    {
        ActionResult CreateXmlResult<T>(T data, IEnumerable<XmlSerializerNamespaceModel> xmlSerializerNamespaceModel = null);
    }
}
