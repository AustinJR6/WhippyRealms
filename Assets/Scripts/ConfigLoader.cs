using System.IO;
using UnityEngine;

[System.Serializable]
public class OpenAIConfig
{
    public string apiKey;
}

public class ConfigLoader : MonoBehaviour
{
    public static string LoadOpenAIKey()
    {
        string path = Path.Combine(Application.dataPath, "Resources/Secrets/openai_config.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            OpenAIConfig config = JsonUtility.FromJson<OpenAIConfig>(json);
            return config.apiKey;
        }
        else
        {
            Debug.LogError("OpenAI config file not found.");
            return null;
        }
    }
}
