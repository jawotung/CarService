using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarService.Startup))]
namespace CarService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}