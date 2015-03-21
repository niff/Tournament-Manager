using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;
using System.Web.Mvc;

namespace IglaClub.Web.Authorization
{
    public class TournamentOwnerAttribute : ActionFilterAttribute
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            long tournamentId = 0;
            if (filterContext.ActionParameters.ContainsKey("tournamentId"))
                tournamentId = long.Parse(filterContext.ActionParameters["tournamentId"].ToString());
            else if (filterContext.ActionParameters.ContainsKey("id"))
                tournamentId = long.Parse(filterContext.ActionParameters["id"].ToString());
           
            string username = filterContext.HttpContext.User.Identity.Name;

            if (tournamentId == 0 || !IsOwnerOfTournament(username, tournamentId))
                HandleUnauthorizedRequest(filterContext);
        }

        private void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Errors/Error403", true);
        }

        private bool IsOwnerOfTournament(string userName, long tournamentId)
        {
            return (new TournamentRepository(db)).UserIsTournamentOwner(userName, tournamentId);
        }
    }
}