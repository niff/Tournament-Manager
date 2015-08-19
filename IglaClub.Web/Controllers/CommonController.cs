using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;

namespace IglaClub.Web.Controllers
{
    public class CommonController : Controller
    {
        protected readonly IglaClubDbContext Db = new IglaClubDbContext();

        protected readonly UserRepository UserRepository;

        public CommonController()
        {
            UserRepository = new UserRepository(Db);            
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserId = UserRepository.GetUserByLogin(User.Identity.Name).Id;
            }
            if (Request.QueryString["tournamentId"] != null)
                ViewBag.TournamentId = Request.QueryString["tournamentId"];
            else if (Request.RequestContext.RouteData.Values["tournamentId"] != null)
                ViewBag.TournamentId = Request.RequestContext.RouteData.Values["tournamentId"];

            base.OnActionExecuting(filterContext);
        }

    }
}
