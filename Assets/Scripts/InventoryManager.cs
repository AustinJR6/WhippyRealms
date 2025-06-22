using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles simple inventory actions for the demo.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public Text logText;
    public LogManager logManager;
    private PlayerState player;

    public void Initialize(PlayerState state)
    {
        player = state;
    }

    public void OpenInventory()
    {
        foreach (var item in player.inventory)
            Append($"Inventory Item: {item}");
    }

    public void UseItem(string item)
    {
        Append($"You use {item}");
    }

    private void Append(string msg)
    {
        if (logManager != null)
        {
            logManager.Log(msg);
            return;
        }

        if (logText != null)
            logText.text += msg + "\n";
        else
            Debug.Log(msg);
    }
}
