using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : MonoBehaviour
{
    private VehicleControls vehicleControls;
    private CarController carController;

    void Awake()
    {
        vehicleControls = new VehicleControls();
        carController = GetComponent<CarController>();
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
        vehicleControls.Vehicle.Throttle.performed += ctx => carController.SetThrottleInput(ctx.ReadValue<float>());
        vehicleControls.Vehicle.Throttle.canceled += ctx => carController.SetThrottleInput(0f);
        
        vehicleControls.Vehicle.Brake.performed += ctx => carController.SetBrakeInput(ctx.ReadValue<float>());
        vehicleControls.Vehicle.Brake.canceled += ctx => carController.SetBrakeInput(0f);
        
        vehicleControls.Vehicle.Steering.performed += ctx => carController.SetSteeringInput(ctx.ReadValue<float>());
        vehicleControls.Vehicle.Steering.canceled += ctx => carController.SetSteeringInput(0f);

        vehicleControls.Vehicle.Hadbrake.performed += ctx => carController.SetHandbrake(true);
        vehicleControls.Vehicle.Hadbrake.canceled += ctx => carController.SetHandbrake(false);   
    }

}
