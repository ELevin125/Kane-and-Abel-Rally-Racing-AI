using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StabiliseSensors : MonoBehaviour
{
    public Transform targetTransform;
    public float raycastLength = 10f;
    
    public float yOffset;
    public float raycastAngle = 45f;
    public Transform rotationOffset; // the rotation it should have when the terrain is perfectly flat

    void Update()
    {
        Vector3 rayDirection = Quaternion.AngleAxis(raycastAngle, transform.right) * transform.forward;
        Ray ray = new Ray(transform.position + transform.up * yOffset, rayDirection);
        // Draw the ray for visualisation
        Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.magenta);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastLength))
        {
            // Get the normal of the terrain at the hit point
            // i.e. the direction of the road ahead
            Vector3 terrainNormal = hit.normal;

            // Calculate the rotation to align with the terrain normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, terrainNormal);

            // Apply the initial rotation and then adjust based on the terrain normal
            targetTransform.rotation = rotationOffset.rotation * targetRotation;
        }
    }
}
