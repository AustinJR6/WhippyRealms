using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates HUD text elements based on the current player state.
/// </summary>
public class UIManager : MonoBehaviour
{
    public Text hpText;
    public Text xpText;
    public Text levelText;
    public Text zoneText;
    public Text questText;

    public void UpdateHUD(PlayerState state)
    {
        if (state == null) return;

        if (hpText != null) hpText.text = $"HP: {state.hp}";
        if (xpText != null) xpText.text = $"XP: {state.xp}";
        if (levelText != null) levelText.text = $"Level: {state.level}";
        if (zoneText != null) zoneText.text = $"Zone: {state.zone}";
        if (questText != null)
        {
            // Display first active quest if available
            questText.text = state.inventory != null && state.inventory.Length > 0 ? $"Quest: {state.inventory[0]}" : "Quest: None";
        }
    }
}
