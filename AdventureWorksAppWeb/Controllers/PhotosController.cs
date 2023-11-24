using AdventureWorksAppWeb.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AdventureWorksAppWeb.Controllers
{
    public class PhotosController : Controller
    {
        private AdventureWorksDb dbContext = new AdventureWorksDb();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Photo photo, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid || file == null || file.ContentLength <= 0)
                return RedirectToAction("Index", "Home");
            // Guardar la foto en el servidor y actualizar la propiedad ImagePath
            var imagePath = GuardarFoto(file);
            photo.ImagePath = imagePath;
            var user = dbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            photo.UserId = user.UserId;
            photo.User = null;
            photo.UploadDate = DateTime.Now;
            dbContext.Photos.Add(photo);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home"); // Redirigir a la página principal después de la subida
        }

        private string GuardarFoto(HttpPostedFileBase file)
        {
            // Lógica para guardar la foto en el servidor
            // Puedes usar el nombre original del archivo o generar un nombre único
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/Files/"), fileName);
            file.SaveAs(path);

            // Devolver la ruta relativa de la foto para almacenar en la base de datos
            return "/Files/" + fileName;
        }
    }
}
