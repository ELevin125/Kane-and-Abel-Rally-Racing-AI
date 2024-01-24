using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VehicleSensors
{
    // starting left, first half points left, second half points right 
    public static Vector3[] CalculateRays(Transform tf, int rayCount, float halfFieldOfView, int rowCount, float spread, float rowIncrement, float verticalAngle)
    {
        List<Vector3> rays = new List<Vector3>();
        for (int i = 0; i < rayCount; i++)
        {   
            float angle = 0f;
            if (rayCount > 1)
                angle = i * (halfFieldOfView * 2f / (rayCount - 1)) - halfFieldOfView; // Calculate angle within the specified range
            else
                angle = 0f;

            for (int j = 0; j < rowCount; j++)
            {
                // Calculate direction for each ray within the specified range, slightly angled downwards
                Vector3 rayDirection = Quaternion.AngleAxis(angle + Mathf.Sign(angle) * spread * j, tf.up) * Quaternion.AngleAxis(verticalAngle + rowIncrement * j * j, tf.right) * tf.forward;
            
                rays.Add(rayDirection);
            }
        }
        return rays.ToArray();
    }

    public static float CalculateAngleForSpeed(Transform tf, Rigidbody rb, float speedRayDistance)
    {
        // Change ray aim based on speed, higher speed aims further away
        float forwardVelocity = Vector3.Dot(tf.forward, rb.velocity);
        float topSpeed = 35f;
        float normalisedSpeed = forwardVelocity / topSpeed;
        float speedRayAngle = speedRayDistance + (1.5f * (1f - normalisedSpeed));
        // Aim rays down when car tilts up or down, to ensure rays hit the road
        if (Mathf.Abs(NormalizeAngle(tf.localEulerAngles.x)) > 1)
            speedRayAngle += Mathf.Min(Mathf.Abs(NormalizeAngle(tf.localEulerAngles.x)) / 12 * 6f, 6f);

        return speedRayAngle;
    }

    static float NormalizeAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }

        while (angle <= -180f)
        {
            angle += 360f;
        }

        return angle;
    }
}
