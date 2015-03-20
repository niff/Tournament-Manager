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
        public void DisplayMessage(string message, NotificationType type = NotificationType.Info)
        {
            this.TempData["Message"] = message;
            this.TempData["MessageType"] = type;
        }
    }

    public enum NotificationType
    {
        Success,
        Info,
        Warning,
        Danger
    }
}