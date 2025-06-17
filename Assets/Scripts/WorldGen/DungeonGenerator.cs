using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates a simple procedural dungeon using a seeded random walk. Rooms are
/// connected with corridors and a goal room is spawned at the end.
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public GameObject enemyPlaceholder;
    public GameObject lootPlaceholder;
    public GameObject portalPrefab;

    public int seed = 0;
    public int size = 5;

    private readonly List<Vector3> roomPositions = new List<Vector3>();

    /// <summary>
    /// Generates the dungeon layout. If seed is zero a random seed is chosen.
    /// </summary>
    public void Generate()
    {
        if (seed == 0)
            seed = System.DateTime.Now.Millisecond;

        Random.InitState(seed);
        int roomCount = Mathf.Clamp(size, 3, 7);
        Vector3 currentPos = Vector3.zero;
        roomPositions.Add(currentPos);

        // Random walk to place rooms
        for (int i = 1; i < roomCount; i++)
        {
            Vector3 dir = GetRandomDirection();
            currentPos += dir * 10f;
            roomPositions.Add(currentPos);
        }

        // Instantiate rooms and corridors
        for (int i = 0; i < roomPositions.Count; i++)
        {
            var room = Instantiate(roomPrefab, roomPositions[i], Quaternion.identity, transform);
            room.name = $"Room_{i}";

            SpawnContents(room.transform);

            if (i > 0)
            {
                Vector3 prev = roomPositions[i - 1];
                CreateCorridor(prev, roomPositions[i]);
            }
        }

        // Goal room object/boss
        var goal = new GameObject("GoalObject");
        goal.transform.position = roomPositions[roomPositions.Count - 1];
        goal.transform.SetParent(transform);

        // Portal back to village at entrance
        if (portalPrefab != null)
        {
            Instantiate(portalPrefab, roomPositions[0] + Vector3.back * 2, Quaternion.identity, transform);
        }
    }

    private void SpawnContents(Transform room)
    {
        if (enemyPlaceholder != null && Random.value > 0.5f)
            Instantiate(enemyPlaceholder, room.position + Vector3.right * 2, Quaternion.identity, room);
        if (lootPlaceholder != null && Random.value > 0.5f)
            Instantiate(lootPlaceholder, room.position + Vector3.left * 2, Quaternion.identity, room);
    }

    private void CreateCorridor(Vector3 from, Vector3 to)
    {
        if (corridorPrefab == null) return;
        Vector3 mid = (from + to) / 2f;
        Quaternion rot = Quaternion.LookRotation(to - from);
        float length = Vector3.Distance(from, to);
        var corridor = Instantiate(corridorPrefab, mid, rot, transform);
        corridor.transform.localScale = new Vector3(1, 1, length);
    }

    private Vector3 GetRandomDirection()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: return Vector3.forward;
            case 1: return Vector3.back;
            case 2: return Vector3.left;
            default: return Vector3.right;
        }
    }
}
