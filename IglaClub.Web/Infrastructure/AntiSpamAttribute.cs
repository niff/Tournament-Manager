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
        /// <summary>
        /// Delay between two requests in seconds
        /// </summary>
        public int DelayRequest = 10;
        public string ErrorMessage = "Excessive request attempts detected. Please wait 10 seconds before next attempt.";
        public string RedirectURL;

        private static readonly object cacheLocker = new object();

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
            lock (cacheLocker)
            {
                if (cache[hashValue] != null)
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError("ExcessiveRequests", ErrorMessage);
                }
                else
                {
                    cache.Insert(hashValue, "", null, DateTime.Now.AddSeconds(DelayRequest), Cache.NoSlidingExpiration);
                }
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}