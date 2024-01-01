using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : MonoBehaviour
{
    private VehicleControls vehicleControls;

    void Awake()
    {
        vehicleControls = new VehicleControls();
    }

    void OnEnable()
    {
        vehicleControls.Enable();
    }

    void OnDisable()
    {
        vehicleControls.Disable();
    }

    void FixedUpdate()
    {
        vehicleControls.Vehicle.Throttle.performed += ctx => CarController.Instance.SetThrottleInput(ctx.ReadValue<float>());
        vehicleControls.Vehicle.Throttle.canceled += ctx => CarController.Instance.SetThrottleInput(0f);
        
        vehicleControls.Vehicle.Brake.performed += ctx => CarController.Instance.SetBrakeInput(ctx.ReadValue<float>());
        vehicleControls.Vehicle.Brake.canceled += ctx => CarController.Instance.SetBrakeInput(0f);
        
        vehicleControls.Vehicle.Steering.performed += ctx => CarController.Instance.SetSteeringInput(ctx.ReadValue<float>());
        vehicleControls.Vehicle.Steering.canceled += ctx => CarController.Instance.SetSteeringInput(0f);

        vehicleControls.Vehicle.Hadbrake.performed += ctx => CarController.Instance.SetHandbrake(true);
        vehicleControls.Vehicle.Hadbrake.canceled += ctx => CarController.Instance.SetHandbrake(false);   
    }

}
