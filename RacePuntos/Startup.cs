using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RacePuntos.Startup))]
namespace RacePuntos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
