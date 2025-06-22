using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Very lightweight combat resolution for demo purposes.
/// </summary>
public class CombatManager : MonoBehaviour
{
    public Text logText;
    private PlayerState player;

    public void Initialize(PlayerState state)
    {
        player = state;
    }

    public void UseSkill(string skillName)
    {
        Append($"You use {skillName}!");
    }

    private void Append(string msg)
    {
        if (logText != null)
            logText.text += msg + "\n";
        else
            Debug.Log(msg);
    }
}
