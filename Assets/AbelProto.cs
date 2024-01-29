    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AbelProto : Agent
{
    private float timeSinceLastCheckpoint = 0;
    public float maxTimeSinceLastCheckpoint = 15; 
    private float timeOffroad = 0;
    public float maxTimeOffroad = 240;
    public Transform startPos;
    [SerializeField] 
    private CheckpointSystem checkpoints;
    private string collidedTag = "road";
    private bool insideCheckpoint = false;

    private Rigidbody rb;

    private CarController carController;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        timeOffroad = 0;
        timeSinceLastCheckpoint = 0; 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        carController.ResetInputs();
        checkpoints.ResetCheckpoints();
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;
    }

    void FixedUpdate()
    {
        // AddReward(-0.001f);
        if (timeOffroad > maxTimeOffroad)
        {
            AddReward(-3);
            EndEpisode();
        }

        if (timeSinceLastCheckpoint > maxTimeSinceLastCheckpoint)
        {
            AddReward(-1);
            EndEpisode();
        }

        timeSinceLastCheckpoint += Time.fixedDeltaTime;  // Update the timer

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

        AddReward(-0.0001f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector3 checkpointForward = checkpoints.GetNextCheckpoint().transform.forward;
        // float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        // sensor.AddObservation(directionDot);
        Vector3 normalVelocity = rb.velocity.normalized;
        // sensor.AddObservation(normalVelocity);
        float speed = normalVelocity.magnitude;
        sensor.AddObservation(speed);

        // sensor.AddObservation(transform.rotation.eulerAngles.x / 180);

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
        float verticalInput = Input.GetAxisRaw("Vertical");
        continuousActions[0] = Mathf.Max(verticalInput, 0);
        continuousActions[1] = -Mathf.Min(verticalInput, 0);
        continuousActions[2] = Input.GetAxisRaw("Horizontal");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float throttleInput = Mathf.Clamp01(actions.ContinuousActions[0]);
        float brakeInput = Mathf.Clamp01(actions.ContinuousActions[1]);
        float steeringInput = actions.ContinuousActions[2];

        // Debug.Log("Brake " + brakeInput.ToString());
        // Debug.Log("Throttle " + throttleInput.ToString());
        if (carController)
        {
            carController.SetThrottleInput(throttleInput);
            carController.SetBrakeInput(brakeInput);
            carController.SetSteeringInput(steeringInput);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "checkpoint")
            insideCheckpoint = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint" && !insideCheckpoint)
        {
            insideCheckpoint = true;
            Checkpoint cp = other.gameObject.GetComponent<Checkpoint>();
            if (cp.triggered == false)
            {
                timeSinceLastCheckpoint = 0;
                bool complete = checkpoints.TriggerNext(cp);
                if (complete)
                {
                    AddReward(10);
                    if (timeSinceLastCheckpoint < 2)
                        AddReward(2f - timeSinceLastCheckpoint);
                }

                Vector3 triggerForwardDirection = other.transform.forward;
                Vector3 enteringDirection = other.transform.position - transform.position;

                triggerForwardDirection.Normalize();
                enteringDirection.Normalize();
                float dotProduct = Vector3.Dot(triggerForwardDirection, enteringDirection);

                    
                if (dotProduct > 0)
                    AddReward(2); // The object entered the trigger from the back
                else
                {
                    AddReward(-3);
                    EndEpisode();
                }

            }
            else
            {
                AddReward(-3f);
                EndEpisode();
            }
        }
    }
}
