using System.Web;

namespace GameStore.WEB.Tests.Routes.Stubs
{
    public class StubHttpResponseForRouting : HttpResponseBase
    {
        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }
    }
}
