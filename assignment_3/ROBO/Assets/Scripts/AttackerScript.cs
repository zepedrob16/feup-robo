using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class AttackerScript : Agent {
    Rigidbody rBody;
    public float speed = 10;
    [HideInInspector] public float oldDistance = 1000f;
    public Vector3 goal = new Vector3(10f, 0.1f, 0f);
    private RayPerception3D rayPerception;
    private float cumulativeReward = 0.0f;
    private float timer = 0.0f;
    public Rigidbody target;

    void Start () {
        rBody = GetComponent<Rigidbody>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    public override void AgentReset()
    {
       // if (this.transform.position.y < 0)
        //{
            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(-7f, 0.5f, Random.Range(-2,2));
        //}

        // Move the target to a new spot       
        target.transform.position = new Vector3(Random.Range(-5.0f,0),
                                      0.1f,
                                      Random.Range(-1,3));

        target.velocity = Vector3.zero;
        target.angularVelocity = Vector3.zero;
        timer = 0.0f;
        oldDistance = 0.0f;
        //target.AddForce(new Vector3(20f,0f,20f));
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
        float distanceToTarget = Vector3.Distance(this.transform.position,target.transform.position);

        if (this.rBody.velocity.x >= 6.0f)
            this.rBody.velocity = new Vector3(6.0f, this.rBody.velocity.y, this.rBody.velocity.z);

        if (this.rBody.velocity.z >= 6.0f)
            this.rBody.velocity = new Vector3(this.rBody.velocity.x, this.rBody.velocity.y, 6.0f);

        if (this.rBody.velocity.x <= -6.0f)
            this.rBody.velocity = new Vector3(-6.0f, this.rBody.velocity.y, this.rBody.velocity.z);

        if (this.rBody.velocity.z <= -6.0f)
            this.rBody.velocity = new Vector3(this.rBody.velocity.x, this.rBody.velocity.y, -6.0f);
        
        if (this.transform.position.x >= 0)
            this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);

        if (target.transform.position.x >= 10f) {
            SetReward(1.0f);
            Done();
        }

        if (target.velocity.x >= 3.0f)
            SetReward(0.1f);

        if (target.velocity.x < 3.0f)
            SetReward(-0.1f);

        if (this.transform.position.x >= 0)
            SetReward(-0.1f);

        timer++;
        if (timer >= 600) {
            SetReward(-1.0f);
            Done();
        }

        if (distanceToTarget < oldDistance)
            SetReward(0.1f);
        else
            SetReward(-0.1f);

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
            SetReward(0.1f);
            cumulativeReward += 1.0f;
            Done();
        }

    }
}