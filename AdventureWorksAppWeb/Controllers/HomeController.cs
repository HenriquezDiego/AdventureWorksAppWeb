using AdventureWorksAppWeb.Models;
using System.Linq;
using System.Web.Mvc;

namespace AdventureWorksAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdventureWorksDb _context;

        public HomeController()
        {
            _context = new AdventureWorksDb(); 
        }

        public ActionResult Index(string searchQuery)
        {
            var photos = _context.Photos.AsQueryable();
            if(string.IsNullOrEmpty(searchQuery)) return View(photos.ToList());
            photos = photos.Where(p => p.Title.Contains(searchQuery) || p.User.UserName.Contains(searchQuery));
            return View(photos.ToList());
        }


        [Authorize]
        public ActionResult MyGaleria()
        {
            var photos = _context.Photos.AsQueryable();
            var user = HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(user)) return View("Index");
            photos = photos.Where(p => p.User.UserName == user);
            return View(photos.ToList());
        }
    }
}