using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Infrastructure;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels;
using IglaClub.Web.Models.ViewModels.Clubs;

namespace IglaClub.Web.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly ClubRepository clubRepository;
        private readonly UserRepository userRepository;
        private readonly NotificationService notificationService;


        public ClubsController()
        {
            clubRepository = new ClubRepository(db);
            userRepository = new UserRepository(db);
            notificationService = new NotificationService(TempData);
        }
        //
        // GET: /Clubs/

        public ActionResult Index()
        {
            var user = this.userRepository.GetUserByLogin(User.Identity.Name);
            var clubs = this.db.Clubs.Include("ClubUsers");
            var clubsListViewModel = new ClubsIndexViewModel
            {
                ClubsWithSubscribedUser = clubs.Where(c => c.ClubUsers.Any(cu => cu.User.Id == user.Id)).ToList(),
                ClubsWithNotSubscribedUser = clubs.Where(c => c.ClubUsers.All(cu => cu.User.Id != user.Id)).ToList(),
                User = user
            };
            return View(clubsListViewModel);
        }

        //
        // GET: /Clubs/Details/5

        public ActionResult Details(int id)
        {
            var club = clubRepository.Get<Club>(id);
            return View(club);
        }
        public PartialViewResult ClubMembers(int clubId)
        {
            var club = clubRepository.Get<Club>(clubId);
            IList<ClubUser> clubUsers = clubRepository.GetClubUsers(clubId);
            var model = new ClubMembersViewModel(clubUsers, club);
            return PartialView("_ClubMembers", model);
        }

        public ActionResult Subscribe(long id)
        {
            try
            {
                var user = userRepository.GetUserByLogin(GetCurrentUserName());
                var userId = user.Id;
                clubRepository.Subscribe(id, userId);

            }
            catch (Exception)
            {
                notificationService.DisplayError("Something went wrong. Try again later.");
            }
            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Index");
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Unsubscribe(long clubId, long userId)
        {
            try
            {
                clubRepository.Unsubscribe(clubId, userId);

                //var user = userRepository.GetUserByLogin(GetCurrentUserName());
                //if (user != null)
                //{
                //    var userId = user.Id;
                    
                //}
            }
            catch (Exception ex)
            {
                notificationService.DisplayError("Something went wrong. Try again later.");
            }

            if (Request.UrlReferrer == null)
                return RedirectToAction("Details", "Clubs", new { id = clubId });
            return Redirect(Request.UrlReferrer.ToString());
        }

        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }
    }
}
