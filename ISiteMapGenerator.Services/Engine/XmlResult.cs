using ISiteMapGenerator.Services.Models;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ISiteMapGenerator.Services.Engine
{
    public class XmlResult<T> : ActionResult
    {
        private readonly T _data;
        private readonly IEnumerable<XmlSerializerNamespaceModel> _namespaces;

        public XmlResult(T data, IEnumerable<XmlSerializerNamespaceModel> namespaces = null)
        {
            _data = data;
            _namespaces = namespaces;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            string xml = new XmlSerializer().Serialize(_data, _namespaces);
            context.HttpContext.Response.Write(xml);
        }
    }
}
