using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPO.Startup))]
namespace SPO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
