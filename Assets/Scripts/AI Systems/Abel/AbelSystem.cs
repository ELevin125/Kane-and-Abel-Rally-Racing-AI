using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// [ExecuteInEditMode]
public class AbelSystem : Agent
{   
    // public float rayHeight = 5f;
    
    // [Header("Steering")]
    // public int rowCount = 3;
    // public int steeringRayCount = 16;
    // public float halfFieldOfView = 45f;
    // public float rowIncrement = 0.5f;
    // public float steeringRayDistance = 2.8f;


    // [SerializeField]
    // private Vector3[] steeringRays;
    // [SerializeField]
    // private float steeringAngle = 0f;
    // // how fast does the steering go back to 0
    // // prevents the car having sporadic movement on uneven roads
    // public float steeringCenterRate = 0.8f;
    
    // [Header("Speed Control")]
    // public float speedRayDistance = 1.4f;
    // [SerializeField]
    // private Vector3[] speedRays;
    // public int speedRayCount = 6;
    // public int speedRowCount = 12;
    // public float speedRowIncrement = 0.5f;
    // public float speedHalfFOV = 3f;

    // private Rigidbody rb;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();

    //     if (steeringRayCount % 2 != 0)
    //         Debug.LogError("Should have an even amount of rays");
    // }

    // void Update()
    // {
    //     steeringRays = VehicleSensors.CalculateRays(transform, steeringRayCount, halfFieldOfView, rowCount, 3.5f, rowIncrement, steeringRayDistance);

    //     float speedRayAngle = VehicleSensors.CalculateAngleForSpeed(transform, rb, speedRayDistance);
    //     speedRays = VehicleSensors.CalculateRays(transform, speedRayCount, speedHalfFOV, speedRowCount,  0.2f, speedRowIncrement, speedRayAngle);
    //     ControlCar();
    // }

    // void ControlCar()
    // {
   
    //     steeringAngle = CalculateSteeringInput(steeringAngle);


    //     float speedInput = CalculateSpeedInput();

    //     if (CarController.Instance)
    //     {
            
    //         CarController.Instance.SetSteeringInput(steeringAngle);
    //         CarController.Instance.SetThrottleInput(speedInput);
    //         float forwardVelocity = Vector3.Dot(transform.forward, rb.velocity);

    //         CarController.Instance.SetBrakeInput((1-speedInput));
    //         if (forwardVelocity > 10f)
    //         {
    //             if ((1-speedInput) > 0.33f)
    //                 CarController.Instance.SetBrakeInput(1f);
    //         }
    //         else
    //         {
    //             CarController.Instance.SetBrakeInput(0);
    //         }

    //         if (MathF.Abs(steeringAngle) > 1.3)
    //             CarController.Instance.SetHandbrake(true);
    //         else
    //             CarController.Instance.SetHandbrake(false);
    //     }
    //     // steeringAngle = 0f;
    //     steeringAngle *= steeringCenterRate;
    //     if (Mathf.Abs(steeringAngle) < 0.05f)
    //         steeringAngle = 0;
    // }

    // float CalculateSteeringInput(float previousInput = 0f)
    // {
    //     float steeringInput = previousInput;

    //     for (int i = 0; i < steeringRays.Length; i++)
    //     {
    //         Vector3 rayDirection = steeringRays[i];
    //         RaycastHit hit;
    //         Vector3 rayOrigin = transform.position + transform.up * rayHeight;
    //         if (Physics.Raycast(rayOrigin, rayDirection, out hit))
    //         {
    //             if (hit.collider.tag != "road")
    //             {
    //                 if (i < steeringRays.Length / 2)
    //                 {
    //                     Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.cyan);   
    //                     steeringInput += 0.45f / (((float)steeringRays.Length / 2f));
    //                 }    
    //                 else
    //                 {
    //                     Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.blue);   
    //                     steeringInput -= 0.45f / (((float)steeringRays.Length / 2f));
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.white);   
    //             }
    //         }
    //         else
    //             Debug.DrawRay(rayOrigin, rayDirection * 100, Color.yellow);   
    //     }

    //     return steeringInput;
    // }

    // float CalculateSpeedInput()
    // {
    //     float throttleInput = 0f;

    //     for (int i = 0; i < speedRays.Length; i++)
    //     {
    //         Vector3 rayDirection = speedRays[i];
    //         RaycastHit hit;
    //         Vector3 rayOrigin = transform.position + transform.up * rayHeight;
    //         if (Physics.Raycast(rayOrigin, rayDirection, out hit))
    //         {
    //             if (hit.collider.tag == "road")
    //             {
    //                 Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);   
    //                 throttleInput += 1f / (float)speedRays.Length;
    //             }
    //             else
    //             {
    //                 Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);   
    //             }
    //         }
    //         else
    //             Debug.DrawRay(rayOrigin, rayDirection * 100, Color.black);   
    //             throttleInput += 0.1f / (float)speedRays.Length;
    //     }

    //     return throttleInput;
    // }
}
