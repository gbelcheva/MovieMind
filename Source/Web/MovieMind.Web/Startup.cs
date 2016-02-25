using Microsoft.Owin;

using Owin;

[assembly: OwinStartupAttribute(typeof(MovieMind.Web.Startup))]

namespace MovieMind.Web
{
    /// <summary>
    /// Startup class for MVC architecture
    /// </summary>
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
