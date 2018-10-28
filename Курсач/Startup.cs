using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Курсач.Startup))]
namespace Курсач
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
