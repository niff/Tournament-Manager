using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IglaClub.Web.Authorization
{
    public class TournamentOwnerAttribute : AuthorizeAttribute
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                // The user is not authenticated
                return false;
            }

            var user = httpContext.User;
            //if (user.IsInRole("Admin"))
            //{
            //    // Administrator => let him in
            //    return true;
            //}

            var rd = httpContext.Request.RequestContext.RouteData;

            long id = 0;
            if (rd.Values.ContainsKey("id"))
                id = long.Parse(rd.Values["id"].ToString());
            else if (rd.Values.ContainsKey("tournamentId"))
                id = long.Parse(rd.Values["tournamentId"].ToString());

            if (id == 0)
                return false;

            return IsOwnerOfTournament(user.Identity.Name, id);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Errors/Error403");
        }

        private bool IsOwnerOfTournament(string userName, long tournamentId)
        {
            return (new TournamentRepository(db)).UserIsTournamentOwner(userName, tournamentId);
        }
    }
}