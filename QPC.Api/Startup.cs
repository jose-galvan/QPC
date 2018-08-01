using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QPC.Api.Startup))]

namespace QPC.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
