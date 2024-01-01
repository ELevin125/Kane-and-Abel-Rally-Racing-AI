using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle", fileName = "New Vehicle")]
public class VehicleConfig : ScriptableObject
{
    [Header("Suspension")]
    [Range(-0.5f, 2.0f)]
    public float suspensionRestDist = 0.0f;
    [Range(0.0f, 1.0f)]
    public float maxSpringStretch = 0.17f;
    public float springStregth = 6200.0f;
    public float springDamper = 100.0f;

    [Header("Wheels")]
    public bool allWheelDrive = true;
    public float tireRadius = 0.34f;
    [Range(0.0f, 5.0f)]
    public float frontTireGrip = 1.0f;
    [Range(0.0f, 1.0f)]
    public float frontTireGripHandbrakeLoss = 1.0f;
    [Range(0.0f, 5.0f)]
    public float rearTireGrip = 0.2f;
    [Range(0.0f, 1.0f)]
    public float rearTireGripHandbrakeLoss = 1.0f;
    public float tireMass = 1.0f;
    public Transform wheelPrefab;
    [Range(0.0f, 1.0f)]
    public float skidThreshold = 0.8f;
    public float turnSpeed = 200.0f;
    public float maxTurningAngle = 33.0f;

    [Header("Engine")]
    public float topSpeed = 50.0f;
    public float maxTorque = 300.0f;
    [Range(0.0f, 1.0f)]
    public float TorqueCurveOffset = 0.48f;
}
