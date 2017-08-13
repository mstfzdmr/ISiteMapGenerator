using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ISiteMapGenerator.Services.Models;

namespace ISiteMapGenerator.Services
{
    public interface ISiteMapService
    {
        ActionResult CreateSiteMap(HttpContextBase httpContext, IEnumerable<SiteMapNodeModel> siteMapNodes);

        ActionResult CreateSiteMaps(HttpContextBase httpContext, IEnumerable<SiteMapNodeModel> siteMapNodes, ISiteMapConfigurationModel siteMapConfigurationModel);

        ActionResult CreateSiteMap(HttpContextBase httpContext, IEnumerable<SiteMapIndexNodeModel> nodes);
    }
}