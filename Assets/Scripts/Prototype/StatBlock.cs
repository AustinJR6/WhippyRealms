using UnityEngine;

[System.Serializable]
public class StatBlock
{
    public int health;
    public int attack;
    public int magic;
    public int armor;

    public StatBlock Clone()
    {
        return (StatBlock)MemberwiseClone();
    }

    public void Add(StatBlock other)
    {
        health += other.health;
        attack += other.attack;
        magic += other.magic;
        armor += other.armor;
    }
}
