using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    VehicleControls vehicleControls;

    private void Awake()
    {
        vehicleControls = new VehicleControls();
    }

    private void OnEnable()
    {
        vehicleControls.Enable();
    }

    private void OnDisable()
    {
        vehicleControls.Disable();
    }

    private void Update()
    {
        Throttle();
        Steering();
    }
    float throttle = 0f;
    void Throttle()
    {
        
        vehicleControls.Vehicle.Throttle.performed += ctx => throttle = ctx.ReadValue<float>();
        vehicleControls.Vehicle.Throttle.canceled += ctx => throttle = 0f;
        Debug.Log(throttle);
    }

    void Brake()
    {

    }
    float steering = 0f;
    void Steering()
    {

        vehicleControls.Vehicle.Steering.performed += ctx => steering = ctx.ReadValue<float>();
        vehicleControls.Vehicle.Steering.canceled += ctx => steering = 0f;
        Debug.Log(steering);
    }
}
