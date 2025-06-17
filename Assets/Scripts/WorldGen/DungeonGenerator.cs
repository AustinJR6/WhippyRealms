using UnityEngine;

/// <summary>
/// Handles procedural generation of dungeon layouts. Currently this only creates
/// placeholder geometry but can be expanded with rooms, corridors and theming.
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public int roomCount = 5;

    public void Generate()
    {
        for (int i = 0; i < roomCount; i++)
        {
            Instantiate(roomPrefab, transform.position + Vector3.right * i * 5, Quaternion.identity, transform);
        }
    }
}
