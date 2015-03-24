using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Enums;
using IglaClub.ObjectModel.Exceptions;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Authorization;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels;
using IglaClub.Web.Infrastructure;

namespace IglaClub.Web.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly TournamentManager.TournamentManager tournamentManager = new TournamentManager.TournamentManager(new IglaClubDbContext());

        private readonly PairRepository pairRepository;
        private readonly UserRepository userRepository;
        private readonly TournamentRepository tournamentRepository;
        private readonly ResultRepository resultRepository;

        private readonly INotificationService notificationService;
        
        public TournamentController()
        {
             pairRepository = new PairRepository(db);
             userRepository = new UserRepository(db);
             tournamentRepository = new TournamentRepository(db);
             resultRepository = new ResultRepository(db);
             notificationService = new NotificationService(TempData);
        }
        
        //
        // GET: /Tournament/
        
        public ActionResult Index()
        {
            var model = new TournamentMainPageModel
                {
                    CurrentlyPlayedByUser = tournamentRepository.GetCurrentlyPlayingByUser(GetCurrentUserName())
                };
            return View(model);
        }

        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }

        public ActionResult GetAll()
        {
            var model = tournamentRepository.GetAll<Tournament>().ToList();
            return View(model);
        }

        //
        // GET: /Tournament/Manage/5

        [TournamentOwner]
        public ActionResult Manage(long id = 0)
        {
            
            Tournament tournament = db.Tournaments.
                Include(t=>t.Pairs).
                Include(t=>t.Owner).
                FirstOrDefault(t=>t.Id == id);
            
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
            Tournament tournament = db.Tournaments.
                Include(t=>t.Pairs).
                Include(t=>t.Owner).
                FirstOrDefault(t=>t.Id == id);
            
            
            //Tournament tournament = db.Tournaments.Find(id);
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
            var tournament = new Tournament {BoardsInRound = 2};
            return View(tournament);
        }

        //
        // POST: /Tournament/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                if (tournament.TournamentMovingType != TournamentMovingType.Cavendish)
                {
                    notificationService.DisplayError("We are very sorry, but only cavendish type is supported by now :(");
                    return View(tournament);
                }
                
                //tournament.Owner = userRepository.GetUserByName(User.Identity.Name);
                var coordinates = Request.Form["coords"];
                tournament.Coordinates = coordinates;
                tournamentManager.Create(tournament, GetCurrentUserName());
                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        //todo: check why it doesn't show the message on redirect
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) 
                return;
            
            Exception ex = filterContext.Exception;
            if (ex.GetType() == typeof(OperationException))
            {
                this.notificationService.DisplayMessage(ex.Message, NotificationType.Warning);
               // notificationService.DisplayError(ex.Message);
            }

            if (filterContext.HttpContext.Request.UrlReferrer != null)
                filterContext.HttpContext.Response.Redirect(filterContext.HttpContext.Request.UrlReferrer.ToString(),true);
        }


        //
        // GET: /Tournament/Edit/5

        [TournamentOwner]
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
            if (tournament.TournamentMovingType != TournamentMovingType.Cavendish)
            {
                notificationService.DisplayError("We are very sorry, but only cavendish type is supported by now :(");
                return View(tournament);
            }
            
            var coordinates = Request.Form["coords"];
            tournament.Coordinates = coordinates;
            db.Entry(tournament).State = EntityState.Modified;
            db.SaveChanges();
            //return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return RedirectToAction("Manage", new {tournament.Id});
        }

        //
        // GET: /Tournament/Delete/5
        [TournamentOwner]
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

        [TournamentOwner]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tournamentManager.DeleteTournament(id);
            notificationService.DisplaySuccess("Tournament with id {0} deleted", id);
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
        public ActionResult AddPair(int user1, int user2, long tournamentId)
        {
            tournamentManager.AddPair(tournamentId, user1, user2);
            return Json(new { success = true });
        }
        
        public PartialViewResult TournamentParticipantsEdit(long tournamentId)
        {
            var model = CreatePairsViewModel(tournamentId);
            return PartialView("_TournamentParticipantsEdit", model);
        }

        public PartialViewResult PairsList(long tournamentId)
        {
            var model = CreatePairsViewModel(tournamentId);
            return PartialView("_PairsList", model);
        }

        private PairsViewModel CreatePairsViewModel(long tournamentId)
        {
            var currentUser = userRepository.GetUserByName(GetCurrentUserName());
            var tournament = this.tournamentRepository.GetTournamentWithPairsAndOwner(tournamentId);
            var model = new PairsViewModel
            {
                PairsInTounament = tournament.Pairs.ToList(),
                AvailableUsers = userRepository.GetAvailableUsersForTournament(tournamentId),
                Tournament = tournament,
                CurrentUser = currentUser
            };
            return model;
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

        public ActionResult GenerateNextRound(long tournamentId, bool withPairsRepeat)
        {
            var status = tournamentManager.GenerateNextRound(tournamentId, withPairsRepeat);

            if (!status.Ok)
            {
                this.notificationService.DisplayMessage(status.ErrorMessage, NotificationType.Warning);
            }
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new {tournamentId });
            return Redirect(Request.UrlReferrer.ToString());
        }
        
        public JsonResult SearchUsers(long tournamentId, string phrase)
        {
            var result = userRepository.GetUsersByPhraseAndTournament(tournamentId, phrase)
                .Select(u => new { u.Id, value = u.Name +" "+ u.Lastname  + ( (!String.IsNullOrWhiteSpace(u.Login) ) ? " (" + u.Login + ")" : "")})
                        .Take(10)
                        .ToArray();
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public JsonResult AddUser(string name, string email, string password)
        {
            long id;
            try
            {
                id =  userRepository.Add(name, email);
            }
            catch (SqlException sqlException)
            {
                return Json(new {data = -1, errorStatus = sqlException.Message});
            }
            return Json(new {Data = id, ErrorStatus = ""});
            
        }

        [HttpPost]
        public ActionResult Add(PairsViewModel pairsViewModel)
        {
            if (ModelState.IsValid)
            {
                userRepository.Add(pairsViewModel.NewUser.Login, pairsViewModel.NewUser.Email);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult MoveToNextRound(long tournamentid, bool withpairsrepeat)
        {
            throw new NotImplementedException();
        }

        //public PartialViewResult Oncoming()
        //{
        //    return PartialView("_TournamentList", tournamentRepository.GetTournamentsToPlayForUser(GetCurrentUserName()));
        //}

        //public PartialViewResult Past()
        //{
        //    return PartialView("_TournamentList", tournamentRepository.GetFinished().ToList());
        //}
        
        //public PartialViewResult Ongoing()
        //{
        //    return PartialView("_TournamentList", tournamentRepository.GetOngoing().ToList());
        //}



        [HttpPost]
        public void QuickAddUser(string name, string email)
        {
            userRepository.Add(name, email);
            //return RedirectToRoute(Request.UrlReferrer);
            //return View("_QuickAddUser");
        }

        public PartialViewResult MyTournamentsToPlay()
        {
            var model = new TounamentSingleListViewModel
                {
                    Tournaments = tournamentRepository.GetTournamentsToPlayByUser(GetCurrentUserName()),
                    Header = "My tournaments"
                };
            return PartialView("_TournamentList", model);
        }

        public PartialViewResult AvailableTournaments()
        {
            var tournaments = tournamentRepository.GetAvailableTournamentsByUser(GetCurrentUserName()).OrderBy(t => t.PlannedStartDate);
            var pastItemsAtTheEnd = tournaments.Where(t => t.PlannedStartDate >= DateTime.Now)
               .Union(tournaments.Where(t => t.PlannedStartDate < DateTime.Now)).ToList();
            var model = new TounamentSingleListViewModel
                {
                    Tournaments = pastItemsAtTheEnd,
                    Header = "Other tournaments"
                };
            //ViewBag.Title = "Other tournaments";
            return PartialView("_TournamentList", model);
        }

        public ActionResult MyOrganizedTournaments()
        {
            var model = tournamentRepository.GetTournamentsByOwnerUser(GetCurrentUserName()).ToList();
            ViewBag.Title = "Tournaments created by you";
            return View("TournamentsByOwner", model);
        }

        //public ActionResult AllTournamentsMap()
        //{
        //    return View();
        //}

        public ActionResult PlayerTournaments()
        {
            var model = new TounamentListViewModel
            {
                TournamentsList = new List<TounamentSingleListViewModel>
                {
                    new TounamentSingleListViewModel
                        {
                            Header = "Now playing",
                            Tournaments = tournamentRepository.GetCurrentlyPlayingByUser(GetCurrentUserName())
                        },
                    new TounamentSingleListViewModel
                        {
                            Header = "Playing soon",
                            Tournaments = tournamentRepository.GetTournamentsToPlayByUser(GetCurrentUserName())
                        },
                        new TounamentSingleListViewModel
                        {
                            Header = "Already played",
                            Tournaments = tournamentRepository.GetTournamentsAlreadyPlayedByUser(GetCurrentUserName())
                        }
                }
            };
            return View("TournamentsList", model);
        }

        public ActionResult PlayerTournamentsOther()
        {
            ViewBag.Title = "Available tournaments";
            var model = new TounamentListViewModel
            {
                TournamentsList = new List<TounamentSingleListViewModel>
                {
                    new TounamentSingleListViewModel
                        {
                            Header = "Tournaments that you can join",
                            Tournaments = tournamentRepository.GetAvailableTournamentsByUser(GetCurrentUserName())
                                                              .OrderBy(t => t.PlannedStartDate)
                                                              .ToList()
                        }
                }
            };

            return View("TournamentsList", model);
        }
    }

    public class TounamentListViewModel
    {
        public IList<TounamentSingleListViewModel> TournamentsList { get; set; }
    }
    public class TounamentSingleListViewModel
    {
        public IList<Tournament> Tournaments { get; set; }
        public string Header { get; set; }
    }
}