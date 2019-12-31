using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class RobotAgent : Agent {
    Rigidbody rBody;
    public float speed = 10;
    [HideInInspector] public float oldDistance = 1000f;
    public Vector3 goal = new Vector3(10f, 0.1f, 0f);
    private RayPerception3D rayPerception;

    private float cumulativeReward = 0.0f;

    void Start () {
        rBody = GetComponent<Rigidbody>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    public Transform target;
    public override void AgentReset()
    {
       // if (this.transform.position.y < 0)
        //{
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(7f, 0.5f, Random.Range(-2,2));
        //}

        // Move the target to a new spot       
        target.position = new Vector3(-6.0f,
                                      0.1f,
                                      Random.Range(-2,2));
    }

    public override void CollectObservations() {
        // Target and Agent positions
        AddVectorObs(target.position);
        AddVectorObs(this.transform.position);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);

        float rayDistance = 12f;
        float[] rayAngles = {30f, 45f, 60f, 75f, 90f, 105f, 120f, 135f, 150f};
        string[] detectableObjects = {"Target", "Wall"};
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));


    }
    public override void AgentAction(float[] vectorAction, string textAction) {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                target.position);
        float m = (goal.z - target.position.z) / (goal.x - target.position.x);
        float b = target.position.z;
        
        // Moving closer to target
        /*if (distanceToTarget < oldDistance)
            SetReward(0.1f);
        else
            SetReward(-0.1f);*/

        // Reached target
       /* if (distanceToTarget < 1.20f) {
            SetReward(1.0f);
            cumulativeReward += 1.0f;
            Done();
        }*/

        // Fell off platform
        if (this.transform.position.y < 0) {
            SetReward(-1.0f);
            cumulativeReward -= 1.0f;
            Done();
        }

        if (target.position.y <= 0) {
            SetReward(-1.0f);
            cumulativeReward -= 1.0f;
            Done();
        }

        if (this.transform.position.z <= m * this.transform.position.x + b + 1.0f && this.transform.position.z >= m * this.transform.position.x + b - 1.0f && this.transform.position.x < goal.x - 2) {
            SetReward(0.1f);
           // Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            cumulativeReward += 0.1f;
        }
        else {
            SetReward(-0.1f);
            cumulativeReward -= 0.1f;
        }

        /*if (target.position.z <= maxZ && target.position.z >= minZ) {
            SetReward(0.1f);
        }

        if (target.position.z > maxZ || target.position.z < minZ) {
            SetReward(-0.1f);
        }*/

        if (this.transform.position.x <= target.position.x) {
            SetReward(-0.1f);
            cumulativeReward -= 0.1f;
        }

        /*if (cumulativeReward <= -5f) {
            cumulativeReward = 0.0f;
            Done();
        }*/
        
        oldDistance = distanceToTarget;

    }

    public override float[] Heuristic() {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Target") {
            SetReward(1.0f);
            cumulativeReward += 1.0f;
            //Done();
        }

    }
}
