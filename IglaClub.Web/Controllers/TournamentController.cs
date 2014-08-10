using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels;

namespace IglaClub.Web.Controllers
{
    //[Authorize]
    public class TournamentController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly TournamentManager.TournamentManager tournamentManager = new TournamentManager.TournamentManager(new IglaClubDbContext());

        private readonly PairRepository pairRepository;
        private readonly UserRepository userRepository;
        private readonly TournamentRepository tournamentRepository;
        private readonly ResultRepository resultRepository;
        
        public TournamentController()
        {
             pairRepository = new PairRepository(db);
             userRepository = new UserRepository(db);
             tournamentRepository = new TournamentRepository(db);
             resultRepository = new ResultRepository(db);
        }
        
        //
        // GET: /Tournament/
        public ActionResult Index()
        {
            return View(db.Tournaments.Include(t=>t.Pairs).OrderBy(t=>t.TournamentStatus).ToList());
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
                tournamentManager.Create(tournament);
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
            if (!ModelState.IsValid) 
                return View(tournament);
            
            db.Entry(tournament).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return RedirectToAction("Manage", new {tournamentId = tournament.Id});
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
            tournamentManager.DeleteTournament(id);
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

        public ActionResult CalculateResultsComplete(long id)
        {
            tournamentManager.CalculateResultsComplete(id);

            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult CalculateResults(long id)
        {
            tournamentManager.CalculateResults(id);

            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult CalculateScore(long id)
        {
            tournamentManager.CalculateScore(id);
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Results", new { tournamentId = id });
            return Redirect(Request.UrlReferrer.ToString());
            
        }

        [HttpPost]
        public ActionResult RemovePair(long tournamentId, long pairId)
        {
            Tournament tournament = db.Tournaments.Find(tournamentId);
            Pair pair = db.Pairs.Find(pairId);
            tournament.Pairs.Remove(pair);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult AddPair(int user1, int user2, int tournamentId)
        {
            tournamentManager.AddPair(tournamentId, user1, user2);
            return Json(new { success = true });
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

        public PartialViewResult PairRoster(int tournamentId)
        {

            var results = resultRepository.GetResultsFromCurrentRound(tournamentId);
            var list = new List<PairRosterViewModel>();
            foreach (var result in results)
            {
                if(list.Any(prvm=>prvm.TableNumber == result.TableNumber))
                    continue;
                list.Add(new PairRosterViewModel(){NsPair = result.NS, EwPair = result.EW, TableNumber = result.TableNumber, Section = 0});
            }
            
            return PartialView("_PairRoster",list);
        }

        public JsonResult SearchUsers(long tournamentId, string phrase)
        {
            var result = userRepository.GetUsersByPhraseAndTournament(tournamentId, phrase)
                .Select(u => new { u.Id, value = u.Name +" "+ u.Lastname  + ( (!String.IsNullOrWhiteSpace(u.Nickname) ) ? " (" + u.Nickname + ")" : "")})
                        .Take(10)
                        .ToArray();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult GenerateNextRound(long tournamentId, bool withPairsRepeat)
        {
            var status = tournamentManager.GenerateNextRound(tournamentId, withPairsRepeat);
            //TODO: error message handling if(status.Ok == false)
            if(status.Ok == false)
                return Redirect(Request.UrlReferrer.ToString());
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new { tournamentId = tournamentId });
            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult AddUser(string name, string email)
        {
            var id =  userRepository.Add(name, email);
            return Json(id);
        }

        public ActionResult MoveToNextRound(long tournamentid, bool withpairsrepeat)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult Oncoming()
        {
            return PartialView("_TournamentList", tournamentRepository.GetOncoming());
        }


        public PartialViewResult Past()
        {
            return PartialView("_TournamentList", tournamentRepository.GetPast());
        }
        
        public PartialViewResult Ongoing()
        {
            return PartialView("_TournamentList", tournamentRepository.GetOngoing());
        }
    }
}