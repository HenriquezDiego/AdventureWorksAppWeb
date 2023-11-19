using AdventureWorksAppWeb.DataAccess;
using System;
using System.IO;
using System.Web;

namespace AdventureWorksAppWeb.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly AppDbContext _context;

        public PhotoService(AppDbContext context)
        {
            _context = context;
        }


        public string UploadPhoto(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
            {
                // Manejar el caso en el que no se haya proporcionado un archivo
                return null;
            }

            // Generar un nombre de archivo único
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // Obtener la ruta física donde se guardará el archivo
            var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads/");
            var filePath = Path.Combine(uploadPath, fileName);

            // Guardar el archivo en el servidor
            file.SaveAs(filePath);

            // Devolver la ruta del archivo guardado (puedes almacenar esta ruta en la base de datos si es necesario)
            return filePath;
        }
    }
}