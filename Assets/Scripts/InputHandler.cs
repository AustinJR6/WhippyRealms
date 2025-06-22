using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Parses player commands from an InputField and routes them to systems.
/// </summary>
public class InputHandler : MonoBehaviour
{
    public InputField inputField;
    public GameManager gameManager;

    private void Start()
    {
        if (inputField != null)
            inputField.onEndEdit.AddListener(OnInputSubmitted);
    }

    private void OnInputSubmitted(string text)
    {
        if (!Input.GetKeyDown(KeyCode.Return))
            return;

        HandleCommand(text);
        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

    private void HandleCommand(string command)
    {
        if (command.StartsWith("travelTo"))
        {
            var arg = ExtractArg(command);
            gameManager.zoneManager?.TravelTo(arg);
        }
        else if (command.StartsWith("talkTo"))
        {
            var arg = ExtractArg(command);
            gameManager.dialogueManager?.StartDialogue(arg);
        }
        else if (command.StartsWith("useSkill"))
        {
            var arg = ExtractArg(command);
            gameManager.combatManager?.UseSkill(arg);
        }
        else if (command.StartsWith("useItem"))
        {
            var arg = ExtractArg(command);
            gameManager.inventoryManager?.UseItem(arg);
        }
        else if (command.StartsWith("openInventory"))
        {
            gameManager.inventoryManager?.OpenInventory();
        }
        else if (command.StartsWith("showStats"))
        {
            gameManager.zoneManager?.ShowStats();
        }
    }

    private string ExtractArg(string cmd)
    {
        int start = cmd.IndexOf('"');
        int end = cmd.LastIndexOf('"');
        if (start >= 0 && end > start)
            return cmd.Substring(start + 1, end - start - 1);
        return string.Empty;
    }
}
