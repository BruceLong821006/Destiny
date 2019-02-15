using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Destiny.Web.Startup))]
namespace Destiny.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
