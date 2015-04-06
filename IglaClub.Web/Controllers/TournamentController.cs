using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using MvcSiteMapProvider.Web.Mvc.Filters;

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
        private const int DEFAULT_TOURNAMENT_COUNT = 10;
        //todo add caching fortournaments and invalidate on add or edit
        public TournamentController()
        {
             pairRepository = new PairRepository(db);
             userRepository = new UserRepository(db);
             tournamentRepository = new TournamentRepository(db);
             resultRepository = new ResultRepository(db);
             notificationService = new NotificationService(TempData);
        }
        
        public ActionResult Index()
        {
            var currentUser = userRepository.GetUserByLogin(GetCurrentUserName());
            var model = new TournamentMainPageModel
                {
                    CurrentlyPlayedByUser = tournamentRepository.GetCurrentlyPlayingByUser(GetCurrentUserName()),
                    UserIsManagingTournament = currentUser != null && tournamentRepository.UserIsManagingAtLeastOneTournament(currentUser.Id)
                };
            return View(model);
            //todo wazne: nawigacja w tournamentach | zrobic breadcrumb taki np: Organize > Tournaments > Tournament "tytul" > Edit results
            //

        }

        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }

        [TournamentOwner]
        [SiteMapTitle("Tournament.Name")]
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

        public ActionResult Details(long id = 0)
        {
            Tournament tournament = db.Tournaments.
                Include(t=>t.Pairs).
                Include(t=>t.Owner).
                FirstOrDefault(t=>t.Id == id);           
          
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        public ActionResult Create()
        {
            var tournament = new Tournament {BoardsInRound = 2};
            return View(tournament);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AntiSpam]
        public ActionResult Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                if (tournament.TournamentMovingType != TournamentMovingType.Cavendish)
                {
                    notificationService.DisplayError("We are very sorry, but only cavendish type is supported by now :(");
                    return View(tournament);
                }
                
                var coordinates = Request.Form["coords"];
                tournament.Coordinates = coordinates;
                tournamentManager.Create(tournament, GetCurrentUserName());
                return RedirectToAction("OwnerTournaments");
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
                 if (filterContext.HttpContext.Request.UrlReferrer != null)
                filterContext.HttpContext.Response.Redirect(filterContext.HttpContext.Request.UrlReferrer.ToString(),true);
            }

           
        }

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
            return RedirectToAction("Manage", new {tournament.Id});
        }

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
            var currentUser = userRepository.GetUserByLogin(GetCurrentUserName());
            var tournament = this.tournamentRepository.GetTournamentWithPairsAndOwner(tournamentId);
            var model = new PairsViewModel
            {
                //todo add caching for tournaments, users, and invalidate on add or edit
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
            
            return PartialView("_PairPlacing",list);
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
        
        [HttpPost]
        public void Add(PairsViewModel pairsViewModel)
        {
            if (ModelState.IsValid)
            {
                userRepository.Add(pairsViewModel.NewUser.Login, pairsViewModel.NewUser.Email);
            }
        }

        public ActionResult MoveToNextRound(long tournamentid, bool withpairsrepeat)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult MyTournamentsToPlay()
        {
            var model =
                CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetTournamentsToPlayByUser(GetCurrentUserName()), DEFAULT_TOURNAMENT_COUNT,
                    "You are playing soon", manageMode: false, showSubscriptionStatus: false);
            return PartialView("_TournamentList", model);
        }

        

        [AllowAnonymous]
        public ActionResult GetAll()
        {
            var model = CreateTounamentSingleListViewModelWithSortedItems(
                tournamentRepository.GetAll<Tournament>().OrderBy(t => t.TournamentStatus).ToList(),
                1000,
                "All tournaments"
                );
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult FindOnMap()
        {
            var model = CreateTounamentSingleListViewModelWithSortedItems(
                tournamentRepository.GetAll<Tournament>().OrderBy(t => t.TournamentStatus).ToList(),
                1000,
                "All tournaments"
                );
            return View(model);
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
            return PartialView("_TournamentList", model);
        }
        
        public ActionResult OwnerTournaments()
        {
            var model = new TounamentListViewModel
                {

                    TournamentsList = new List<TounamentSingleListViewModel>
                        {
                            GetTournamentsCreatedByUser(GetCurrentUserName())
                        }
                };
            return View("OwnerDashboard", model);//todo zamienic na wywolywanie akcji do kazdej listy w widoku a nie budowanie modelu zlozonego z list tutaj
        }

        private TounamentSingleListViewModel GetTournamentsCreatedByUser(string user)
        {
            var model = CreateTounamentSingleListViewModelWithSortedItems(
                                        tournamentRepository.GetTournamentsByOwnerUser(user),
                                        DEFAULT_TOURNAMENT_COUNT,
                                        "Tournaments created by you",
                                        manageMode :true,
                                        showSubscriptionStatus:false);

            return model;
        }

        private TounamentSingleListViewModel CreateTounamentSingleListViewModelWithSortedItems(IEnumerable<Tournament> tournaments, int defaultTournamentCount, string header, bool manageMode = false, bool showSubscriptionStatus = false)
        {
            var tournamentsDefaultCount = tournaments.Take(defaultTournamentCount).ToList();
            var dateTime = DateTime.Now;
            var future = tournamentsDefaultCount.Where(t => t.PlannedStartDate >= dateTime).OrderBy(t => t.PlannedStartDate).ToList();
            var past = tournamentsDefaultCount.Where(t => t.PlannedStartDate < dateTime).OrderByDescending(t => t.PlannedStartDate).ToList();
            return new TounamentSingleListViewModel
                {
                    Tournaments = future,
                    TournamentsPast = past,
                    Header = header,
                    ManageMode = manageMode,
                    ShowSubscriptionStatus = showSubscriptionStatus
                };
        }

        public PartialViewResult OwnerTournamentsList()
        {
            var model = GetTournamentsCreatedByUser(GetCurrentUserName());
            return PartialView("_TournamentList", model);
        }

        public ActionResult PlayerTournaments()
        {
            var model = new TounamentListViewModel
            {
                TournamentsList = new List<TounamentSingleListViewModel>
                {
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetCurrentlyPlayingByUser(GetCurrentUserName()),
                    DEFAULT_TOURNAMENT_COUNT, 
                    "Now playing"),
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetTournamentsToPlayByUser(GetCurrentUserName()),
                    DEFAULT_TOURNAMENT_COUNT, 
                    "Playing soon"),
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetTournamentsAlreadyPlayedByUser(GetCurrentUserName()),
                    DEFAULT_TOURNAMENT_COUNT, 
                    "Already played")
                }
            };
            return View("TournamentsList", model);
        }

        public ActionResult PlayerTournamentsOther()
        {
            var model = new TounamentListViewModel
            {
                TournamentsList = new List<TounamentSingleListViewModel>
                {
                    //new TounamentSingleListViewModel
                    //    {
                    //        Header = "Tournaments that you can join",
                    //        Tournaments = tournamentRepository.GetAvailableTournamentsByUser(GetCurrentUserName())
                    //                                          .OrderBy(t => t.PlannedStartDate)
                    //                                          .ToList()
                    //    }
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetAvailableTournamentsByUser(GetCurrentUserName()),
                    DEFAULT_TOURNAMENT_COUNT,
                    "Tournaments that you can join"
                    )
                }
            };

            return View("TournamentsList", model);
        }

        public ActionResult UndoStart(long id)
        {
            this.tournamentManager.UndoTournamentStart(id);
            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return Redirect(Request.UrlReferrer.ToString());
        }

        public PartialViewResult QuickUserPartial()
        {
            return PartialView("_QuickAddUser");
        }

        public ActionResult PlayerResults()
        {
            return View();
        }

        [TournamentOwner]
        public ActionResult Finish(long tournamentid)
        {
            var operationStatus = this.tournamentManager.Finish(tournamentid);
            if (operationStatus.Ok)
                notificationService.DisplaySuccess("Tournament finished");
            else
                notificationService.DisplayError("Some errors during tournament finish. \r\n Details: {0}",
                    operationStatus.ErrorMessage);
            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new { tournamentId = tournamentid });
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}