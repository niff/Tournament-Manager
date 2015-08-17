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

        public ActionResult Start()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Start", "Home");
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

        //private async Task Run()
        //{
        //    // Create the service.
        //    var service = new DiscoveryService(new BaseClientService.Initializer
        //    {
        //        ApplicationName = "Igla Club",
        //        ApiKey = "[YOUR_API_KEY_HERE]",
        //    });

        //    var an = new AnalyticsService(new BaseClientService.Initializer
        //        {
        //            ApplicationName = "Igla Club",
        //            ApiKey = "[YOUR_API_KEY_HERE]"
                    
        //        });

        //    // Run the request.
        //    Console.WriteLine("Executing a list request...");
        //    var result = await an.Apis.List().ExecuteAsync();

        //    // Display the results.
        //    if (result.Items != null)
        //    {
        //        foreach (DirectoryList.ItemsData api in result.Items)
        //        {
        //            Console.WriteLine(api.Id + " - " + api.Title);
        //        }
        //    }
        //}


    }
}
