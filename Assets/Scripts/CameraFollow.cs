using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public MainCharSwitch mcs; 
    public float smoothing = 5f; // How smoothly the camera catches up with its target movement
    public float min, max;

    void FixedUpdate() // Using FixedUpdate for smoother camera movement with physics updates
    {
        // Check if character's x position has reached or surpassed 0
        if (mcs.currentPos.x >= 0)
        {
            // Calculate the target x position for the camera, clamped between min and max
            float targetX = Mathf.Clamp(mcs.currentPos.x, min, max);

            // Create a new Vector3 for the camera's target position,
            // using the targetX for the x component, and preserving the current y and z components.
            Vector3 targetCamPos = new Vector3(targetX, transform.position.y, transform.position.z);

            // Smoothly interpolate between the camera's current position and the new target position
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}
