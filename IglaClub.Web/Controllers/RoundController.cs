using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.ObjectModel.Tools;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels;

namespace IglaClub.Web.Controllers
{
    [Authorize]
    public class RoundController : Controller
    {
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
        
       public ActionResult RoundDetails(long tournamentId)
        {
            var currentUser = userRepository.GetUserByLogin(HttpContext.User.Identity.Name);
            if(!this.tournamentRepository.UserIsSubscribedForTournament(currentUser.Login, tournamentId))
            {
                TempData["Message"] = "You are not subscribed for this tournament";
                return RedirectToAction("Index", "Tournament");
            }
            var tournament = this.tournamentRepository.Get<Tournament>(tournamentId);
            var results = this.resultRepository.GetResultsByTournamentAndRoundAndUser(tournamentId, tournament.CurrentRound,
                currentUser.Id);
            if (results.IsNullOrEmpty())
                return View(new RoundDetailsViewModel()
                {
                    Tournament = tournament,
                    Results = results
                });
            var firstResult = results.First();
            return View(new RoundDetailsViewModel()
            {
                Tournament = tournament,
                Results = results,
                NsPair = firstResult.NS,
                EwPair = firstResult.EW,
                TableNumber = firstResult.TableNumber
            });
        }

    }
}
