using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISiteMapGenerator.Services.Engine;
using Moq;
using System.Web;
using ISiteMapGenerator.Services.Models;
using System.Web.Mvc;
using Ploeh.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace ISiteMapGenerator.Services.Tests
{
    [TestClass]
    public class SiteMapServiceTest
    {
        private MockRepository mockRepository;
        private IFixture fixture;

        private ISiteMapService siteMapService;

        private Mock<IActionResultFactory> actionResultFactory;
        private Mock<IBaseUrlProvider> baseUrlProvider;
        private Mock<IXmlNamespaceProvider> namespaceProvider;

        private Mock<HttpContextBase> httpContext;
        private Mock<ISiteMapConfigurationModel> siteMapConfigurationModel;

        private EmptyResult expectedResult;

        private string baseUrl;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            fixture = new Fixture();

            actionResultFactory = mockRepository.Create<IActionResultFactory>();
            baseUrlProvider = mockRepository.Create<IBaseUrlProvider>();
            namespaceProvider = mockRepository.Create<IXmlNamespaceProvider>();

            siteMapService = new SiteMapService(actionResultFactory.Object, baseUrlProvider.Object, namespaceProvider.Object);

            httpContext = mockRepository.Create<HttpContextBase>();
            siteMapConfigurationModel = mockRepository.Create<ISiteMapConfigurationModel>();

            expectedResult = new EmptyResult();

            baseUrl = "http://example.org";
        }

        private void GetBaseUrl()
        {
            baseUrlProvider.Setup(item => item.GetBaseUrl(httpContext.Object)).Returns(baseUrl);
        }

        private void GetNamespaces()
        {
            var xmlSerializerNamespaces = fixture.CreateMany<XmlSerializerNamespaceModel>(1);
            namespaceProvider.Setup(item => item.GetNamespaces(It.IsAny<IEnumerable<SiteMapNodeModel>>())).Returns(xmlSerializerNamespaces);
        }

        [TestMethod]
        public void CreateSiteMap_NodeListIsNull_DoesNotThrowException()
        {
            GetBaseUrl();
            GetNamespaces();

            actionResultFactory.Setup(item => item.CreateXmlResult(It.Is<SiteMapModel>(model => !model.Nodes.Any()), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMap(httpContext.Object, (IEnumerable<SiteMapNodeModel>)null);
        }

        [TestMethod]
        public void CreateSiteMap_SingleSitemapWithAbsoluteUrls()
        {
            GetBaseUrl();
            GetNamespaces();

            string url = "http://notexample.org/abc";

            List<SiteMapNodeModel> sitemapNodes = new List<SiteMapNodeModel> { new SiteMapNodeModel(url) };

            actionResultFactory.Setup(item => item.CreateXmlResult(It.Is<SiteMapModel>(model => model.Nodes.First().Url == url), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMap(httpContext.Object, sitemapNodes);
        }

        [TestMethod]
        public void CreateSiteMap_SingleSitemapWithRelativeUrls()
        {
            GetBaseUrl();
            GetNamespaces();

            string url = "/relative";

            List<SiteMapNodeModel> sitemapNodes = new List<SiteMapNodeModel> { new SiteMapNodeModel(url) };

            Expression<System.Func<SiteMapModel, bool>> validateNode = model => model.Nodes.First().Url == "http://example.org/relative";

            actionResultFactory.Setup(item => item.CreateXmlResult(It.Is(validateNode), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMap(httpContext.Object, sitemapNodes);
        }

        [TestMethod]
        public void CreateSitemapWithConfiguration_PageSizeIsBiggerThanNodeCount_CreatesSitemap()
        {
            GetBaseUrl();
            GetNamespaces();

            List<SiteMapNodeModel> sitemapNodes = new List<SiteMapNodeModel> { new SiteMapNodeModel("/relative") };

            siteMapConfigurationModel.Setup(item => item.Size).Returns(5);

            actionResultFactory.Setup(item => item.CreateXmlResult(It.IsAny<SiteMapModel>(), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMaps(httpContext.Object, sitemapNodes, siteMapConfigurationModel.Object);
        }

        [TestMethod]
        public void CreateSitemapWithConfiguration_NodeCountIsGreaterThanPageSize_And_CurrentPageSize_Equal_Zero_CreatesIndex()
        {
            int? currentPage = 0;

            GetBaseUrl();
            GetNamespaces();

            List<SiteMapNodeModel> sitemapNodes = fixture.CreateMany<SiteMapNodeModel>(5).ToList();
            siteMapConfigurationModel.Setup(item => item.Size).Returns(2);
            siteMapConfigurationModel.Setup(item => item.CurrentPage).Returns(currentPage);
            siteMapConfigurationModel.Setup(item => item.CreateSitemapUrl(It.Is<int>(i => i <= 3))).Returns(string.Empty);

            Expression<Func<SiteMapIndexModel, bool>> validateIndex = index => index.Nodes.Count == 3;

            actionResultFactory.Setup(item => item.CreateXmlResult(It.Is(validateIndex), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMaps(httpContext.Object, sitemapNodes, siteMapConfigurationModel.Object);
        }

        [TestMethod]
        public void CreateSitemapWithConfiguration_NodeCountIsGreaterThanPageSize_And_CurrentPageSize_Is_Null_CreatesIndex()
        {
            int? currentPage = null;

            GetBaseUrl();
            GetNamespaces();

            List<SiteMapNodeModel> sitemapNodes = fixture.CreateMany<SiteMapNodeModel>(5).ToList();
            siteMapConfigurationModel.Setup(item => item.Size).Returns(2);
            siteMapConfigurationModel.Setup(item => item.CurrentPage).Returns(currentPage);
            siteMapConfigurationModel.Setup(item => item.CreateSitemapUrl(It.Is<int>(i => i <= 3))).Returns(string.Empty);

            Expression<Func<SiteMapIndexModel, bool>> validateIndex = index => index.Nodes.Count == 3;

            actionResultFactory.Setup(item => item.CreateXmlResult(It.Is(validateIndex), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMaps(httpContext.Object, sitemapNodes, siteMapConfigurationModel.Object);
        }

        [TestMethod]
        public void CreateSitemapWithConfiguration_AsksForSpecificPage_CreatesSitemap()
        {
            GetBaseUrl();
            GetNamespaces();

            List<SiteMapNodeModel> sitemapNodes = fixture.CreateMany<SiteMapNodeModel>(5).ToList();
            siteMapConfigurationModel.Setup(item => item.Size).Returns(2);
            siteMapConfigurationModel.Setup(item => item.CurrentPage).Returns(3);

            Expression<Func<SiteMapModel, bool>> validateSitemap = model => model.Nodes.Count == 1;
            actionResultFactory.Setup(item => item.CreateXmlResult(It.Is(validateSitemap), It.IsAny<IEnumerable<XmlSerializerNamespaceModel>>())).Returns(expectedResult);

            ActionResult result = siteMapService.CreateSiteMaps(httpContext.Object, sitemapNodes, siteMapConfigurationModel.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSitemap_HttpContextIsNull_ThrowsException()
        {
            List<SiteMapNodeModel> sitemapNodes = new List<SiteMapNodeModel>();

            siteMapService.CreateSiteMap(null, sitemapNodes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSitemapWithConfiguration_HttpContextIsNull_ThrowsException()
        {
            List<SiteMapNodeModel> sitemapNodes = new List<SiteMapNodeModel>();

            siteMapService.CreateSiteMaps(null, sitemapNodes, siteMapConfigurationModel.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSitemapWithConfiguration_ConfigurationIsNull_ThrowsException()
        {
            List<SiteMapNodeModel> sitemapNodes = new List<SiteMapNodeModel>();

            siteMapService.CreateSiteMaps(httpContext.Object, sitemapNodes, null);
        }
    }
}
