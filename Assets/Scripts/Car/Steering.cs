using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Steering : MonoBehaviour
{
    private Quaternion originalRotation;

    public float turnSpeed;
    public float maxTurningAngle;

    public VehicleControls vehicleControls;
    private float steeringInput = 0;

    public float minSteer = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        steeringInput = CarController.Instance.steeringInput;

        Quaternion targetRotation = originalRotation;

        if (!Mathf.Approximately(steeringInput, 0))
        {
            Quaternion twist = Quaternion.Euler(0, 90 * Mathf.Sign(steeringInput), 0);
            targetRotation = transform.rotation * twist;

            float normalizedYRot = NormalizeAngle(targetRotation.eulerAngles.y);
            float yRot = Mathf.Clamp(normalizedYRot, maxTurningAngle, -maxTurningAngle) * steeringInput;

            targetRotation.eulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);
        }

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

    }

    //return angle in range -180 to 180
    private float NormalizeAngle(float a)
    {
        return a - 180.0f * Mathf.Floor((a + 180.0f) / 180.0f);
    }

}
