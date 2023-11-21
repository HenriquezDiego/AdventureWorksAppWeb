using System.Linq;
using System.Web.Mvc;
using AdventureWorksAppWeb.Models;

namespace AdventureWorksAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdventureWorksDb _context;

        public HomeController()
        {
            _context = new AdventureWorksDb(); 
        }

        public ActionResult Index()
        {
            var photos = _context.Photos.ToList();
            return View(photos);
        }
    }
}