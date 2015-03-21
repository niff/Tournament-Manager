﻿using System.Web.Mvc;
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
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, 
                namespaces: area
            );
            routes.MapRoute(
                name: "DefaultWithTournament",
                url: "Results/{action}/{tournamentId}",
                defaults: new { controller = "Results", action = "Manage" },
                namespaces: area
            );

        }
    }
}