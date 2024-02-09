using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StabiliseSensors : MonoBehaviour
{
    // public float raycastLength = 10f;
    // public float raycastAngle = 45f;
    // public float yOffset;
    // [Range(0f, 2f)]
    // public float tiltMultiplier = 1f;
    // public Transform targetTransform;

    // void Update()
    // {
    //     Vector3 rayDirection = Quaternion.AngleAxis(raycastAngle, transform.right) * transform.forward;
    //     Ray ray = new Ray(transform.position + transform.up * yOffset, rayDirection);

    //     // Draw the ray for visualization
    //     Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.magenta);

    //     RaycastHit hit;


    //     if (Physics.Raycast(ray, out hit, raycastLength))
    //     {
    //         // Calculate the angle of the hit object
    //         Debug.DrawRay(hit.point, hit.normal * 5, Color.black);
    //         float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

    //         // Calculate the rotation from the current up direction to the hit normal
    //         Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

    //         // Apply the tilt rotation with the specified multiplier
    //         float rotationAmount = slopeAngle * tiltMultiplier;
    //         targetRotation *= Quaternion.Euler(rotationAmount, 0, 0);
    //         targetTransform.rotation = targetRotation;
    //     }
    // }

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
