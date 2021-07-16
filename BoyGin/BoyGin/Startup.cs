using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoyGin.Startup))]
namespace BoyGin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
