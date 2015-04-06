using System.Linq;
using System.Web.Mvc;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Infrastructure;
using IglaClub.Web.Models;
using IglaClub.Web.Models.ViewModels.Clubs;

namespace IglaClub.Web.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly ClubRepository clubRepository;
        private readonly UserRepository userRepository;
        private readonly NotificationService notificationService;


        public ClubsController()
        {
            clubRepository = new ClubRepository(db);
            userRepository = new UserRepository(db);
            notificationService = new NotificationService(TempData);
        }
        //
        // GET: /Clubs/

        public ActionResult Index()
        {
            var user = this.userRepository.GetUserByLogin(User.Identity.Name);
            var clubs = this.db.Clubs;
            var clubsListViewModel = new ClubsIndexViewModel
            {
                ClubsWithSubscribedUser = clubs.Where(c=>c.ClubUsers.Any(cu=>cu.User.Id == user.Id)).ToList(),
                ClubsWithNotSubscribedUser = clubs.Where(c=>c.ClubUsers.All(cu=>cu.User.Id != user.Id)).ToList(),
                User = user
            };
            return View(clubsListViewModel);
        }

        //
        // GET: /Clubs/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }


    }
}
