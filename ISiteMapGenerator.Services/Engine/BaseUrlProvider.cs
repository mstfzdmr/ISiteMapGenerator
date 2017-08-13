using System.Web;
using System.Web.Mvc;

namespace ISiteMapGenerator.Services.Engine
{
    public class BaseUrlProvider : IBaseUrlProvider
    {
        public string GetBaseUrl(HttpContextBase httpContext)
        {
            HttpRequestBase request = httpContext.Request;
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, UrlHelper.GenerateContentUrl("~", httpContext)).TrimEnd('/');
        }
    }
}
