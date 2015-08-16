using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
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
            GetGAData();
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

        private void GetGAData()
        {
            // path of my .p12 file
            string keyFilePath = @"C:\Users\Justyna\Desktop\IglaClub-key.p12";
            string serviceAccountEmail = "682454275672-31klq8q3d73q63iasr9p4v9cr8fn908o@developer.gserviceaccount.com";
            string websiteCode = "106889360";

            string keyPassword = "notasecret";

            AnalyticsService service = null;

            var byt = System.IO.File.ReadAllBytes(keyFilePath);
            var certificate = new X509Certificate2(byt, keyPassword, X509KeyStorageFlags.Exportable); 
            var scopes =
                    new string[] {     
             AnalyticsService.Scope.Analytics,              // view and manage your analytics data    
             AnalyticsService.Scope.AnalyticsEdit,          // edit management actives    
             AnalyticsService.Scope.AnalyticsManageUsers,   // manage users    
             AnalyticsService.Scope.AnalyticsReadonly};     // View analytics data    


            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));


            service = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            DataResource.GaResource.GetRequest request = service.Data.Ga.Get(
                       "ga:" + websiteCode,
                       DateTime.Today.AddDays(-15).ToString("yyyy-MM-dd"),
                       DateTime.Today.ToString("yyyy-MM-dd"),
                       "ga:sessions,ga:users");

            //request.Dimensions = "ga:hits";
            // is it possible to add custom variable here
            // return exception bcz no such dimension which named: "CustomVar1"
            // request.Dimensions = "ga:CustomVar1";  

            var data = request.Execute();
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
