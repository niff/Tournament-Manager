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
    public class TournamentController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly TournamentManager.TournamentManager tournamentManager = new TournamentManager.TournamentManager(new IglaClubDbContext());

        private readonly PairRepository pairRepository;
        private readonly UserRepository userRepository;

        public TournamentController()
        {
             pairRepository = new PairRepository(db);
             userRepository = new UserRepository(db);
        }
        
        //
        // GET: /Tournament/
        public ActionResult Index()
        {
            return View(db.Tournaments.ToList());
        }

        //
        // GET: /Tournament/Manage/5

        public ActionResult Manage(long id = 0)
        {
            
            Tournament tournament = db.Tournaments.Include(t=>t.Pairs).FirstOrDefault(t=>t.Id == id);
            
            if (tournament == null)
            {
                return HttpNotFound();
            }
            var tournamentVm = new TournamentManageVm() { Tournament = tournament };
            return View(tournamentVm);
            
        }

        //
        // GET: /Tournament/Details/5

        public ActionResult Details(long id = 0)
        {
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        //
        // GET: /Tournament/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tournament/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        //
        // GET: /Tournament/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        //
        // POST: /Tournament/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournament);
        }

        //
        // GET: /Tournament/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        //
        // POST: /Tournament/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Tournament tournament = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Start(long id)
        {
            this.tournamentManager.StartTournament(id);
            return RedirectToAction("Manage", new {id});
        }

        public ActionResult CalculateResults(long id)
        {
            //Tournament tournament = db.Tournaments.Find(tournamentId);
            //return View("CalculateResults",tournament.Pairs);
            throw new NotImplementedException();
        }

        public ActionResult GenerateNextRound()
        {
            throw new NotImplementedException();
        }

        public ActionResult RemovePair(long tournamentId, long pairId )
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            Pair pair = db.Pairs.Find(pairId);
            tournament.Pairs.Remove(pair);
            db.SaveChanges();
            return RedirectToAction("Manage");
        }


        public ActionResult AddPair(long id)
        {
            this.tournamentManager.AddPair(id, 1, 2);

            return RedirectToAction("Manage", new { id });
        }

        public ActionResult ManageResults(long id)
        {
            Tournament tournament = db.Tournaments
                .Include(t => t.Results)
                .Include(t => t.Results.Select(r=>r.NS))
                .Include(t => t.Results.Select(r => r.EW))
                .Include(t => t.Pairs).FirstOrDefault(t => t.Id == id);

            //return View(new TournamentResultsVm() {Tournament = tournament});
            return View(tournament);
        }

        public PartialViewResult Pairs(int tournamentId)
        {
            var model = new PairsViewModel
                {
                    PairsInTounament = pairRepository.GetPairsByTournament(tournamentId),
                    AvailableUsers = userRepository.GetAvailableUsersForTournament(tournamentId),
                    Tournament = db.Tournaments.Find(tournamentId)
                };
            return PartialView("_TournamentParticipants", model);
        }

        public JsonResult SearchUsers(long tournamentId, string phrase)
        {
            var result = userRepository.GetUsersByPhraseAndTournament(tournamentId, phrase)
                        .Select(u => new { u.Id, value = u.Name })
                        .Take(10)
                        .ToArray();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}