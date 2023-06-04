using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent 
{
    // Assignables
    private Rigidbody rb;

    // Wing Information
    [Header("Wing Information")]
    public float thrust = 5f;
    public float wingAcc = 10f;
    public float rotateSpeed = 10f;
    [Space]
    public Transform wing1;
    public Transform wing2;
    public Transform wing3;
    public Transform wing4;
    [Space]
    public float wingForce1;
    public float wingForce2;
    public float wingForce3;
    public float wingForce4;

    // Agent Information
    [Header("Agent Inforrmation")]
    public GameObject target1;
    public GameObject target2;
    public float graceDist = 0.1f;
    public float graceDegree = 20f;
    public float prevDist = 0f;
    [Space]
    public float startPosRange = 1f;
    public float startGoalRange = 1f;
    public float maxFlyRange = 10f;
    [Space]
    public float rewardEnergyUse = -0.01f;
    public float rewardNotFlat = -0.05f;
    public float rewardImprovement = 1f;
    public float rewardOnGoal = 0.01f;
    public float rewardVelocityMult = 1f;

    // Other Helpful Variables
    private bool touchingGoal = false;

    void Start() 
    {
        // Assign Rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        // Set Target Materials
        target1.GetComponent<Goal>().Activated(true);
        target2.GetComponent<Goal>().Disabled();
    }

    void FixedUpdate()
    {
        // Add Force to Wings
        rb.AddForceAtPosition((wingForce1) * transform.up, wing1.position);
        rb.AddForceAtPosition((wingForce2) * transform.up, wing2.position);
        rb.AddForceAtPosition((wingForce3) * transform.up, wing3.position);
        rb.AddForceAtPosition((wingForce4) * transform.up, wing4.position);

        // Rotate Wings
        wing1.Rotate(0f, 0f, (wingForce1) * rotateSpeed);
        wing2.Rotate(0f, 0f, (wingForce2) * rotateSpeed);
        wing3.Rotate(0f, 0f, (wingForce3) * rotateSpeed);
        wing4.Rotate(0f, 0f, (wingForce4) * rotateSpeed);

        // Touching Target?
        if (Vector3.Distance(transform.localPosition, target1.transform.localPosition) < graceDist) 
        {
            target1.transform.position = target2.transform.position;
            target2.GetComponent<Goal>().ResetPos(startGoalRange);
            Debug.Log("Reached Goal");
        }
    }

    public override void OnEpisodeBegin() 
    {
        // Reset State
        target1.GetComponent<Goal>().ResetPos(startGoalRange);
        target2.GetComponent<Goal>().ResetPos(startGoalRange);
        transform.localPosition = new Vector3(Random.Range(-startPosRange, startPosRange), Random.Range(-startPosRange, startPosRange), Random.Range(-startPosRange, startPosRange));
        transform.localRotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        prevDist = Vector3.Distance(transform.localPosition, target1.transform.localPosition);

        wingForce2 = 0f;
        wingForce1 = 0f;
        wingForce3 = 0f;
        wingForce4 = 0f;
    }

    public override void CollectObservations(VectorSensor sensor) 
    {
        sensor.AddObservation(target1.transform.localPosition - transform.localPosition);
        sensor.AddObservation(target2.transform.localPosition - transform.localPosition);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) 
    {
        wingForce1 = (actions.ContinuousActions[0] + 1f) / 2f * thrust;
        wingForce2 = (actions.ContinuousActions[1] + 1f) / 2f * thrust;
        wingForce3 = (actions.ContinuousActions[2] + 1f) / 2f * thrust;
        wingForce4 = (actions.ContinuousActions[3] + 1f) / 2f * thrust;

        // Calculate Energy Used
        float energyUsed = (wingForce1 + wingForce2 + wingForce3 + wingForce4) / thrust;
        float improvement = prevDist - Vector3.Distance(transform.localPosition, target1.transform.localPosition);
        float velocityDir = Vector3.Dot(Vector3.Normalize(rb.velocity), Vector3.Normalize(target1.transform.localPosition - transform.localPosition)) * rb.velocity.magnitude * rewardVelocityMult;
        prevDist = Vector3.Distance(transform.localPosition, target1.transform.localPosition);
        var anglePortion = Mathf.Max(0f, (Vector3.Angle(transform.up, Vector3.up) - graceDegree));

        // Apply Rewards
        SetReward((improvement * rewardImprovement) + (rewardEnergyUse * energyUsed) + (anglePortion * rewardNotFlat) + velocityDir);

        if (transform.localPosition.magnitude > maxFlyRange) 
        {
            Debug.Log("Exceeded Boundry");
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) 
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.Alpha1)) {continuousActions[0] = 1f;} else {continuousActions[0] = -1f;}
        if (Input.GetKey(KeyCode.Alpha2)) {continuousActions[1] = 1f;} else {continuousActions[1] = -1f;}
        if (Input.GetKey(KeyCode.Alpha3)) {continuousActions[2] = 1f;} else {continuousActions[2] = -1f;}
        if (Input.GetKey(KeyCode.Alpha4)) {continuousActions[3] = 1f;} else {continuousActions[3] = -1f;}
    }
}
