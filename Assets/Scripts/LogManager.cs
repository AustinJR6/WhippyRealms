using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Central logger that appends messages to a UI Text element with timestamps
/// and auto-scrolls the log area.
/// </summary>
public class LogManager : MonoBehaviour
{
    public Text logText;
    public ScrollRect scrollRect;

    public void Log(string message)
    {
        string entry = $"[{DateTime.Now:HH:mm}] {message}";
        if (logText != null)
        {
            logText.text += entry + "\n";
            Canvas.ForceUpdateCanvases();
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 0f;
        }
        else
        {
            Debug.Log(entry);
        }
    }
}
