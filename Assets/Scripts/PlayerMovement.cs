using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    // Derived from the following tutorial https://www.youtube.com/watch?v=DVHcOS1E5OQ
    public float driftFactor = 0.95f;
    public float accelerationFactor = 3f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 5f;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;

    private float velocityVsUp = 0;
    private Vector2 moveInput;

    private Rigidbody2D rigidBody2D;

    void Awake() {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        ApplyEngineForce();
        
        DampenOrthogonalVelocity();

        ApplySteering();
    }

    private void OnMove(InputValue inputValue) {
        moveInput = inputValue.Get<Vector2>();
        accelerationInput = moveInput.y;
        steeringInput = moveInput.x;
    }

    void ApplyEngineForce() {
        velocityVsUp = Vector2.Dot(
            transform.up, rigidBody2D.linearVelocity
        );

        if (velocityVsUp > maxSpeed && accelerationInput > 0 || accelerationInput < 0) {
            // don't apply engine force if above max speed or going backwards
            return;
        }

        if (accelerationInput == 0) {
            rigidBody2D.linearDamping = Mathf.Lerp(rigidBody2D.linearDamping, 3.0f, Time.fixedDeltaTime * 3);
        }
        else {
            rigidBody2D.linearDamping = 0;
        }

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        rigidBody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering() {
        rotationAngle -= steeringInput * turnFactor;

        rigidBody2D.MoveRotation(rotationAngle);
    }

    void DampenOrthogonalVelocity() {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(
            rigidBody2D.linearVelocity, transform.up
        );

        Vector2 rightVelocity = transform.right * Vector2.Dot(
            rigidBody2D.linearVelocity, transform.right
        );

        rigidBody2D.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }
}
