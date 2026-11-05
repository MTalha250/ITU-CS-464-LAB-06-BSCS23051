using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveForce = 10f;
    Rigidbody rb;
    InputAction moveAction;
    Vector3 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        input = Vector3.ClampMagnitude(new Vector3(move.x, 0f, move.y), 1f);
    }

    void FixedUpdate() => rb.AddForce(input * moveForce);
}
