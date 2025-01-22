using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rigidbody2d;
    private Vector2 moveInput;

    private InputAction _moveAction;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        moveInput = _moveAction.ReadValue<Vector2>();

        moveInput.Normalize();

        rigidbody2d.linearVelocity = moveInput * moveSpeed;
    }
}
