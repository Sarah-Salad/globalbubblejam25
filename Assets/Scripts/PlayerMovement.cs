using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    private Rigidbody2D _rigidbody2d;
    private Vector2 _moveInput;
    private Vector2 _moveInputSmoothed;
    private Vector2 _moveInputVelocity;

    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        SetPlayerVelocity();
        RotatePlayer();
    }

    private void SetPlayerVelocity()
    {
        _moveInputSmoothed = Vector2.SmoothDamp(
                    _moveInputSmoothed,
                    _moveInput,
                    ref _moveInputVelocity,
                    0.1f);

        _rigidbody2d.linearVelocity = _moveInputSmoothed * moveSpeed;
    }
    private void RotatePlayer()
    {
        if (_moveInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _moveInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            _rigidbody2d.MoveRotation(rotation);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }
}
