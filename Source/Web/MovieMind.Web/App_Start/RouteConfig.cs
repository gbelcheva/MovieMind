namespace MovieMind.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "MoviePartial",
                url: "Movies/MovieById",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "MovieMind.Web.Controllers" });

            routes.MapRoute(
                name: "MovieDetails",
                url: "Movies/Movie/{id}",
                defaults: new { controller = "Movies", action = "MovieById" },
                namespaces: new[] { "MovieMind.Web.Controllers" });

            routes.MapRoute(
                name: "AllMovies",
                url: "Movies/Page/{page}",
                defaults: new { controller = "Movies", action = "All" },
                namespaces: new[] { "MovieMind.Web.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MovieMind.Web.Controllers" });
        }
    }
}
