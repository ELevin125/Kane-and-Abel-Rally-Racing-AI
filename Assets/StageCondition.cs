using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class StageCondition : MonoBehaviour
{
    public enum WeatherCondition {
        dry,
        rainy,
        foggy
    }
    public WeatherCondition weather;
    [SerializeField]
    private Light sunLight;
    [SerializeField]
    private Color rainLightColor;
    [SerializeField]
    private float rainGrip = 0.8f;
    [SerializeField]
    private float fogViewDistance = 0.8f;

    public float stageGrip { get; private set; }

    void Awake()
    {
        if (weather == WeatherCondition.rainy)
        {
            stageGrip = rainGrip;
            ParticleSystem[] rainEffect = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particleSys in rainEffect)
            {
                particleSys.Play();
            }
            sunLight.color = rainLightColor;
        }
        else
            stageGrip = 1.0f;


    }
}
