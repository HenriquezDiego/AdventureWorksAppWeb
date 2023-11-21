using AdventureWorksAppWeb.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AdventureWorksAppWeb.Controllers
{
    public class PhotosController : Controller
    {
        private AdventureWorksDb db = new AdventureWorksDb();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Photo photo, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                // Guardar la foto en el servidor y actualizar la propiedad ImagePath
                string imagePath = GuardarFoto(file);
                photo.ImagePath = imagePath;

                // Aquí puedes guardar la instancia de Photo en la base de datos
                // dbContext.Photos.Add(photo);
                // dbContext.SaveChanges


                return RedirectToAction("Index", "Home"); // Redirigir a la página principal después de la subida
            }

            // Si hay errores, volver a la vista de subida
            return RedirectToAction("Index", "Home");
        }

        private string GuardarFoto(HttpPostedFileBase file)
        {
            // Lógica para guardar la foto en el servidor
            // Puedes usar el nombre original del archivo o generar un nombre único
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/Files/"), fileName);
            file.SaveAs(path);

            // Devolver la ruta relativa de la foto para almacenar en la base de datos
            return "/Files/" + fileName;
        }
    }
}
