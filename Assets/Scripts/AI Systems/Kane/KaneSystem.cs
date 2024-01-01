using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaneSystem : MonoBehaviour
{   
    public int rowCount = 3;
    public int visionRaysCount = 4;
    public float halfFieldOfView = 45f;
    public float maxVisionDistance = -3f;
    public float rowIncrement = 0.5f;
    public float rayHeight = 5f;
    [SerializeField]
    private Quaternion[] mainVisionRays;
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
    Quaternion[] CalculateRays()
    {
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // BUGGED - as car rotates around y axis, the spread of the vertical rays change
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        List<Quaternion> rays = new List<Quaternion>();
        for (int i = 0; i < visionRaysCount; i++)
        {
            float angle = i * (halfFieldOfView * 2f / (visionRaysCount - 1)) - halfFieldOfView; // Calculate angle within the specified range
            for (int j = 0; j < rowCount; j++)
            {
                Quaternion rayDirection = Quaternion.Euler(-maxVisionDistance - rowIncrement * j * j, angle, 0);
                rays.Add(rayDirection);
            }
        }
        return rays.ToArray();
    }


    void ControlCar()
    {
        for (int i = 0; i < mainVisionRays.Length; i++)
        {
            Vector3 rayDirection = mainVisionRays[i] * transform.forward;
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + transform.up * rayHeight;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit))
            {
                if (hit.collider.tag != "road")
                {
                    if (i < mainVisionRays.Length / 2)
                    {
                        Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);   
                        Debug.Log(1f / ((float)mainVisionRays.Length / 2f));
                        steeringAngle += 0.5f / ((float)mainVisionRays.Length / 2f);
                    }    
                    else
                    {
                        Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.blue);   
                        steeringAngle -= 0.5f / ((float)mainVisionRays.Length / 2f);
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
        CarController.Instance.SetSteeringInput(steeringAngle);

        steeringAngle *= steeringCenterRate;
        if (Mathf.Abs(steeringAngle) < 0.05f)
            steeringAngle = 0;
    }
}
