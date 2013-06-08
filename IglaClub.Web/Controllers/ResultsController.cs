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

        public ResultsController()
        {
            pairRepository = new PairRepository(db);
            userRepository = new UserRepository(db);
        }
        
        //Results from tournament, as partial view
        // GET: /Results/1
        public PartialViewResult Index(long tournamentId)
        {
            throw new NotImplementedException();
            //return View(db.Tournaments.ToList());
        }
        
        
        //
        // POST: /Tournament/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage", new { id = result.Tournament.Id});
            }
            return RedirectToAction("Manage", new { id = result.Tournament.Id });
            //return View(result);
        }
        
        //
        // GET: /Results/Manage/5

        public ActionResult Manage(long tournamentId)
        {
            Tournament tournament = db.Tournaments
                                      .Include(t => t.Results)
                                      .Include(t => t.Results.Select(r => r.NS))
                                      .Include(t => t.Results.Select(r => r.EW))
                                      .Include(t => t.Boards)
                                      .Include(t => t.Pairs).FirstOrDefault(t => t.Id == tournamentId);
                
            return View(tournament);
        }

        public ActionResult CreateEmpty(long tournamentId)
        {
            var tournament = db.Tournaments.Find(tournamentId);
            var result = new Result {Tournament = tournament};
            db.Results.Add(result);
            return RedirectToAction("Manage",new { tournamentId });
        }

        //
        // POST: /Tournament/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Manage", new { id = result.Tournament.Id});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Calculate(long id)
        {
            throw new NotImplementedException();
        }

    }
}