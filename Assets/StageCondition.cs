using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherCondition {
    dry,
    rainy,
    foggy
}
public class StageCondition : MonoBehaviour
{
    public WeatherCondition weather;
    [SerializeField]
    private float rainGrip = 0.6f;
    [SerializeField]
    private float fogViewDistance = 0.6f;

    public float stageGrip { get; private set; }

    void Awake()
    {
        if (weather == WeatherCondition.rainy)
            stageGrip = rainGrip;
        else
            stageGrip = 1.0f;
    }
}
