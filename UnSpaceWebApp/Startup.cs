using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnSpaceWebApp.Startup))]
namespace UnSpaceWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
