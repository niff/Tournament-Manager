using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
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
        private readonly ResultRepository resultRepository;

        public RoundController()
        {
            pairRepository = new PairRepository(db);
            userRepository = new UserRepository(db);
            tournamentRepository = new TournamentRepository(db);
            resultRepository = new ResultRepository(db);
        }
        
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult Index(long tournamentId, int? tableNumber = null, int? roundNumber = null)
        //{
        //    //todo: not authorized check (i.e. pair is not playing here)
        //    var roundVm = new RoundVm();
        //    //return View();
        //    throw new NotImplementedException();
        //}

        public ActionResult RoundDetails(long tournamentId)
        {
            var currentUser = userRepository.GetUserByName(HttpContext.User.Identity.Name);
            var tournament = this.tournamentRepository.Get<Tournament>(tournamentId);
            var results = this.resultRepository.GetResultsByRoundAndUser(tournamentId, tournament.CurrentRound,
                currentUser.Id);
            if (results == null || results.Count == 0)
                return View(new RoundDetailsViewModel()
                {
                    Tournament = tournament,
                    Results = results
                });
            return View(new RoundDetailsViewModel()
            {
                Tournament = tournament,
                Results = results,
                NsPair = results.FirstOrDefault().NS,
                EwPair = results.FirstOrDefault().EW
            });
        }

    }
}
