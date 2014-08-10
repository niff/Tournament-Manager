using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels;

namespace IglaClub.Web.Controllers
{
    [Authorize]
    public class RoundController : Controller
    {
        //
        // GET: /Round/
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly TournamentManager.TournamentManager tournamentManager = new TournamentManager.TournamentManager(new IglaClubDbContext());

        private readonly PairRepository pairRepository;
        private readonly UserRepository userRepository;
        private readonly TournamentRepository tournamentRepository;

        public RoundController()
        {
            pairRepository = new PairRepository(db);
            userRepository = new UserRepository(db);
            tournamentRepository = new TournamentRepository(db);
        }
        
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(long tournamentId, int? tableNumber = null, int? roundNumber = null)
        {
            //todo: not authorized check (i.e. pair is not playing here)
            var roundVm = new RoundVm();
            //return View();
            throw new NotImplementedException();
        }

    }
}
