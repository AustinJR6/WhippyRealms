using System.Linq;
using UnityEngine;

public class DevConsole : MonoBehaviour
{
    public PrototypeGame game;

    public void Execute(string command)
    {
        var args = command.Split(' ');
        if (args.Length == 0) return;
        switch (args[0])
        {
            case "spawn":
                if (args.Length > 1) game.SendMessage("SpawnEnemy", args[1]);
                break;
            case "give":
                if (args.Length > 1) game.player.inventory.items.Add(args[1]);
                break;
            case "teleport":
                if (args.Length > 1) game.SendMessage("EnterZone", args[1]);
                break;
            case "godmode":
                if (args.Length > 1) game.player.godMode = args[1] == "on";
                break;
            default:
                Debug.Log("Unknown command");
                break;
        }
    }
}
