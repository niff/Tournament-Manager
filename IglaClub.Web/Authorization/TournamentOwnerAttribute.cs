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
                return false;
            }

            var user = httpContext.User;
            
            var rd = httpContext.Request.RequestContext.RouteData;

            long tournamentId = 0;
            if (rd.Values.ContainsKey("id"))
                tournamentId = long.Parse(rd.Values["id"].ToString());
            else if (rd.Values.ContainsKey("tournamentId"))
                tournamentId = long.Parse(rd.Values["tournamentId"].ToString());

            if (tournamentId == 0)
                return false;

            return IsOwnerOfTournament(user.Identity.Name, tournamentId);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Errors/Error403", true);
        }

        private bool IsOwnerOfTournament(string userName, long tournamentId)
        {
            return (new TournamentRepository(db)).UserIsTournamentOwner(userName, tournamentId);
        }
    }
}