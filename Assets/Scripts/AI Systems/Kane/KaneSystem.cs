using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KaneSystem : MonoBehaviour
{   
    public int visionRaysCount = 4;
    public float halfFieldOfView = 45f;
    public float visionDistance = 5f;
    private Vector3[] mainVisionRays;

    void Start()
    {
        mainVisionRays = CalculateRays();
    }

    void Update()
    {
        #if UNITY_EDITOR
        mainVisionRays = CalculateRays();
        #endif
        DrawRays();
    }

    Vector3[] CalculateRays()
    {
        List<Vector3> rays = new List<Vector3>();

        for (int i = 0; i < visionRaysCount; i++)
        {
            float angle = i * (halfFieldOfView * 2f / (visionRaysCount - 1)) - halfFieldOfView; // Calculate angle within the specified range
            Vector3 rayDirection = Quaternion.Euler(-visionDistance, angle, 0) * transform.forward; // Rotate the forward vector by the angle
            
            rays.Add(rayDirection);
        }
        return rays.ToArray();
    }

    void DrawRays()
    {

        foreach(Vector3 rayDirection in mainVisionRays)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                // Draw the ray until it hits an object
                if (hit.collider.tag == "road")
                    Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.green);
                else
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
