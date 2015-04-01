using System;
using System.Web;

namespace IglaClub.Web.Helpers.Html
{
    public static class LinkHelper
    {
        public static IHtmlString GoBackLink(string url, string label)
        {
            var link = String.Format(@"<div><a href='{0}'><i class='glyphicon glyphicon-arrow-left right-side-margin'></i>{1}</a></div>", url, label);
            return new HtmlString(link);
        }
    }
}