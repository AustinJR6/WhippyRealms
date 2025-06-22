using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Handles simple branching dialogue loaded from JSON.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public Text logText;

    private Dictionary<string, DialogueTree> trees = new Dictionary<string, DialogueTree>();

    public void Initialize(DialogueDatabase db)
    {
        trees.Clear();
        foreach (var t in db.dialogueTrees)
            trees[t.id] = t;
    }

    public void StartDialogue(string id)
    {
        if (trees.TryGetValue(id, out var tree))
        {
            foreach (var line in tree.lines)
                Append($"{tree.speaker}: {line.text}");
        }
        else
        {
            Append($"No dialogue found for {id}");
        }
    }

    public void DisplayLine(string speaker, string line)
    {
        Append($"{speaker}: {line}");
    }

    private void Append(string msg)
    {
        if (logText != null)
            logText.text += msg + "\n";
        else
            Debug.Log(msg);
    }
}

[System.Serializable]
public class DialogueLine
{
    public string text;
}

[System.Serializable]
public class DialogueTree
{
    public string id;
    public string speaker;
    public List<DialogueLine> lines;
}

[System.Serializable]
public class DialogueDatabase
{
    public List<DialogueTree> dialogueTrees;
}
