using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitatingPlatform : MonoBehaviour
{
      public float floatingHeight = 1.0f; // The height at which the object floats
      public float lerpSpeed = 5.0f; // The speed of the smooth movement
  
      private Vector3 initialPosition; // The initial position of the object
      private Vector3 targetPosition; // The target position of the object
  
      void Start()
      {
          initialPosition = transform.position;
      }
  
      void Update()
      {
          float verticalOffset = Mathf.Sin(Time.time); // Calculate the vertical offset for the floating effect
          targetPosition = initialPosition + new Vector3(0, verticalOffset, 0) * floatingHeight; // Calculate the target position
  
          // Use lerp to smoothly move the object towards the target position
          transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
      }
}
