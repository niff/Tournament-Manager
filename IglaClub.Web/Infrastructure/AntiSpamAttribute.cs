using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;

namespace IglaClub.Web.Infrastructure
{
    public class AntiSpamAttribute : ActionFilterAttribute
    {
        public int DelayRequest = 10;
        public string ErrorMessage = "Excessive request attempts detected";
        public string RedirectURL;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var cache = filterContext.HttpContext.Cache;

            var originationInfo = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;

            originationInfo += request.UserAgent;

            var targetInfo = request.RawUrl + request.QueryString;

            // Generate a hash for your strings (this appends each of the bytes of the value into a single hashed string
            var hashValue = string.Join("", MD5.Create().
                ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));

            if (cache[hashValue] != null)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("ExcessiveRequests", ErrorMessage);
            }
            else
            {
                cache.Insert(hashValue, "", null, DateTime.Now.AddSeconds(DelayRequest), Cache.NoSlidingExpiration);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}