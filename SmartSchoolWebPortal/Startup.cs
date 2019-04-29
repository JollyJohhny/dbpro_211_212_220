using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartSchoolWebPortal.Startup))]
namespace SmartSchoolWebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
