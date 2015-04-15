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
using MvcSiteMapProvider;
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
        private const int MAX_TOURNAMENT_COUNT = 1000;
        //todo add caching for tournaments and invalidate on add or edit
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
        }

        private string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
        }

        [TournamentOwner]
        [SiteMapTitle("Tournament.Name")]
        public ActionResult Manage(long tournamentId = 0)
        {
            Tournament tournament = db.Tournaments.
                Include(t=>t.Pairs).
                Include(t=>t.Owner).
                FirstOrDefault(t => t.Id == tournamentId);
            
            if (tournament == null)
            {
                return HttpNotFound();
            }
            var tournamentVm = new TournamentManageVm() { Tournament = tournament };
            return View(tournamentVm);
            
        }

        [SiteMapTitle("TournamentName")]
        public ActionResult Details(long tournamentId = 0)
        {
            Tournament tournament = db.Tournaments.
                Include(t=>t.Pairs).
                Include(t=>t.Owner).
                FirstOrDefault(t => t.Id == tournamentId);           
          
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewData["TournamentName"] = tournament.Name;
            return View(tournament);
        }
        [SiteMapTitle("SMCurrentRound", Target = AttributeTarget.ParentNode)]
        [SiteMapTitle("SMTitle")]
        public ActionResult TournamentResults(long tournamentId = 0)
        {
            Tournament tournament = db.Tournaments.
                Include(t=>t.Pairs).
                Include(t=>t.Owner).
                FirstOrDefault(t => t.Id == tournamentId);           
          
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewData["SMCurrentRound"] = "Current round - " + tournament.CurrentRound;
            ViewData["SMTitle"] = "All results";
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
                
                //var coordinates = Request.Form["coords"];
                //tournament.Coordinates = coordinates;
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
        [SiteMapTitle("Name", Target= AttributeTarget.ParentNode)]
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
            
            //var coordinates = Request.Form["coords"];
            //tournament.Coordinates = coordinates;
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
            var result = this.tournamentManager.StartTournament(id);
            if (!result.Ok)
            {
                notificationService.DisplayError(result.ErrorMessage);
            }
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

        public PartialViewResult TournamentsListPlannedUserNotSubscribedPartial()
        {
            var model = TournamentsListPlannedUserNotSubscribedModel(DEFAULT_TOURNAMENT_COUNT);
            return PartialView("_TournamentList", model);
        }

        [ActionName("Join")]
        public ActionResult TournamentsListPlannedUserNotSubscribed()
        {
            var model = new TounamentListViewModel
                {
                    TournamentsList = new List<TounamentSingleListViewModel>
                        {
                            TournamentsListPlannedUserNotSubscribedModel(1000)
                        }
                };
            return View("TournamentsList", model);
        }

        private TounamentSingleListViewModel TournamentsListPlannedUserNotSubscribedModel(int count)
        {
            return CreateTounamentSingleListViewModelWithSortedItems(
                tournamentRepository.GetAvailableTournamentsByUser(GetCurrentUserName()),
                count,
                "Tournaments that you can join",
                manageMode: false,
                showSubscriptionStatus: true
                );
        }

        //[SiteMapTitle("Organize tournaments", Target = AttributeTarget.ParentNode)]
        public ActionResult OwnerTournaments()
        {
            var model = new TounamentListViewModel
                {

                    TournamentsList = new List<TounamentSingleListViewModel>
                        {
                            GetTournamentsCreatedByUser(GetCurrentUserName(), MAX_TOURNAMENT_COUNT)
                        }
                };
            return View("OwnerDashboard", model);//todo zamienic na wywolywanie akcji do kazdej listy w widoku a nie budowanie modelu zlozonego z list tutaj
        }

        public PartialViewResult OwnerTournamentsList()
        {
            var model = GetTournamentsCreatedByUser(GetCurrentUserName(), DEFAULT_TOURNAMENT_COUNT);
            return PartialView("_TournamentList", model);
        }

        private TounamentSingleListViewModel GetTournamentsCreatedByUser(string user, int count)
        {
            var model = CreateTounamentSingleListViewModelWithSortedItems(
                                        tournamentRepository.GetTournamentsByOwnerUser(user),
                                        count,
                                        "Tournaments created by you",
                                        manageMode :true,
                                        showSubscriptionStatus:false);
           return model;
        }
       
        private TounamentSingleListViewModel CreateTounamentSingleListViewModelWithSortedItems(IEnumerable<Tournament> tournaments, int defaultTournamentCount, string header, bool manageMode = false, bool showSubscriptionStatus = false, bool nowPlayingMode = false)
        {
            var tournamentsDefaultCount = tournaments.Take(defaultTournamentCount).ToList();
            var dateTime = DateTime.Now;
            var future = tournamentsDefaultCount.Where(t => t.PlannedStartDate >= dateTime || t.TournamentStatus == TournamentStatus.Started).OrderBy(t => t.PlannedStartDate).ToList();
            var past = tournamentsDefaultCount.Where(t => t.PlannedStartDate < dateTime && t.TournamentStatus != TournamentStatus.Started).OrderByDescending(t => t.PlannedStartDate).ToList();
            return new TounamentSingleListViewModel
                {
                    Tournaments = future,
                    TournamentsPast = past,
                    Header = header,
                    ManageMode = manageMode,
                    ShowSubscriptionStatus = showSubscriptionStatus,
                    NowPlayingMode = nowPlayingMode,
                    IsShortList = defaultTournamentCount == DEFAULT_TOURNAMENT_COUNT
                };
        }

      

        public ActionResult PlayerTournaments()
        {
            //todo to not show toutn. which are broken
            var model = new TounamentListViewModel
            {
                TournamentsList = new List<TounamentSingleListViewModel>
                {
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetCurrentlyPlayingByUser(GetCurrentUserName()),
                    MAX_TOURNAMENT_COUNT, 
                    "Now playing", nowPlayingMode : true),
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetTournamentsToPlayByUser(GetCurrentUserName()),
                    MAX_TOURNAMENT_COUNT, 
                    "Playing soon"),
                    CreateTounamentSingleListViewModelWithSortedItems(
                    tournamentRepository.GetTournamentsAlreadyPlayedByUser(GetCurrentUserName()),
                    MAX_TOURNAMENT_COUNT, 
                    "Already played")
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
        public ActionResult Finish(long id)
        {
            var operationStatus = this.tournamentManager.Finish(id);
            if (operationStatus.Ok)
                notificationService.DisplaySuccess("Tournament finished");
            else
                notificationService.DisplayError("Some errors during tournament finish. \r\n Details: {0}",
                    operationStatus.ErrorMessage);
            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return Redirect(Request.UrlReferrer.ToString());
        }
    }

    //public class FixMissingIdParameterForBreadcrumbAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (filterContext.RouteData.Values["id"] != null)
    //        {
    //            filterContext.ActionParameters["tournamentId"] = long.Parse(filterContext.RouteData.Values["id"].ToString());
    //        }
    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}