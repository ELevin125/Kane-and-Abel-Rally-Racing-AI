using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    private float fogViewOffset = 1f;
    [SerializeField]
    private Color fogColor;
    private float startingViewOffset;
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

        if (weather == WeatherCondition.foggy)
        {
            Transform offset = FindFirstObjectByType<RayOffset>().transform;
            startingViewOffset = offset.localRotation.eulerAngles.x;
            offset.localEulerAngles = new Vector3(startingViewOffset + fogViewOffset, 0f, 0f);

            RenderSettings.fogColor = fogColor;
            RenderSettings.fogEndDistance = 100;
            RenderSettings.fogStartDistance = 10;
        }

    }

    void Update()
    {
        // change values during runtime, so that weather can be changed at runtime too
        if (weather == WeatherCondition.rainy)
            stageGrip = rainGrip;
        else
            stageGrip = 1.0f;

        if (weather == WeatherCondition.foggy)
        {
            Transform offset = FindFirstObjectByType<RayOffset>().transform;
            if (offset.localRotation.eulerAngles.x != startingViewOffset + fogViewOffset)
                offset.localEulerAngles = new Vector3(startingViewOffset + fogViewOffset, 0f, 0f);
        }
    }

}
