using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MoShaabn.CleanArch.Integrations.Storage;

public interface IStorageService
{
    Task<string> Upload(IFormFile file);
    Task<string> Upload(IFormFile file, string location);
    Task<bool> Delete(string fileUrl);
}
