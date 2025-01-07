using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoShaabn.CleanArch.Integrations.Storage;
public static class StorageExtensions
{
    public static string GetBaseUrl(bool secure = false)
    {
        //return null;
        var context = new HttpContextAccessor();
        var request = context.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host.Value}";
        return secure ? baseUrl.Replace("http://", "https://") : baseUrl;
    }

    public static string GetNewName()
    {
        return Guid.NewGuid().ToString().Replace("-", string.Empty);
    }

    public static string PrependBaseUrl(this string path)
    {
        //return
        //    GetBaseUrl()
        //    + (path.StartsWith('/') ? "" : "/")
        //    + path;

        return null;

    }


    public static List<string> StringToListUrls(string images)
    {
        var urls = images.Split(',').ToList();

        // add base url to each url
        for (var i = 0; i < urls.Count; i++)
        {
            urls[i] = $"{GetBaseUrl(true)}{urls[i]}";
        }

        return urls;
    }

    public static string ListToStringUrls(string images)
    {
        // string "a,b,c" to string "https://a,https://b,https://c" or "a" to "https://a"
        var urls = new List<string>();

        // split string to array
        var imagesArray = images.Split(',');

        // add base url to each url
        for (var i = 0; i < imagesArray.Length; i++)
        {
            urls.Add($"{GetBaseUrl(true)}{imagesArray[i]}");
        }

        // convert list to string
        var result = string.Join(',', urls);

        return result;
    }
    //get last param in url
    public static string ExtractFileName(string url)
    {
        // Create a Uri object from the URL
        var uri = new Uri(url);

        // Get the absolute path of the URL
        string path = uri.AbsolutePath;

        // Extract the filename from the path
        // Remove leading '/' and split by '/' to get the last segment
        return path.TrimStart('/').Split('/').Last();
    }
}