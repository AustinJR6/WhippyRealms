using UnityEngine;

/// <summary>
/// Simple dialogue system that can display lines of dialogue and track basic
/// conversational state. This is where AI-generated dialogue can be plugged in.
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    public void Say(string speaker, string line)
    {
        Debug.Log($"{speaker}: {line}");
    }
}
