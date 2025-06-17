using UnityEngine;

/// <summary>
/// Basic player controller handling movement and interaction. This is a starting
/// point and should be expanded with combat, inventory, etc.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0, vertical);
        controller.SimpleMove(move * moveSpeed);
    }
}
