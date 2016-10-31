using System.Web.Routing;
using GameStore.WEB.Tests.Routes.Stubs;
using NUnit.Framework;

namespace GameStore.WEB.Tests.Routes
{
    [TestFixture]
    public class RoutesTests
    {
        private readonly RouteCollection _routes;

        public RoutesTests()
        {
            _routes = RouteTable.Routes;
            RouteConfig.RegisterRoutes(_routes);
        }

        [Test]
        [TestCase("~/", "get", "Games", "Get")]
        [TestCase("~/games", "get", "Games", "Get")]
        [TestCase("~/game/AAA", "get", "Games", "GetDetails")]
        [TestCase("~/game/AAA/comments", "get", "Comments", "GetGameComments")]
        [TestCase("~/game/AAA/download", "get", "Games", "Download")]
        [TestCase("~/game/AAA/newcomment", "post", "Comments", "CreateComment")]
        [TestCase("~/games/new", "post", "Games", "New")]
        [TestCase("~/games/update", "post", "Games", "Update")]
        [TestCase("~/games/remove", "post", "Games", "Remove")]
        public void DefoultRoute(string url, string httpMethod, string expectedController, string expectedAction)
        {
            // Arrange
            var context = new StubHttpContextForRouting(requestUrl: url, httpMethod: httpMethod);

            // Act
            RouteData routeData = _routes.GetRouteData(context);

            // Assert
            Assert.IsNotNull(routeData);
            Assert.AreEqual(expectedController.ToUpper(), ((string)routeData.Values["controller"]).ToUpper());
            Assert.AreEqual(expectedAction.ToUpper(), ((string)routeData.Values["action"]).ToUpper());
        }
    }
}
