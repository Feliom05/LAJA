using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Laja.Startup))]
namespace Laja
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
