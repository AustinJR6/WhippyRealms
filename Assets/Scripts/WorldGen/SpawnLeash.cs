using UnityEngine;

/// <summary>
/// Simple component to keep spawned creatures near a center point.
/// Moves the object back toward the center if it strays too far.
/// </summary>
public class SpawnLeash : MonoBehaviour
{
    public Transform center;
    public float maxDistance = 30f;

    private void Update()
    {
        if (center == null) return;
        Vector3 flat = transform.position - center.position;
        flat.y = 0f;
        if (flat.magnitude > maxDistance)
        {
            transform.position = center.position + flat.normalized * maxDistance;
        }
    }
}
