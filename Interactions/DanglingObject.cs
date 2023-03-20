using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanglingObject : MonoBehaviour
{
    public float angleRange = 15.0f; // The maximum angle the object can swing
    public float damping = 0.1f; // The rate at which the swinging stops
    public float lerpSpeed = 5.0f; // The speed of the smooth movement
    public float speedMultiplier = 2.0f;

    private Quaternion initialRotation; // The initial rotation of the object
    private Quaternion targetRotation; // The target rotation of the object

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        float angle = angleRange * Mathf.Sin(Time.time * speedMultiplier); // Calculate the angle of the swing
        targetRotation = initialRotation * Quaternion.Euler(0, 0, angle); // Calculate the target rotation

        // Use lerp to smoothly rotate the object towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);

        // Dampen the swinging movement over time
        targetRotation = Quaternion.Lerp(targetRotation, initialRotation, Time.deltaTime * damping);
    }
}
