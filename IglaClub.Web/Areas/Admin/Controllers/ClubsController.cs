using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;

namespace IglaClub.Web.Areas.Admin.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly ClubRepository clubRepository;

        public ClubsController()
        {
            clubRepository = new ClubRepository(db);
        }

        public ActionResult MyClubs()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Club club)
        {
            var coordinates = Request.Form["coords"];
            club.Coordinates = coordinates;
            clubRepository.Add(club);
            return View();
        }

        public ActionResult Edit(int id)
        {
            var club = clubRepository.Get<Club>(id);
            return View(club);
        }

        [HttpPost]
        public ActionResult Edit(Club club)
        {
            var coordinates = Request.Form["coords"];
            club.Coordinates = coordinates;
            clubRepository.Update(club);
            return View("Details", club);
        }

        public ActionResult Details(int id)
        {
            var club = clubRepository.Get<Club>(id);
            return View(club);
        }
    }
}