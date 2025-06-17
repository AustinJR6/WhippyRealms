using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for loading and unloading Unity scenes. Use this for transitioning
/// between world zones, dungeons, etc.
/// </summary>
public class SceneLoader : MonoBehaviour
{
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
}
