using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Parses player commands from an InputField and routes them to systems.
/// </summary>
public class InputHandler : MonoBehaviour
{
    public InputField inputField;
    public GameManager gameManager;
    public UIManager uiManager;

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
            var args = ExtractArgs(command);
            if (args.Count > 0)
            {
                string npc = args[0];
                string msg = args.Count > 1 ? args[1] : string.Empty;
                if (gameManager.npcManager != null && gameManager.npcManager.IsAI(npc))
                {
                    string loc = gameManager.playerState?.zone;
                    gameManager.npcManager.TalkTo(npc, msg, loc, string.Empty);
                }
                else
                {
                    gameManager.dialogueManager?.StartDialogue(npc);
                }
            }
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
        else if (command == "help" || command == "commands" || command == "?")
        {
            string helpText =
                "Available Commands:\n" +
                "- talkTo(\"NPC\")\n" +
                "- travelTo(\"Zone\")\n" +
                "- attack(\"Enemy\")\n" +
                "- useSkill(\"SkillName\")\n" +
                "- useItem(\"ItemName\")\n" +
                "- openInventory()\n" +
                "- openQuests()\n" +
                "- showStats()\n" +
                "- help()";
            gameManager.logManager?.Log(helpText);
        }

        uiManager?.UpdateHUD(gameManager.playerState);
    }

    private string ExtractArg(string cmd)
    {
        int start = cmd.IndexOf('"');
        int end = cmd.LastIndexOf('"');
        if (start >= 0 && end > start)
            return cmd.Substring(start + 1, end - start - 1);
        return string.Empty;
    }

    private List<string> ExtractArgs(string cmd)
    {
        var args = new List<string>();
        int idx = 0;
        while (idx < cmd.Length)
        {
            int start = cmd.IndexOf('"', idx);
            if (start < 0) break;
            int end = cmd.IndexOf('"', start + 1);
            if (end < 0) break;
            args.Add(cmd.Substring(start + 1, end - start - 1));
            idx = end + 1;
        }
        return args;
    }
}
