using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;

namespace IglaClub.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserRepository userRepository;

        //
        // GET: /Admin/Users/
        public UsersController()
        {
            this.userRepository = new UserRepository(new IglaClubDbContext());
        }

        public ActionResult Index()
        {
            var users = this.userRepository.GetAllUsers();
            return View(users);
        }

    }
}
