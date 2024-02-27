using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KaneSystem : MonoBehaviour
{   
    public float rayHeight = 5f;
    
    [Header("Steering")]
    public int rowCount = 3;
    public int steeringRayCount = 16;
    public float halfFieldOfView = 45f;
    public float rowIncrement = 0.5f;
    public float steeringRayDistance = 2.8f;


    [SerializeField]
    private Vector3[] steeringRays;
    [SerializeField]
    private float steeringAngle = 0f;
    // how fast does the steering go back to 0
    // prevents the car having sporadic movement on uneven roads
    public float steeringCenterRate = 0.8f;
    
    [Header("Speed Control")]
    public float speedRayDistance = 1.4f;
    [SerializeField]
    private Vector3[] speedRays;
    public int speedRayCount = 6;
    public int speedRowCount = 12;
    public float speedRowIncrement = 0.5f;
    public float speedHalfFOV = 3f;

    private Rigidbody rb;

    private CarController carController;

    [SerializeField]
    private Transform sensorTransform;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();
    }

    void Start()
    {
        if (steeringRayCount % 2 != 0)
            Debug.LogError("Should have an even amount of rays");
    }

    void Update()
    {
        steeringRays = VehicleSensors.CalculateRays(sensorTransform, steeringRayCount, halfFieldOfView, rowCount, 3.5f, rowIncrement, steeringRayDistance);

        // // Change ray aim based on speed, higher speed aims further away
        // float forwardVelocity = Vector3.Dot(transform.forward, rb.velocity);
        // float topSpeed = 35f;
        // float normalisedSpeed = forwardVelocity / topSpeed;
        // float speedRayAngle = speedRayDistance + (1.5f * (1f - normalisedSpeed));
        // // Aim rays down when car tilts up or down, to ensure rays hit the road
        // if (Mathf.Abs(NormalizeAngle(gameObject.transform.localEulerAngles.x)) > 1)
        //     speedRayAngle += Mathf.Min(Mathf.Abs(NormalizeAngle(gameObject.transform.localEulerAngles.x)) / 12 * 6f, 6f);

        float speedRayAngle = VehicleSensors.CalculateAngleForSpeed(transform, rb, speedRayDistance);
        speedRays = VehicleSensors.CalculateRays(sensorTransform, speedRayCount, speedHalfFOV, speedRowCount,  0.2f, speedRowIncrement, speedRayAngle);
        ControlCar();
    }
    // float NormalizeAngle(float angle)
    // {
    //     while (angle > 180f)
    //     {
    //         angle -= 360f;
    //     }

    //     while (angle <= -180f)
    //     {
    //         angle += 360f;
    //     }

    //     return angle;
    // }

    // // starting left, first half points left, second half points right 
    // Vector3[] CalculateRays(int rayCount, float halfFieldOfView, int rowCount, float spread, float rowIncrement, float verticalAngle)
    // {
    //     List<Vector3> rays = new List<Vector3>();
    //     for (int i = 0; i < rayCount; i++)
    //     {   
    //         float angle = 0f;
    //         if (rayCount > 1)
    //             angle = i * (halfFieldOfView * 2f / (rayCount - 1)) - halfFieldOfView; // Calculate angle within the specified range
    //         else
    //             angle = 0f;

    //         for (int j = 0; j < rowCount; j++)
    //         {
    //             // Calculate direction for each ray within the specified range, slightly angled downwards
    //             Vector3 rayDirection = Quaternion.AngleAxis(angle + Mathf.Sign(angle) * spread * j, transform.up) * Quaternion.AngleAxis(verticalAngle + rowIncrement * j * j, transform.right) * transform.forward;
            
    //             rays.Add(rayDirection);
    //         }
    //     }
    //     return rays.ToArray();
    // }


    void ControlCar()
    {
   
        steeringAngle = CalculateSteeringInput(steeringAngle);


        float speedInput = CalculateSpeedInput(speedRays);

        if (carController)
        {
            
            carController.SetSteeringInput(steeringAngle);
            carController.SetThrottleInput(speedInput);
            float forwardVelocity = Vector3.Dot(transform.forward, rb.velocity);

            carController.SetBrakeInput((1-speedInput));
            if (forwardVelocity > 10f)
            {
                if ((1-speedInput) > 0.33f)
                    carController.SetBrakeInput(1f);
            }
            else
            {
                carController.SetBrakeInput(0);
            }

            if (MathF.Abs(steeringAngle) > 1.3)
                carController.SetHandbrake(true);
            else
                carController.SetHandbrake(false);
        }
        // steeringAngle = 0f;
        steeringAngle *= steeringCenterRate;
        if (Mathf.Abs(steeringAngle) < 0.05f)
            steeringAngle = 0;
    }

    float CalculateSteeringInput(float previousInput = 0f)
    {
        float steeringInput = previousInput;

        for (int i = 0; i < steeringRays.Length; i++)
        {
            Vector3 rayDirection = steeringRays[i];
            RaycastHit hit;
            Vector3 rayOrigin = sensorTransform.position + sensorTransform.up * rayHeight;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit))
            {
                if (hit.collider.tag != "road")
                {
                    if (i < steeringRays.Length / 2)
                    {
                        Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.cyan);   
                        steeringInput += 0.45f / (((float)steeringRays.Length / 2f));
                    }    
                    else
                    {
                        Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.blue);   
                        steeringInput -= 0.45f / (((float)steeringRays.Length / 2f));
                    }
                }
                else
                {
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.white);   
                }
            }
            else
                Debug.DrawRay(rayOrigin, rayDirection * 100, Color.yellow);   
        }

        return steeringInput;
    }

    // Determine the throttle input based on the raycasts
    // Each ray contribute a portion to the overall input
    // i.e. the more rays are offroad, the slower the vehicle will go
    float CalculateSpeedInput(Vector3[] speedRays)
    {
        float throttleInput = 0f;

        for (int i = 0; i < speedRays.Length; i++)
        {
            Vector3 rayDirection = speedRays[i];
            RaycastHit hit;
            Vector3 rayOrigin = sensorTransform.position + sensorTransform.up * rayHeight;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit))
            {
                if (hit.collider.tag == "road")
                {
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);   
                    throttleInput += 1f / (float)speedRays.Length;
                }
                else
                {
                    // offroad
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);   
                }
            }
            else
            {
                // Agent is blind
                Debug.DrawRay(rayOrigin, rayDirection * 100, Color.black);   
                // apply a little throttle to hopefully move out of the blind spot
                throttleInput += 0.1f / (float)speedRays.Length;
            }
        }

        return throttleInput;
    }
}
