using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Selama.Startup))]
namespace Selama
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
