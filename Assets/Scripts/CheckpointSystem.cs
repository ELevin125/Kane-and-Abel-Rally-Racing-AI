using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CheckpointSystem : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    private int targetCheckpoint = 0;

    [SerializeField] private Material startMat;
    public int startIndex = 0;
    
    #if UNITY_EDITOR
    void OnValidate() {
        if (startIndex < 0 || startIndex > checkpoints.Length - 1)
        {
            Debug.LogError("Set startIndex to default. startIndex out of range. Max level should be the size of checkpoints array.");
            startIndex = 0;
        }
    }
    private int oldStart = 0;

    void Update()
    {   
        if (oldStart != startIndex)
        {
            checkpoints[oldStart].ResetState();
            oldStart = startIndex;
        }
        checkpoints[startIndex].ChangeMaterial(startMat);
    }
    #endif

    // returns next checkpoint in the array and loops back to the start if everything was triggered
    public Transform GetNextCheckpoint()
    {
        while (checkpoints[targetCheckpoint].triggered)
        {
            int end = startIndex - 1;
            if (end < 0)
                end = checkpoints.Length - 1;
            if (targetCheckpoint == end)
            {
                ResetCheckpoints();
                targetCheckpoint = 0;
            }
            else
                targetCheckpoint++;
        }

        return checkpoints[targetCheckpoint].tf;
    }

    public bool TriggerNext(Checkpoint cp)
    {
        cp.Trigger();
        targetCheckpoint++;
        int end = startIndex - 1;
        if (end < 0)
            end = checkpoints.Length - 1;
        if (targetCheckpoint == end)
        {
            targetCheckpoint = 0;
            ResetCheckpoints();
            return true;
        }
        return false;
    }

    public void ResetCheckpoints()
    {
        foreach(Checkpoint c in checkpoints)
            c.ResetState();
    }
}
