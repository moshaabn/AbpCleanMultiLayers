using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace OnlineStoreAbp.Helpers
{
    public static class JsonSeedHelper
    {
        public static List<T> LoadFromJson<T>(string filePath)
        {

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found.");
            }

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json)!;
        }
    }
}
