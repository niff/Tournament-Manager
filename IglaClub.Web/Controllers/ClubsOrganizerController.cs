using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Infrastructure;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels.Clubs;

namespace IglaClub.Web.Controllers
{
    public class ClubsOrganizerController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly ClubRepository clubRepository;
        private readonly UserRepository userRepository;
        private readonly NotificationService notificationService;


        public ClubsOrganizerController()
        {
            clubRepository = new ClubRepository(db);
            userRepository = new UserRepository(db);
            notificationService = new NotificationService(TempData);
        }

        public ActionResult MyClubs()
        {
            var clubs = db.Clubs.Where(c=>c.ClubUsers.Any(cu=>cu.User.Login == User.Identity.Name && cu.IsAdministrator));
            return View(clubs);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Club club)
        {
            if (!ModelState.IsValid)
                return View();
            
            var user = userRepository.GetUserByLogin(User.Identity.Name);

            club.CreationDate = DateTime.UtcNow;
            clubRepository.Insert(club, user);
            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var club = clubRepository.Get<Club>(id);
            return View(club);
        }

        [HttpPost]
        public ActionResult Edit(Club club)
        {
            //var coordinates = Request.Form["coords"];
            //club.Coordinates = coordinates;
            clubRepository.Update(club);
            return View("Details", club);
        }

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
            return PartialView("_ClubMembersAdmin", model);
        }

        public ActionResult Index()
        {
            var user = userRepository.GetUserByLogin(User.Identity.Name);
            var clubs = clubRepository.GetClubsByAdmin(user.Id);
            return View(new ClubsIndexAdminViewModel(clubs));
        }

        public ActionResult Delete(long id)
        {
            var club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
            notificationService.DisplayInfo("Club {0} removed.",club.Name);
            return RedirectToAction("Index");
        }

    }
}