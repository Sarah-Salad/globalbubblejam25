using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using System.Drawing.Drawing2D;

public class PlayerMovement : MonoBehaviour
{
    // Derived from the following tutorial https://www.youtube.com/watch?v=DVHcOS1E5OQ
    public float driftFactor = 0.95f;
    public float accelerationFactor = 3f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 5f;
    private float originalDriftFactor, originalAccelerationFactor, originalTurnFactor, originalMaxSpeed;

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;

    private float velocityVsUp = 0;
    public float dashFactor = 2f;
    public float dashTime = 1f;
    private bool isDashing = false;
    private Vector2 moveInput;

    private Rigidbody2D rigidBody2D;

    void Awake() {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
        // Store copies of values set by default or via inspector so that
        // once the dash is finished we can reset our values used in
        // physics calculations
        originalDriftFactor = driftFactor;
        originalAccelerationFactor = accelerationFactor;
        originalTurnFactor = turnFactor;
        originalMaxSpeed = maxSpeed;
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

    private void OnSprint(InputValue inputValue) {
        Dash();
    }

    void ApplyEngineForce() {
        velocityVsUp = Vector2.Dot(
            transform.up, rigidBody2D.linearVelocity
        );

        if (velocityVsUp > maxSpeed && accelerationInput > 0 || accelerationInput < 0) {
            // don't apply more engine force if above max speed or going backwards
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

    void Dash() {
        if (!isDashing && rigidBody2D.linearVelocity != Vector2.zero) {
            Debug.Log("Starting dash");
            isDashing = true;
            // boost 
            driftFactor = 0.01f;
            rigidBody2D.linearVelocity *= dashFactor;
            turnFactor *= 2;
            maxSpeed += dashFactor;
            // cool down before reset vals
            StartCoroutine(waitDashTime());
        }
    }

    IEnumerator waitDashTime() {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(dashTime);

        Debug.Log("Resetting vals");
        isDashing = false;
        driftFactor = originalDriftFactor;
        accelerationFactor = originalAccelerationFactor;
        turnFactor = originalTurnFactor;
        maxSpeed = originalMaxSpeed;
    }
}
