﻿using System;
using System.Linq;
using System.Web.Mvc;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Infrastructure;
using IglaClub.Web.Models;
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
                ClubsWithSubscribedUser = clubs.Where(c=>c.ClubUsers.Any(cu=>cu.User.Id == user.Id)).ToList(),
                ClubsWithNotSubscribedUser = clubs.Where(c=>c.ClubUsers.All(cu=>cu.User.Id != user.Id)).ToList(),
                User = user
            };
            return View(clubsListViewModel);
        }

        //
        // GET: /Clubs/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Subscribe(long id)
        {
            try
            {
                var user = userRepository.GetUserByLogin(GetCurrentUserName());
                if (user != null)
                {
                    var userId = user.Id;
                    clubRepository.Subscribe(id, userId);

                }
            }
            catch (Exception ex)
            {
                notificationService.DisplayError("Something went wrong. Try again later.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Unsubscribe(long id)
        {
            try
            {
                var user = userRepository.GetUserByLogin(GetCurrentUserName());
                if (user != null)
                {
                    var userId = user.Id;
                    clubRepository.Unsubscribe(id, userId);

                }
            }
            catch (Exception ex)
            {
                notificationService.DisplayError("Something went wrong. Try again later.");
            }
            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Details", "Clubs", new {id});
            return Redirect(Request.UrlReferrer.ToString());
            //return RedirectToAction("Index");
        }

        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }
    }
}
