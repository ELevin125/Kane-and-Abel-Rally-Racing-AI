using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelForces : MonoBehaviour
{
    private Rigidbody rb;
    private Transform carTransform;

    [Header("Suspension")]
    [Range(-0.5f, 2.0f)]
    public float suspensionRestDist;
    [Range(0.0f, 1.0f)]
    public float maxSpringStretch;
    private float trueRestDist;
    public float springStregth;
    public float springDamper;

    [Header("Wheels")]
    public bool powered = true;
    public float tireRadius;
    [Range(0.0f, 5.0f)]
    public float tireGrip;
    public float tireGripHandbrakeLoss;
    public float tireMass;
    public Transform wheelPrefab;
    private Transform wheel;
    // public float skidThreshold = 8f;
    // private TrailRenderer trailRenderer;
    public Transform wheelModel;

    [Header("Engine")]
    public float topSpeed;
    public float maxTorque;
    [Range(0.0f, 1.0f)]
    public float TorqueCurveOffset;

    public VehicleControls vehicleControls;
    private float throttleInput = 0;
    private float brakeInput = 0;
    private bool handbrakeInput = false;

    private float normalizedSpeed = 0.0f;
    
    private void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();
        carTransform = transform.root;

        if (wheelPrefab)
        {
            wheel = Instantiate(wheelPrefab, transform.position, Quaternion.identity);
            wheel.parent = transform;
            wheelModel = transform.GetChild(0).GetChild(0);
            // trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

    }

    private void FixedUpdate()
    {
        normalizedSpeed = Mathf.Clamp01(Mathf.Abs(rb.velocity.magnitude) / topSpeed);

        Inputs();
        SkidMarks();
        RotateWheels();
        trueRestDist = suspensionRestDist + tireRadius;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, (trueRestDist + maxSpringStretch)))
        {
            Suspension(hit);
            Steering();
            if (powered)
                Movement();
        }
        else
        {

            // if (trailRenderer.emitting)
            //     trailRenderer.emitting = false;
        }
    }

    void Inputs()
    {
        vehicleControls.Vehicle.Throttle.performed += ctx => throttleInput = ctx.ReadValue<float>();
        vehicleControls.Vehicle.Throttle.canceled += ctx => throttleInput = 0f;

        vehicleControls.Vehicle.Brake.performed += ctx => brakeInput = ctx.ReadValue<float>();
        vehicleControls.Vehicle.Brake.canceled += ctx => brakeInput = 0f;

        vehicleControls.Vehicle.Hadbrake.performed += ctx => handbrakeInput = true;
        vehicleControls.Vehicle.Hadbrake.canceled += ctx => handbrakeInput = false;
    }

    void SkidMarks()
    {
        Vector3 steeringDir = transform.forward;
        Vector3 tireVel = rb.GetPointVelocity(transform.position).normalized;

        float steeringVel = Vector3.Dot(steeringDir, tireVel);

        // if (Mathf.Abs(steeringVel) < skidThreshold)
        //     trailRenderer.emitting = true;
        // else if (trailRenderer.emitting)
        //     trailRenderer.emitting = false;
    }

    void Suspension(RaycastHit hit)
    {
        Vector3 springDir = transform.up;
        Vector3 tireVel = rb.GetPointVelocity(transform.position);
        float offset = trueRestDist - hit.distance;

        wheel.localPosition = new Vector3(0, -(suspensionRestDist - offset), 0);

        float velocity = Vector3.Dot(springDir, tireVel);
        float force = (offset * springStregth) - (velocity * springDamper);

        rb.AddForceAtPosition(springDir * force, transform.position);
    }

    void Steering()
    {
        if (Mathf.Approximately(rb.velocity.magnitude, 0))
            return;

        Vector3 steeringDir = transform.right;
        Vector3 tireVel = rb.GetPointVelocity(transform.position);

        float steeringVel = Vector3.Dot(steeringDir, tireVel);

        float understeerPenalty = Mathf.Clamp01(1 - normalizedSpeed + 0.2f);

        float desiredVelChange = -steeringVel * tireGrip * understeerPenalty;

        desiredVelChange = handbrakeInput ? desiredVelChange * tireGripHandbrakeLoss : desiredVelChange;

        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        rb.AddForceAtPosition(steeringDir * tireMass * desiredAccel, transform.position);
    }


    void Movement()
    {
        float accelInput = throttleInput - brakeInput;

        Vector3 accelDir = transform.forward;
        if (!Mathf.Approximately(accelInput, 0))
        {
            float carspeed = Vector3.Dot(carTransform.forward, rb.velocity);

            float powerCurveValue = -30f * (Mathf.Pow((normalizedSpeed - TorqueCurveOffset), 6f)) + 1.0f;
            float availableTorque = accelInput * powerCurveValue * maxTorque;
            Debug.DrawRay(transform.position, accelDir * availableTorque);
            rb.AddForceAtPosition(accelDir * availableTorque, transform.position);
        }
    }

    void RotateWheels()
    {
        float accelInput = throttleInput - brakeInput;
        float vehicleSpeed = rb.GetPointVelocity(transform.position).magnitude;
        // creates fake wheelspin, and prevents the powered wheels from not spinning if they are not on the ground
        vehicleSpeed = powered ? Mathf.Max(vehicleSpeed, 1) * 2.0f : vehicleSpeed * 2.0f; 

        float inputPower = powered ? Mathf.Max(Mathf.Abs(accelInput), 0.8f) : 1;

        wheelModel.Rotate(-vehicleSpeed * inputPower * Mathf.Sign(accelInput), 0, 0);
    }

}
