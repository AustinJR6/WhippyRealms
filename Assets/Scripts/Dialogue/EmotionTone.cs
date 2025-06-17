using UnityEngine;

/// <summary>
/// Represents a simple emotional tone that can influence dialogue delivery or
/// NPC reactions.
/// </summary>
[System.Serializable]
public class EmotionTone
{
    public enum ToneType { Neutral, Kind, Hostile, Sorrowful }
    public ToneType tone = ToneType.Neutral;

    public Color associatedColor = Color.white;
}
