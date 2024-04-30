using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Car : MonoBehaviour
{
    // Variables to control car movement
    [SerializeField] float accelerationSpeed = 5f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float reverseSpeed = 3f; // Speed when reversing
    [SerializeField] float brakeSpeed = 5f;
    [SerializeField] float turnSpeed = 100f;

    // Reference to the parking progress slider
    public UnityEngine.UI.Slider parkingProgressSlider;


    private float currentSpeed = 0f;
    private float currentTurn = 0f;

    private bool hasParked = false;

    public Collider2D parkingSpaceCollider;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasParked)
            return;

        // Brake takes priority
        if (Input.GetKey(KeyCode.B))
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= brakeSpeed * Time.deltaTime;
            }
            else if (currentSpeed < 0) // Braking while reversing
            {
                currentSpeed += brakeSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0;
            }
        }
        // Acceleration
        else if (Input.GetKey(KeyCode.Space))
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += accelerationSpeed * Time.deltaTime;
            }
        }
        // Reverse
        else if (Input.GetKey(KeyCode.R))
        {
            if (currentSpeed > -maxSpeed / 2) // Limit the reverse speed
            {
                currentSpeed -= reverseSpeed * Time.deltaTime;
            }
        }
        // Natural deceleration or 'idle' braking when no keys pressed
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= brakeSpeed * Time.deltaTime; // Decelerate
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += brakeSpeed * Time.deltaTime; // Decelerate when reversing
            }
            else
            {
                currentSpeed = 0;
            }
        }


        // Apply current speed to move the car
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        // Turning left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        currentTurn = horizontalInput * turnSpeed * Time.deltaTime;

        transform.Rotate(Vector3.forward * -currentTurn);

        // Check if the car is inside the parking lot
        if (parkingSpaceCollider != null && parkingSpaceCollider.bounds.Contains(transform.position))
        {
            float distanceToEnter = Vector2.Distance(transform.position, parkingSpaceCollider.bounds.min);
            float totalDistance = Vector2.Distance(parkingSpaceCollider.bounds.min, parkingSpaceCollider.bounds.max);
            float parkingProgress = (distanceToEnter / totalDistance) * 100f;

            // Update the parking progress slider value
            parkingProgressSlider.value = parkingProgress;

            // Check if parking is complete
            if (parkingProgress >= 100f)
            {
                hasParked = true;
                Debug.Log("Parking complete!");
            }
        }

    }

}






