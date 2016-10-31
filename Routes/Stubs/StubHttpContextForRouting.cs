using System.Web;

namespace GameStore.WEB.Tests.Routes.Stubs
{
    public class StubHttpContextForRouting : HttpContextBase
    {
        readonly StubHttpRequestForRouting _request;
        readonly StubHttpResponseForRouting _response;

        public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/", string httpMethod = "get")
        {
            _request = new StubHttpRequestForRouting(appPath, requestUrl, httpMethod);
            _response = new StubHttpResponseForRouting();
        }

        public override HttpRequestBase Request => _request;

        public override HttpResponseBase Response => _response;
    }
}
