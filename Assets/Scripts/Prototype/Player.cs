using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string species;
    public string playerClass;
    public StatBlock stats = new StatBlock();
    public List<string> skills = new List<string>();
    public InventoryDatabase inventory = new InventoryDatabase { gold = 0, items = new List<string>() };
    public int xp = 0;
    public int level = 1;
    public string zone;
    public bool godMode = false;
}
