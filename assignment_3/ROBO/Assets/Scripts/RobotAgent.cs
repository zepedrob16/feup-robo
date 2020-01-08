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
    private float timer = 0.0f;
    public Rigidbody target;
    private bool moveTowards = false;

    void Start () {
        rBody = GetComponent<Rigidbody>();
        rayPerception = GetComponent<RayPerception3D>();
       // this.transform.rotation = new Quaternion(0f, -Mathf.PI/2, 0f, this.transform.rotation.w);
    }

    public override void AgentReset()
    {

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.position = new Vector3(7f, 0.5f, Random.Range(-2,2));
        this.transform.rotation = new Quaternion(0f, 0f, 0f, this.transform.rotation.w);
        this.transform.rotation = new Quaternion(0f, -1f, 0f, this.transform.rotation.w);

        // Move the target to a new spot       
        target.transform.position = new Vector3(-2.0f,
                                      0.1f,
                                      Random.Range(-1,3));

        target.velocity = Vector3.zero;
        target.angularVelocity = Vector3.zero;
        timer = 0.0f;
    }

    public override void CollectObservations() {
        // Target and Agent positions
        AddVectorObs(target.transform.position);
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
       // float distanceToTarget = Vector3.Distance(this.transform.position,target.position);
        float m = (goal.z - target.transform.position.z) / (goal.x - target.transform.position.x);
        float b = target.transform.position.z;

        // Fell off platform
        if (this.transform.position.y < 0) {
            SetReward(-1.0f);
            cumulativeReward -= 1.0f;
            timer = 0.0f;
            Done();
        }

        if (target.transform.position.x >= 10) {
            SetReward(-1.0f);
            cumulativeReward -= 1.0f;
            timer = 0.0f;
            Done();
        }

        if (target.transform.position.x <= -9)
            Done();

        if (this.transform.position.z <= m * this.transform.position.x + b + 1.0f && this.transform.position.z >= m * this.transform.position.x + b - 1.0f && this.transform.position.x < goal.x - 2) {
            SetReward(0.1f);
            cumulativeReward += 0.1f;
        }
        else {
            SetReward(-0.1f);
            cumulativeReward -= 0.1f;
        }

        if (this.transform.position.x <= target.transform.position.x) {
            SetReward(-0.1f);
            cumulativeReward -= 0.1f;
        }
    }

    public override float[] Heuristic() {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Target") {
            /*if (target.transform.position.x >= this.transform.position.x) {
                SetReward(-1.0f);
                timer = 0.0f;
                Done();
            } 
            else {*/
                SetReward(1.0f);
                cumulativeReward += 1.0f;
                timer = 0.0f;
            //    Done();
            //}
        }

    }
}
