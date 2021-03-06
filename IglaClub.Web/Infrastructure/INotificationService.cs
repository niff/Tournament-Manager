﻿namespace IglaClub.Web.Infrastructure
{
    interface INotificationService
    {
        void DisplayMessage(string message, NotificationType type = NotificationType.Info);
        void DisplayError(string message, params object[] parameters);
        void DisplaySuccess(string message, params object[] parameters);
    }
}
