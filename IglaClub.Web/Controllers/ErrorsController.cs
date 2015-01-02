using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IglaClub.Web.Controllers
{
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/

        public ActionResult Error403()
        {
            ViewBag.ErrorMessage = "You have no access to that page.";
            ViewBag.ReffererUrl = Request.UrlReferrer;
            return View();
        }

        public ActionResult Error404()
        {
            ViewBag.ErrorMessage = "Item not found.";
            ViewBag.ReffererUrl = Request.UrlReferrer;
            return View();
        }
    }
}
