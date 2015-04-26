using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.Web.Models;

namespace IglaClub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IglaClubDbContext db = new IglaClubDbContext();
        private readonly HelpRepository helpRepository;

        public HomeController()
        {
            helpRepository = new HelpRepository(db);
        }
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //return RedirectToAction("Index","Tournament");
            if (Request.IsAuthenticated)
               return RedirectToAction("Index", "Tournament");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult OrganizerFaq()
        {
            var model = helpRepository.GetAll<HelpEntry>();
            return View(model);
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
public ActionResult TermsAndConditions()
        {
            return View();
        }


    }
}
