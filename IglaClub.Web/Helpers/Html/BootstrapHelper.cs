using System;
using System.Web;

namespace IglaClub.Web.Helpers.Html
{
    public static class BootstrapHelper
    {
        public static IHtmlString Popover(string content, string icon)
        {
            var popover = String.Format("<a role=\"button\" data-toggle=\"popover\"  class=\"popovers\" tabindex=\"0\"   data-content=\"{0}\" ><i class=\"glyphicon glyphicon-{1}\"></i></a>", content, icon);
            return new HtmlString(popover);
        }
    }
}