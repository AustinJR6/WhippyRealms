using UnityEngine;

/// <summary>
/// Handles saving and loading of persistent game data. This class can be
/// expanded to serialize world state, player data, etc.
/// </summary>
public class SaveSystem : MonoBehaviour
{
    private const string SaveKey = "WR_Save";

    public void NewSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }

    public void SaveGame(string jsonData)
    {
        PlayerPrefs.SetString(SaveKey, jsonData);
        PlayerPrefs.Save();
    }

    public string LoadGame()
    {
        return PlayerPrefs.GetString(SaveKey, string.Empty);
    }
}
