using Moq;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace QPC.Web.Tests.Extensions
{
    public static class ControllerExtensions
    {
        public static void MockCurrentUser(this ApiController controller, string userId, string username)
        {
            var claim = new Claim("test", userId);
            var mockIdentity =
                Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
            controller.User = Mock.Of<IPrincipal>(ip => ip.Identity == mockIdentity);
        }


        public static Guid GetGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }
    }
}