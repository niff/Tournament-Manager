using System;
using System.Web;

namespace IglaClub.Web.Helpers.Html
{
    public static class DateHelper
    {
        public static IHtmlString DaysLeftToDate(DateTime? inputDate)
        {
            if(inputDate == null) return new HtmlString("not set yet");
            DateTime date = (DateTime) inputDate;;
            var title = string.Format("{0} {1}", date.ToLongDateString(), date.ToShortTimeString());
            const int weekDays = 7;
            int span = (date.Date - DateTime.Today).Days;
            if (span <= weekDays && span >= -weekDays)
            {
                if (span == 0)
                {
                    return new HtmlString(String.Format("<span title=\"{0}\">today</span>", title));
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
                var daysCount = Math.Abs(span);
                var daysPlural = daysCount != 1 ? "s" : String.Empty;

                return new HtmlString(String.Format("<span title=\"{4}\">{3}{0} day{1}{2}</span>",
                    daysCount, daysPlural, dateIsPast, dateIsFuture, title));
            }
            return new HtmlString(String.Format("<span>{0}</span>", date.ToLongDateString()));
        }
    }
}