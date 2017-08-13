using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ISiteMapGenerator.Services.Engine
{
    public class ActionResultFactory : IActionResultFactory
    {
        public ActionResult CreateXmlResult<T>(T data, IEnumerable<XmlSerializerNamespaceModel> serializerNamespaces = null)
        {
            return new XmlResult<T>(data, serializerNamespaces);
        }
    }
}
