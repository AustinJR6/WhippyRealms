using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The central coordinator for the game. Holds references to persistent systems
/// such as the SaveSystem, SceneLoader, and manages global state. This component
/// should live on a GameObject that persists across scenes (use DontDestroyOnLoad).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    public SaveSystem saveSystem;
    public SceneLoader sceneLoader;

    private readonly List<GameObject> persistentObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Register an object that should persist across scenes.
    /// </summary>
    public void RegisterPersistentObject(GameObject obj)
    {
        if (!persistentObjects.Contains(obj))
        {
            DontDestroyOnLoad(obj);
            persistentObjects.Add(obj);
        }
    }

    /// <summary>
    /// Example entry point that could be called when starting a new game.
    /// </summary>
    public void StartNewGame(string firstScene)
    {
        saveSystem.NewSave();
        sceneLoader.LoadScene(firstScene);
    }
}
