using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class AbelProto : Agent
{
    private float timeOffroad = 0;
    public float maxTimeOffroad = 240;
    public Transform startPos;
    [SerializeField] 
    private CheckpointSystem checkpoints;
    private string collidedTag = "road";
    public override void OnEpisodeBegin()
    {
        timeOffroad = 0;
        CarController.Instance.ResetInputs();
        checkpoints.ResetCheckpoints();
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;
    }

    void FixedUpdate()
    {
        if (timeOffroad > maxTimeOffroad)
        {
            AddReward(-3);
            EndEpisode();
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            collidedTag = hit.collider.tag;
        }

        switch (collidedTag)
        {
            case "road":
                timeOffroad = 0;
                break;
            case "offroad":
                timeOffroad++;
                AddReward(-0.1f);
                break;
            
            default:
                break;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 checkpointForward = checkpoints.GetNextCheckpoint().transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);

        switch (collidedTag)
        {
            case "road":
                sensor.AddObservation(0);
                break;
            case "offroad":
                sensor.AddObservation(1);
                break;
            
            default:
                sensor.AddObservation(0);
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions =  actionsOut.ContinuousActions;
        continuousActions[0] = Mathf.Max(Input.GetAxisRaw("Vertical"), 0);
        continuousActions[1] = Mathf.Min(Input.GetAxisRaw("Vertical"), 0);
        continuousActions[2] = Input.GetAxisRaw("Horizontal");
        // Debug.Log(continuousActions[0]);
        // Debug.Log(continuousActions[1]);
        // Debug.Log(continuousActions[2]);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float throttleInput = actions.ContinuousActions[0];
        float brakeInput = actions.ContinuousActions[1];
        float steeringInput = actions.ContinuousActions[2];

        if (CarController.Instance)
        {
            CarController.Instance.SetThrottleInput(throttleInput);
            CarController.Instance.SetBrakeInput(brakeInput);
            CarController.Instance.SetSteeringInput(steeringInput);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint")
        {
            Vector3 triggerForwardDirection = other.transform.forward;
            Vector3 enteringDirection = other.transform.position - transform.position;

            triggerForwardDirection.Normalize();
            enteringDirection.Normalize();
            float dotProduct = Vector3.Dot(triggerForwardDirection, enteringDirection);

                
            if (dotProduct > 0)
                AddReward(2); // The object entered the trigger from the back
            else
            {
                AddReward(-1);
                EndEpisode();
            }
        }
    }
}
