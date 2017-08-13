using System.Web;

namespace ISiteMapGenerator.Services.Engine
{
    public interface IBaseUrlProvider
    {
        string GetBaseUrl(HttpContextBase httpContext);
    }
}
