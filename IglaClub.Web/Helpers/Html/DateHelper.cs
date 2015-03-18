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

                string formattedDate = String.Format("{0} day{1}", span, span != 1 ? "s" : String.Empty);
                if (span > 0)
                    return "in " + formattedDate;
                if (span < 0)
                {
                    return formattedDate + " ago";
                }
            }
            return inputDate.ToLongDateString();
        }
    }
}