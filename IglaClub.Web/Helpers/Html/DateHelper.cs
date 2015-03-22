using System;

namespace IglaClub.Web.Helpers.Html
{
    public static class DateHelper
    {
        public static string DaysLeftToDate(DateTime inputDate)
        {
            const int weekDays = 7;
            int span = (inputDate - DateTime.Today).Days;
            if (span <= weekDays && span >= -weekDays)
            {
                if (span == 0)
                {
                    return "today";
                }

                string dateIsFuture = "";
                string dateIsPast = "";
                if (span >= 1)
                {
                    dateIsFuture = "in ";
                }
                else if (span <= -1)
                {
                    dateIsPast = " ago";
                }
                var daysPlural = span != 1 ? "s" : String.Empty;
                var daysCount = Math.Abs(span);

                return String.Format("{3}{0} day{1}{2}", 
                    daysCount, daysPlural, dateIsPast, dateIsFuture);
            }
            return inputDate.ToLongDateString();
        }
    }
}