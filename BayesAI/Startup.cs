using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BayesAI.Startup))]
namespace BayesAI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
