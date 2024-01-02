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
    public int speedRowCount = 12;
    public float speedRowIncrement = 0.5f;
    public float speedHalfFOV = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (steeringRayCount % 2 != 0)
            Debug.LogError("Should have an even amount of rays");
    }

    void Update()
    {
        steeringRays = CalculateRays(steeringRayCount, halfFieldOfView, rowCount, 3f, rowIncrement, steeringRayDistance);

        float forwardVelocity = Vector3.Dot(transform.forward, rb.velocity);
        float topSpeed = 35f;
        float normalisedSpeed = forwardVelocity / topSpeed;
        speedRays = CalculateRays(12, speedHalfFOV, speedRowCount,  0.2f, speedRowIncrement, speedRayDistance + (1.5f * (1f - normalisedSpeed)));
        ControlCar();
    }

    // starting left, first half points left, second half points right 
    Vector3[] CalculateRays(int rayCount, float halfFieldOfView, int rowCount, float spread, float rowIncrement, float verticalAngle)
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
                Vector3 rayDirection = Quaternion.AngleAxis(angle + Mathf.Sign(angle) * spread * j, transform.up) * Quaternion.AngleAxis(verticalAngle + rowIncrement * j * j, transform.right) * transform.forward;
            
                rays.Add(rayDirection);
            }
        }
        return rays.ToArray();
    }


    void ControlCar()
    {
   
        steeringAngle = CalculateSteeringInput(steeringAngle);


        float speedInput = CalculateSpeedInput();

        if (CarController.Instance)
        {
            
            CarController.Instance.SetSteeringInput(steeringAngle);
            CarController.Instance.SetThrottleInput(speedInput);
            float forwardVelocity = Vector3.Dot(transform.forward, rb.velocity);
            // Debug.Log("velos " + forwardVelocity.ToString());
            CarController.Instance.SetBrakeInput((1-speedInput));
            if (forwardVelocity > 10f)
            {
                if ((1-speedInput) > 0.33f && Mathf.Abs(steeringAngle) < 0.2)
                    CarController.Instance.SetBrakeInput(1f);
            }
            else
            {
                CarController.Instance.SetBrakeInput(0);
            }
            Debug.Log(steeringAngle);

            if (MathF.Abs(steeringAngle) > 1.4)
                CarController.Instance.SetHandbrake(true);
            else
                CarController.Instance.SetHandbrake(false);

            // Debug.Log("Throt " +CarController.Instance.throttleInput.ToString());
            // Debug.Log("brake " + CarController.Instance.brakeInput.ToString());
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
            Vector3 rayOrigin = transform.position + transform.up * rayHeight;
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
                Debug.DrawRay(rayOrigin, rayDirection * 100, Color.black);   
        }

        return steeringInput;
    }

    float CalculateSpeedInput()
    {
        float throttleInput = 0f;

        for (int i = 0; i < speedRays.Length; i++)
        {
            Vector3 rayDirection = speedRays[i];
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + transform.up * rayHeight;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit))
            {
                if (hit.collider.tag == "road")
                {
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);   
                    throttleInput += 1f / (float)speedRays.Length;
                }
                else
                {
                    // throttleInput -= 1f / (float)speedRays.Length;
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);   
                }
            }
            else
                Debug.DrawRay(rayOrigin, rayDirection * 100, Color.black);   
                throttleInput += 0.1f / (float)speedRays.Length;
        }

        return throttleInput;
    }
}
