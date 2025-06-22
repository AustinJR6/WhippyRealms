using UnityEngine;

/// <summary>
/// Utility class for loading JSON files from Resources/Data.
/// </summary>
public static class JsonLoader
{
    public static T LoadJson<T>(string fileName)
    {
        string path = System.IO.Path.Combine("Data", System.IO.Path.GetFileNameWithoutExtension(fileName));
        TextAsset asset = Resources.Load<TextAsset>(path);
        if (asset == null)
        {
            Debug.LogError($"Failed to load {fileName} from Resources/Data");
            return default;
        }
        return JsonUtility.FromJson<T>(asset.text);
    }
}
