using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMat;
    [SerializeField] private Material loseMat;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions =  actionsOut.ContinuousActions;
        continuousActions[0] =  Input.GetAxisRaw("Horizontal");
        continuousActions[1] =  Input.GetAxisRaw("Vertical");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish") {
            SetReward(1f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        }
        else if (other.tag == "Respawn") {
            SetReward(-1f);
            floorMeshRenderer.material = loseMat;
            EndEpisode();
        }
    }
}
