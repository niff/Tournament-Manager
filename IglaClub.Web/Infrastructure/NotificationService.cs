using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IglaClub.Web.Infrastructure
{
    public class NotificationService : INotificationService
    {
        private readonly TempDataDictionary TempData;
        public NotificationService(TempDataDictionary tempData)
        {
            this.TempData = tempData;
        }
        public void DisplayMessage(string message)
        {
            this.TempData["Message"] = message;
        }
    }
}