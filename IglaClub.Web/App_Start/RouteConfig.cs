using System.Web.Mvc;
using System.Web.Routing;

namespace IglaClub.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            string[] area = new[] { "IglaClub.Web.Controllers" };
            
            routes.MapRoute(
                name: "TournamentResults",
                url: "TournamentResults/{tournamentId}/{action}",
                defaults: new { controller = "Results", action = ""}, 
                namespaces: area
            );

            routes.MapRoute(
                name: "TournamentManage",
                url: "Tournament/Manage/{tournamentId}",
                defaults: new { controller = "Tournament", action = "Manage" },
                namespaces: area
            );

            routes.MapRoute(
                name: "TournamentDetails",
                url: "Tournament/Details/{tournamentId}",
                defaults: new { controller = "Tournament", action = "Details" },
                namespaces: area
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, 
                namespaces: area
            );

        }
    }
}