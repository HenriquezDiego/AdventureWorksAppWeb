using System.Web;

namespace AdventureWorksAppWeb.Services
{
    public interface IPhotoService
    {
        string UploadPhoto(HttpPostedFileBase file);
    }
}
