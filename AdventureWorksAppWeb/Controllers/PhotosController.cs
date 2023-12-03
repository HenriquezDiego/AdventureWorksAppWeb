using AdventureWorksAppWeb.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdventureWorksAppWeb.Controllers
{
    [Authorize]
    public class PhotosController : Controller
    {
        private readonly AdventureWorksDb _adventureWorksDb = new AdventureWorksDb();

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
            var user = _adventureWorksDb.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            photo.UserId = user.UserId;
            photo.User = null;
            photo.UploadDate = DateTime.Now;
            _adventureWorksDb.Photos.Add(photo);
            _adventureWorksDb.SaveChanges();

            return RedirectToAction("Index", "Home"); // Redirigir a la página principal después de la subida
        }

        private string GuardarFoto(HttpPostedFileBase file)
        {
            // Lógica para guardar la foto en el servidor
            // Puedes usar el nombre original del archivo o generar un nombre único
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Files/"), fileName);
            file.SaveAs(path);

            // Devolver la ruta relativa de la foto para almacenar en la base de datos
            return "/Files/" + fileName;
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            var photo = _adventureWorksDb.Photos.Include(p=>p.Comments).FirstOrDefault(p=>p.PhotoId == id);
            if (photo == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(int id,string content)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index", "Home");
            var newComment = new Comment();
            var userName = HttpContext.User.Identity.Name;
            var user = _adventureWorksDb.Users.FirstOrDefault(u => u.UserName == userName);
            newComment.UserId = user.UserId;
            newComment.PhotoId = id;
            newComment.Content = content;
            _adventureWorksDb.Comments.Add(newComment);
            await _adventureWorksDb.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var photo = _adventureWorksDb.Photos.Find(id);

            if (photo == null)
            {
                return HttpNotFound();
            }
            _adventureWorksDb.Photos.Remove(photo);
            _adventureWorksDb.SaveChanges();
            EliminarFotoDelServidor(photo.ImagePath);
            return RedirectToAction("MyGaleria", "Home");
        }

        private void EliminarFotoDelServidor(string imagePath)
        {
            var path = Server.MapPath(imagePath);
            System.IO.File.Delete(path);
        }
    }
}
