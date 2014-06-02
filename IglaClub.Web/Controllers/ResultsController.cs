using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels;

namespace IglaClub.Web.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly TournamentManager.TournamentManager tournamentManager = new TournamentManager.TournamentManager(new IglaClubDbContext());

        private readonly PairRepository pairRepository;
        private readonly UserRepository userRepository;
        private readonly ResultRepository resultRepository;
        private readonly TournamentRepository tournamentRepository;

        public ResultsController()
        {
            pairRepository = new PairRepository(db);
            userRepository = new UserRepository(db);
            resultRepository = new ResultRepository(db);
            tournamentRepository = new TournamentRepository(db);
        }

        public PartialViewResult Index(long tournamentId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(long tournamentId)
        {
            var model = db.Tournaments.Find(tournamentId).Results.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(List<Result> results)
        {

            if (results != null)
            {
                foreach (var result in results)
                {
                    resultRepository.InsertOrUpdate(result);
                }

                if (results.Any())
                {
                    this.tournamentManager.CalculateResultsComplete(results[0].TournamentId);
                    return RedirectToAction("Manage", new { results.FirstOrDefault().TournamentId });
                }
            }
            return null;
        }

        public ActionResult Manage(long tournamentId, string sort, string sortdir)
        {
            Tournament tournament = tournamentRepository.GetTournament(tournamentId);
            List<Result> results = tournament.Results.ToList();
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "BoardNumber":
                        results = tournament.Results.OrderBy(r => r.Board.BoardNumber).ToList();
                        break;
                    case "TableNumber":
                        results = tournament.Results.OrderBy(r => r.TableNumber).ToList();
                        break;
                    default:
                        results = tournament.Results.OrderBy(r => r.RoundNumber).ToList();
                        break;
                }
                if (sortdir == "DESC")
                    results.Reverse();
            }
            return View(new TournamentResultsVm{Tournament = tournament, Results = results});
        }

        public ActionResult CreateEmpty(long tournamentId)
        {
            var tournament = db.Tournaments.Find(tournamentId);
            var result = new Result {Tournament = tournament};
            db.Results.Add(result);
            return RedirectToAction("Manage",new { tournamentId });
        }

        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            long tournamentId = tournamentManager.DeleteResult(id);
            return RedirectToAction("Manage", new { tournamentId});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult RemoveLastRound(long id)
        {
            tournamentManager.RemoveLastRound(id);
            return RedirectToAction("Manage", new { tournamentId = id });
        }

        public ActionResult GenerateNextRound(long tournamentId, bool withPairsRepeat)
        {
            tournamentManager.GenerateNextRound(tournamentId, withPairsRepeat);
            return RedirectToAction("Manage", new { tournamentId });
        }

        public PartialViewResult PairsResults(int tournamentId)
        {
            var tournament = tournamentRepository.GetTournament(tournamentId);
            Dictionary<long, int> pairNumberMaxPoints = resultRepository.GetDictionaryPairNumberMaxPoints(tournamentId);
            var pairsResultsViewModel = new PairsResultsViewModel
                {
                    TournamentScoringType = tournament.TournamentScoringType,
                    PairsInTounament = pairRepository.GetPairsByTournament(tournamentId).OrderByDescending(p=>p.Score).ToList(),
                    PairNumberMaxPoints = pairNumberMaxPoints
                    
                };

            return PartialView("_PairsResults", pairsResultsViewModel);
        }
    }
}