using System.IO;
using UnityEngine;

public static class DataLoader
{
    public static T LoadJson<T>(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"Missing data file: {path}");
            return default;
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}
