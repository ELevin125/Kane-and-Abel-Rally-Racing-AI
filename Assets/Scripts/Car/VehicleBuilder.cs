using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using UnityEngine;

[ExecuteInEditMode]
public class VehicleBuilder : MonoBehaviour
{
    public VehicleConfig config;
    public GameObject[] frontWheels;
    public GameObject[] backWheels;

    private GameObject[] allWheels;

    private VehicleControls vehicleControls;

    private void OnEnable()
    {
        vehicleControls.Enable();
    }

    private void OnDisable()
    {
        vehicleControls.Disable();
    }
    private void Awake()
    {
        vehicleControls = new VehicleControls();

        allWheels = frontWheels.Concat(backWheels).ToArray();
        foreach (GameObject wheel in allWheels)
        {
            WheelForces wf;
            if (wheel.TryGetComponent(out WheelForces existingWF))
                wf = existingWF;
            else
                wf = wheel.AddComponent<WheelForces>();

            wf.suspensionRestDist = config.suspensionRestDist;
            wf.maxSpringStretch = config.maxSpringStretch;
            wf.springStregth = config.springStregth;
            wf.springDamper = config.springDamper;
            wf.powered = true;
            wf.tireRadius = config.tireRadius;
            wf.tireMass = config.tireMass;
            wf.wheelPrefab = config.wheelPrefab;
            wf.skidThreshold = config.skidThreshold;
            wf.topSpeed = config.topSpeed;
            wf.maxTorque = config.maxTorque;
            wf.TorqueCurveOffset = config.TorqueCurveOffset;
            wf.tireGrip = config.rearTireGrip;
            wf.tireGripHandbrakeLoss = config.rearTireGripHandbrakeLoss;

            wf.vehicleControls = vehicleControls;
        }


        foreach (GameObject frontWheel in frontWheels)
        {
            WheelForces wf = frontWheel.GetComponent<WheelForces>();
            wf.tireGrip = config.frontTireGrip;
            wf.tireGripHandbrakeLoss = config.frontTireGripHandbrakeLoss;

            if (!config.allWheelDrive)
                wf.powered = false;

            Steering st;
            if (frontWheel.TryGetComponent(out Steering existingST))
                st = existingST;
            else
                st = frontWheel.AddComponent<Steering>();

            st.turnSpeed = config.turnSpeed;
            st.maxTurningAngle = config.maxTurningAngle;

            st.vehicleControls = vehicleControls;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity * 2);

        foreach (GameObject wheel in allWheels)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(wheel.transform.position, wheel.transform.position + new Vector3(0, -(config.suspensionRestDist + config.tireRadius + config.maxSpringStretch), 0));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(wheel.transform.position, wheel.transform.position + new Vector3(0, -config.suspensionRestDist, 0));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wheel.transform.position + new Vector3(0, -config.suspensionRestDist, 0), config.tireRadius);
        }
    }
}
