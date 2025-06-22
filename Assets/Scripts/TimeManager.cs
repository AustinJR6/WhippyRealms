using UnityEngine;

/// <summary>
/// Tracks in-game time for future timed events or narrative triggers.
/// </summary>
public class TimeManager : MonoBehaviour
{
    public int daysPassed;
    public int hours;
    public int minutes;

    /// <summary>
    /// Advance time for traveling between zones (+6 hours).
    /// </summary>
    public void AdvanceTravel()
    {
        AddMinutes(6 * 60);
    }

    /// <summary>
    /// Advance time for resting (+8 hours).
    /// </summary>
    public void AdvanceRest()
    {
        AddMinutes(8 * 60);
    }

    /// <summary>
    /// Advance time when clearing a dungeon (+12 hours).
    /// </summary>
    public void AdvanceDungeonClear()
    {
        AddMinutes(12 * 60);
    }

    private void AddMinutes(int mins)
    {
        minutes += mins;
        while (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }
        while (hours >= 24)
        {
            hours -= 24;
            daysPassed++;
        }
    }

    /// <summary>
    /// Returns the current in-game time formatted as a string.
    /// </summary>
    public string GetCurrentTime()
    {
        return $"Day {daysPassed}, {hours:D2}:{minutes:D2}";
    }
}
