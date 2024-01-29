using UnityEngine;

public class CarController : MonoBehaviour
{
    // public static CarController Instance { get; private set; }
    public float steeringInput { get; private set; }
    public float throttleInput { get; private set; }
    public float brakeInput { get; private set; }
    public bool handbrake { get; private set; }

    // void Awake()
    // {
    //     if (Instance == null)
    //         Instance = this;
    //     else
    //         Destroy(gameObject);
    // }

    public void SetSteeringInput(float angle)
    {
        steeringInput = Mathf.Clamp(angle, -1f, 1f);
    }

    public void SetThrottleInput(float value)
    {
        throttleInput = Mathf.Clamp(value, -1f, 1f);
    }

    public void SetBrakeInput(float value)
    {
        brakeInput = Mathf.Clamp(value, -1f, 1f);
    }

    public void SetHandbrake(bool value)
    {
        handbrake = value;
    }

    public void ResetInputs()
    {
        steeringInput = 0f;
        throttleInput = 0f;
        brakeInput = 0f;
        handbrake = false;
    }
}
