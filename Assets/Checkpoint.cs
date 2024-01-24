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

    void Awake()
    {
        tf = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "car")
        {
            triggered = true;
            renderer.material = triggeredMat;
        }
    }

    public void ResetState()
    {
        triggered = false;
        renderer.material = untriggeredMat;
    }
}
