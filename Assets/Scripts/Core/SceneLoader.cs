using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Information about a procedural scene request.
/// </summary>
public struct ProceduralSceneRequest
{
    public string zoneName;
    public int seed;
    public int size;
}

/// <summary>
/// Responsible for loading and unloading Unity scenes. Use this for transitioning
/// between world zones, dungeons, etc.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public WorldMemory worldMemory;

    /// <summary>
    /// Load a scene by name. Can be expanded to include loading screens or async
    /// operations.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Example of asynchronous scene loading with optional callback when complete.
    /// </summary>
    public void LoadSceneAsync(string sceneName, System.Action onComplete = null)
    {
        StartCoroutine(LoadAsyncRoutine(sceneName, onComplete));
    }

    private System.Collections.IEnumerator LoadAsyncRoutine(string sceneName, System.Action onComplete)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

    /// <summary>
    /// Load a procedural dungeon. If the zone was generated previously, the same
    /// seed is used. Otherwise a new entry is created in WorldMemory.
    /// </summary>
    public void LoadProceduralDungeon(ProceduralSceneRequest request)
    {
        StartCoroutine(LoadDungeonRoutine(request));
    }

    private System.Collections.IEnumerator LoadDungeonRoutine(ProceduralSceneRequest request)
    {
        // Load an empty scene to host the dungeon
        var op = SceneManager.LoadSceneAsync("EmptyDungeon");
        while (!op.isDone)
            yield return null;

        if (worldMemory == null)
            worldMemory = FindObjectOfType<WorldMemory>();

        if (worldMemory != null)
        {
            var info = worldMemory.GetZone(request.zoneName);
            if (info == null)
            {
                worldMemory.RegisterZone(request.zoneName, request.seed, Vector3.zero);
                worldMemory.Save();
            }
            else
            {
                request.seed = info.seed;
            }
        }

        // Spawn a generator in the scene
        var generatorObj = new GameObject("DungeonGenerator");
        var generator = generatorObj.AddComponent<DungeonGenerator>();
        generator.seed = request.seed;
        generator.size = request.size;
        generator.Generate();
    }
}
