using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool triggered = false;
    public Transform tf;

    [SerializeField]
    private Renderer renderer;
    [SerializeField]
    private Material untriggeredMat;
    [SerializeField]
    private Material triggeredMat;


    public delegate void CheckpointTriggeredEventHandler();
    public static event CheckpointTriggeredEventHandler OnCheckpointTriggered;

    void Awake()
    {
        tf = transform;
    }

    public void ChangeMaterial(Material targetMat)
    {
        renderer.material = targetMat;
    }

    public void Trigger()
    {
        triggered = true;
        renderer.material = triggeredMat;

        // Invoke the event when triggered
        if (OnCheckpointTriggered != null)
        {
            OnCheckpointTriggered();
        }
    }

    public void ResetState()
    {
        triggered = false;
        renderer.material = untriggeredMat;
    }
}
