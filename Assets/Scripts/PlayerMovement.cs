using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    
    public float normalMoveSpeed;
    public float dashSpeed;
    public float dashLength = .5f, dashCooldown = 1f;
    public float rotationSpeed;
    private float _dashCounter;
    private float _dashCooldownCounter;
    private float _swimSpeed;
    private Rigidbody2D _rigidbody2d;
    private Vector2 _moveInput;
    private Vector2 _moveInputSmoothed;
    private Vector2 _moveInputVelocity;
    private bool _isDashing = false;
    private Animator _animator;
    
    void Start()
    {
        _swimSpeed = normalMoveSpeed;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate() 
    {
        SetPlayerVelocity();
        RotatePlayer();
        if (_isDashing == true)
        {
            Dash();
        }
    }


    private void SetPlayerVelocity()
    {
        _moveInputSmoothed = Vector2.SmoothDamp(
                    _moveInputSmoothed,
                    _moveInput,
                    ref _moveInputVelocity,
                    0.1f);

        _rigidbody2d.linearVelocity = _moveInputSmoothed * _swimSpeed;
        _animator.SetBool(Moving, _rigidbody2d.linearVelocity.magnitude > .1f);
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

    private void OnSprint(InputValue inputValue)
    {
        _isDashing = true;
    }

    private void Dash()
    {
        if (_dashCooldownCounter <= 0 && _dashCounter <= 0)
        {
            _swimSpeed = dashSpeed;
            _dashCounter = dashLength;
        }

        if (_dashCounter > 0) 
        {
            _dashCounter -= Time.deltaTime;

            if (_dashCounter <= 0)
            {
                _swimSpeed = normalMoveSpeed;
                _dashCooldownCounter = dashCooldown;
                _isDashing = false;
            }
        }

        if (_dashCooldownCounter > 0)
        {
            _dashCooldownCounter -= Time.deltaTime;
        }
    }
}
