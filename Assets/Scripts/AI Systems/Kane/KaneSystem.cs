using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KaneSystem : MonoBehaviour
{   
    public int VisionRaysCount = 8;
    public float halfFieldOfView = 45f;
    public float visionDistance = 5f;
    private float[] mainVisionRays;

    void Update()
    {
        DrawRays();
    }

    void DrawRays()
    {

        for (int i = 0; i < VisionRaysCount; i++)
        {
            float angle = i * (halfFieldOfView * 2f / (VisionRaysCount - 1)) - halfFieldOfView; // Calculate angle within the specified range
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward; // Rotate the forward vector by the angle

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, visionDistance))
            {
                // Draw the ray until it hits an object
                Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.red);
            }
            else
            {
                // Draw the ray if it doesn't hit anything
                Debug.DrawRay(transform.position, rayDirection * visionDistance, Color.red);
            }
        }
    }


}
