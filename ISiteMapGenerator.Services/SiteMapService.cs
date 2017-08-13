using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ISiteMapGenerator.Services.Models;
using ISiteMapGenerator.Services.Engine;
using System.Linq;

namespace ISiteMapGenerator.Services
{
    public class SiteMapService : ISiteMapService
    {
        #region Fields

        private readonly IActionResultFactory actionResultFactory;
        private readonly IBaseUrlProvider baseUrlProvider;
        private readonly IXmlNamespaceProvider xmlNamespaceProvider;
        private ActionResultFactory object1;
        private BaseUrlProvider object2;
        private XmlNamespaceProvider object3;

        #endregion

        #region Ctor

        public SiteMapService(IActionResultFactory actionResultFactory, IBaseUrlProvider baseUrlProvider, IXmlNamespaceProvider xmlNamespaceProvider)
        {
            this.actionResultFactory = actionResultFactory;
            this.baseUrlProvider = baseUrlProvider;
            this.xmlNamespaceProvider = xmlNamespaceProvider;
        }

        public SiteMapService() : this(new ActionResultFactory(), new BaseUrlProvider(), new XmlNamespaceProvider()) { }

        #endregion

        #region Methods

        public ActionResult CreateSiteMap(HttpContextBase httpContext, IEnumerable<SiteMapNodeModel> siteMapNodes)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            string baseUrl = baseUrlProvider.GetBaseUrl(httpContext);
            List<SiteMapNodeModel> nodeList = siteMapNodes != null ? siteMapNodes.ToList() : new List<SiteMapNodeModel>();
            IEnumerable<XmlSerializerNamespaceModel> namespaces = xmlNamespaceProvider.GetNamespaces(nodeList);
            return CreateSitemapInternal(baseUrl, nodeList, namespaces);
        }

        public ActionResult CreateSiteMap(HttpContextBase httpContext, IEnumerable<SiteMapIndexNodeModel> nodes)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            string baseUrl = baseUrlProvider.GetBaseUrl(httpContext);

            List<SiteMapIndexNodeModel> nodeList = nodes != null ? nodes.ToList() : new List<SiteMapIndexNodeModel>();
            nodeList.ForEach(node => ValidateUrl(baseUrl, node));

            var sitemap = new SiteMapIndexModel(nodeList);
            return actionResultFactory.CreateXmlResult(sitemap);
        }

        public ActionResult CreateSiteMaps(HttpContextBase httpContext, IEnumerable<SiteMapNodeModel> siteMapNodes, ISiteMapConfigurationModel siteMapConfigurationModel)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (siteMapConfigurationModel == null)
            {
                throw new ArgumentNullException("configuration");
            }

            string baseUrl = baseUrlProvider.GetBaseUrl(httpContext);
            List<SiteMapNodeModel> nodeList = siteMapNodes != null ? siteMapNodes.ToList() : new List<SiteMapNodeModel>();
            IEnumerable<XmlSerializerNamespaceModel> namespaces = xmlNamespaceProvider.GetNamespaces(nodeList);

            if (siteMapConfigurationModel.Size >= nodeList.Count)
            {
                return CreateSitemapInternal(baseUrl, nodeList, namespaces);
            }
            if (siteMapConfigurationModel.CurrentPage.HasValue && siteMapConfigurationModel.CurrentPage.Value > 0)
            {
                int skipCount = (siteMapConfigurationModel.CurrentPage.Value - 1) * siteMapConfigurationModel.Size;
                List<SiteMapNodeModel> pageNodes = nodeList.Skip(skipCount).Take(siteMapConfigurationModel.Size).ToList();
                return CreateSitemapInternal(baseUrl, pageNodes, namespaces);
            }

            int pageCount = (int)Math.Ceiling((double)nodeList.Count / siteMapConfigurationModel.Size);
            var indexNodes = CreateIndexNode(siteMapConfigurationModel, baseUrl, pageCount);
            return actionResultFactory.CreateXmlResult(new SiteMapIndexModel(indexNodes));
        }

        #endregion

        #region Private Methods

        private ActionResult CreateSitemapInternal(string baseUrl, List<SiteMapNodeModel> nodes, IEnumerable<XmlSerializerNamespaceModel> namespaces = null)
        {
            nodes.ForEach(node => ValidateUrl(baseUrl, node));

            SiteMapModel sitemap = new SiteMapModel(nodes);

            return actionResultFactory.CreateXmlResult(sitemap, namespaces);
        }

        private IEnumerable<SiteMapIndexNodeModel> CreateIndexNode(ISiteMapConfigurationModel configuration, string baseUrl, int pageCount)
        {
            for (int page = 1; page <= pageCount; page++)
            {
                string url = configuration.CreateSitemapUrl(page);
                SiteMapIndexNodeModel indexNode = new SiteMapIndexNodeModel { Url = url };
                ValidateUrl(baseUrl, indexNode);
                yield return indexNode;
            }
        }

        private void ValidateUrl(string baseUrl, IHasUrl node)
        {
            if (!Uri.IsWellFormedUriString(node.Url, UriKind.Absolute))
            {
                node.Url = string.Concat(baseUrl, node.Url);
            }
        }

        #endregion
    }
}
