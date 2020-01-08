using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public GameObject gameObject;
    private Vector3 target;
    public GameObject invisibleWall;
    private bool goingUp = true;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {   
        target = new Vector3(10f, 0.1f, -1.2f);
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    

    // Update is called once per frame
    void Update()
    {  
        if (rb.velocity.x >= 7.0f)
            rb.velocity = new Vector3(7.0f,0.0f,rb.velocity.z);
        if (rb.velocity.z >= 5.0f)
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, 5.0f);
    }
}
