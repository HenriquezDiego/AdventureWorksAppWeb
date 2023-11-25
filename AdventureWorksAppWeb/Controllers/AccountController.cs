using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AdventureWorksAppWeb.Models;
using System.Web.Mvc;
using System.Web.Security;
using AdventureWorksAppWeb.Models.Account;

namespace AdventureWorksAppWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly AdventureWorksDb _appDbContext;

        public AccountController()
        {
            _appDbContext = new AdventureWorksDb();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _appDbContext.Users.FirstOrDefault(u => u.UserName == model.UserName);
            if(user == null) return View(model);
            if (model.UserName == user.UserName && HashPassword(model.Password) == user.Password)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos.");
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var users = _appDbContext.Users.Where(u=>u.UserName == model.UserName).ToList();
            if (users.Any())
            {
                ViewBag.Error = "El nombre de usuario ya existe";
                return View("Register");
            }
            var passwordHash = HashPassword(model.Password);
            var user = new User { UserName = model.UserName, Password = passwordHash};
            // Guardar el usuario en la base de datos
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            FormsAuthentication.SetAuthCookie(model.UserName, false);
            return RedirectToAction("Index", "Home");

        }

        private string HashPassword(string password)
        {
            // Implementa aquí la lógica de hash que desees (por ejemplo, BCrypt, SHA256, etc.)
            // Aquí se utiliza un ejemplo simple con SHA256 (no recomendado para producción)
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}