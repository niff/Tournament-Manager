using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.Web.Models;

namespace IglaClub.Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();

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

            return View(tournament);
            
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
            throw new NotImplementedException();
        }

        public ActionResult CalculateResults(long id)
        {
            //Tournament tournament = db.Tournaments.Find(id);
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
            Tournament tournament = db.Tournaments.Find(id);
            //tournament.Pairs.Add(new Pair(){Tournament = tournament,PairNumber = 1, Users = new List<User>(){new User(){Name = "Pierwszy"},new User(){Name = "Drugi"}}});
            tournament.Pairs.Add(new Pair() { Tournament = tournament, PairNumber = 1, Player1 = new User() { Name = "Pierwszy" }, Player2 = new User() { Name = "Drugi" } } );
            db.SaveChanges();
            return RedirectToAction("Manage",new {id = id});
        }
    }
}