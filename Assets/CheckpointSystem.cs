using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    private int targetCheckpoint = 0;

    // returns next checkpoint in the array and loops back to the start if everything was triggered
    public Transform GetNextCheckpoint()
    {
        while (checkpoints[targetCheckpoint].triggered)
        {
            if (targetCheckpoint >= checkpoints.Length - 1)
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
        if (targetCheckpoint >= checkpoints.Length - 1)
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
