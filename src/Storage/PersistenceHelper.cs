using System.IO;
using BepInEx;
using Newtonsoft.Json;

namespace BagOfTricks.src.Storage
{
    internal static class PersistenceHelper
    {
        private readonly static string filePath = Path.Combine(Paths.PluginPath, $"{Meta.RelPaths.SETTINGS_DIR}\\PersistentData.json");

        internal static void Store(object @object)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();

            string jsonString = JsonConvert.SerializeObject(@object, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }

        internal static T Retreive<T>()
        {
            if (!File.Exists(filePath))
                return default;

            string fileContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(fileContent);
        }
    }
}
