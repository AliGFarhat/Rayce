using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    // Local Variables
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;

    float velocityVsUp = 0;

    // Components
    Rigidbody2D carRigidbody2D;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }

    void ApplyEngineForce()
    {
        // Calculate how much forward velocity in terms of direction.
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        // Limit the car max speed.
        if (velocityVsUp > maxSpeed && accelerationInput > 0) // Forward
            return;

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) // Reverse
            return;

        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) // Left and Right
            return;

        // Apply drag if no acceleration input. Meaning the car slows down if the accelerate input is not being pressed.
        if (accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else carRigidbody2D.drag = 0;

        // Create a force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        // Apply force and moves the car forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        // Prevent the car turning unless at a certain speed
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // Update the rotation angle based on the input
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        // Apply the steering by rotating the Car Object.
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float GetLateralVelocity()
    {
        // Returns how fast the car is moving sideways.
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        // Check if the car is moving forwards and if the player is inputting the brakes.
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true; 
        }

        // If there is alot of side velocity then tires should screech
        if (Mathf.Abs(GetLateralVelocity()) > 7.0)
            return true;

        return false;
    }
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude()
    {
        return carRigidbody2D.velocity.magnitude;
    }
}