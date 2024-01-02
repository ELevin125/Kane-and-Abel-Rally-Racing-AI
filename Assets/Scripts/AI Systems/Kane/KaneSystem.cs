using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KaneSystem : MonoBehaviour
{   
    public int rowCount = 3;
    public int visionRaysCount = 4;
    public float halfFieldOfView = 45f;
    public float maxVisionDistance = -3f;
    public float rowIncrement = 0.5f;
    public float rayHeight = 5f;
    [SerializeField]
    private Vector3[] mainVisionRays;
    [SerializeField]
    private float steeringAngle = 0f;
    // how fast does the steering go back to 0
    // prevents the car having sporadic movement on uneven roads
    public float steeringCenterRate = 0.8f;
    void Start()
    {
        if (visionRaysCount % 2 != 0)
            Debug.LogError("Should have an even amount of rays");
        mainVisionRays = CalculateRays();
    }

    void Update()
    {
        mainVisionRays = CalculateRays();
        ControlCar();
    }

    // starting left, first half points left, second half points right 
    Vector3[] CalculateRays()
    {
        List<Vector3> rays = new List<Vector3>();
        for (int i = 0; i < visionRaysCount; i++)
        {
            float angle = i * (halfFieldOfView * 2f / (visionRaysCount - 1)) - halfFieldOfView; // Calculate angle within the specified range
            for (int j = 0; j < rowCount; j++)
            {
                // Calculate direction for each ray within the specified range, slightly angled downwards
                Vector3 rayDirection = Quaternion.AngleAxis(angle + Mathf.Sign(angle) * 1.5f * j, transform.up) * Quaternion.AngleAxis(maxVisionDistance + rowIncrement * j * j, transform.right) * transform.forward;
            
                rays.Add(rayDirection);
            }
        }
        return rays.ToArray();
    }


    void ControlCar()
    {
        int nonRoadHits = 0;

        for (int i = 0; i < mainVisionRays.Length; i++)
        {
            Vector3 rayDirection = mainVisionRays[i];
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + transform.up * rayHeight;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit))
            {
                if (hit.collider.tag != "road")
                {
                    nonRoadHits++;
                    if (i < mainVisionRays.Length / 2)
                    {
                        Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);   
                        steeringAngle += 0.5f / (((float)mainVisionRays.Length / 2f));
                    }    
                    else
                    {
                        Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.blue);   
                        steeringAngle -= 0.5f / (((float)mainVisionRays.Length / 2f));
                    }
                }
                else
                {
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);   
                }
            }
            else
                Debug.DrawRay(rayOrigin, rayDirection * 100, Color.black);   
        }

        if (CarController.Instance)
        {
            CarController.Instance.SetSteeringInput(steeringAngle);
            CarController.Instance.SetThrottleInput(1 - ((nonRoadHits * 1.5f) / mainVisionRays.Length));
            float brakeInput = CalculateBrakeInput(nonRoadHits, mainVisionRays.Length);
            Debug.Log(brakeInput);
            // CarController.Instance.SetBrakeInput(brakeInput);
        }
        
        steeringAngle *= steeringCenterRate;
        if (Mathf.Abs(steeringAngle) < 0.05f)
            steeringAngle = 0;
    }

    float CalculateBrakeInput(int nonRoadHits, int totalRaysCount)
    {
        return (float)nonRoadHits / totalRaysCount;
        // float ratio = (float)nonRoadHits / totalRaysCount;
        // float exponent = 2.0f;

        // // Exponential function to control the growth of BrakeInput based on the ratio
        // float brakeInput = Mathf.Pow(ratio, exponent);

        // return brakeInput;
    }
}
