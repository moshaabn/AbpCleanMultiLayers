using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MoShaabn.CleanArch.Integrations.Storage;

public class StorageService : IStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StorageService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> Upload(IFormFile file)
    {
        string url = "";
        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName);
            var filename = StorageExtensions.GetNewName() + extension;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/", filename);
            var stream = new FileStream(path, FileMode.Create);
            var result = file.CopyToAsync(stream);
            result.Wait();
            if (result.IsCompletedSuccessfully)
            {
                url = "/uploads/" + filename;
                stream.Close();
            }
            else
            {
                url = "/uploads/default.jpg";
            }
        }
        else
        {
            url = "/uploads/default.jpg";
        }
        url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}{url}";
        return url;
    }

    public async Task<string> Upload(IFormFile file, string location)
    {
        string url;
        if (file != null)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", location);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var extension = Path.GetExtension(file.FileName);
            var filename = StorageExtensions.GetNewName() + extension;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/" + location, filename);
            var stream = new FileStream(path, FileMode.Create);
            var result = file.CopyToAsync(stream);
            result.Wait();
            if (result.IsCompletedSuccessfully)
            {
                url = StorageExtensions.GetBaseUrl(true) + "/uploads/" + location + "/" + filename;
                stream.Close();
            }
            else
            {
                url = StorageExtensions.GetBaseUrl(true) + "/uploads/default.jpg";
            }
        }
        else
        {
            url = StorageExtensions.GetBaseUrl(true) + "/uploads/default.jpg";
        }
        return url;
    }

    public async Task<bool> Delete(string fileUrl)
    {
        try
        {
            var hostUrl = StorageExtensions.GetBaseUrl(true);
            var imagePath = fileUrl;
            var directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/" + imagePath);
            if (!File.Exists(directoryPath)) return false;
            File.Delete(directoryPath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
