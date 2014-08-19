using System.Data.Entity;
using System.Web.Mvc;
using IglaClub.ObjectModel.Entities;
using IglaClub.ObjectModel.Repositories;
using IglaClub.Web.Models;

namespace IglaClub.Web.Areas.Admin.Controllers
{
    [Authorize]
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

        public ActionResult Edit(long id = 0)
        {
            var user = userRepository.Get<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User tournament)
        {
            if (!ModelState.IsValid) 
                return View(tournament);
            
            this.userRepository.Entry(tournament).State = EntityState.Modified;
            this.userRepository.SaveChanges();
            //return RedirectToAction("Manage", "Tournament", new { tournamentId = id });
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long id)
        {
            User user = this.userRepository.Get<User>(id);
            this.userRepository.Entry(user).State = EntityState.Deleted;
            this.userRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
