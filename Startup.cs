using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wb6.Startup))]
namespace wb6
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
