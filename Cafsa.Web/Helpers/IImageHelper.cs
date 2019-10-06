using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cafsa.Web.Helpers
{
    public interface IImageHelper
    {
        //este metodo devuelve la ruta donde quedo la imagen del servicio
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
