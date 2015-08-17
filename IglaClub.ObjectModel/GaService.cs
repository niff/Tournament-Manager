using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace IglaClub.ObjectModel
{
    public static class GaService
    {
        public static string GetGAData()
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

            DataResource.RealtimeResource.GetRequest request1 = service.Data.Realtime.Get("ga:56789", "rt:activeUsers");

 
            GaData data = request.Execute();

            RealtimeData realtimeData = request1.Execute();


            //return data.TotalsForAllResults["ga:sessions"];
            return realtimeData.TotalsForAllResults["rt:activeUsers"];
        }

    }
}
