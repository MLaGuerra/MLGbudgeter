using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLGbudgeter.Startup))]
namespace MLGbudgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
