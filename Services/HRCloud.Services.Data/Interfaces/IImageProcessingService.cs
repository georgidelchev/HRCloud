using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace HRCloud.Services.Data.Interfaces
{
    public interface IImageProcessingService
    {
        Task<string> SaveImageLocallyAsync(IFormFile image, string directory, int width = 200, int height = 120);
    }
}
