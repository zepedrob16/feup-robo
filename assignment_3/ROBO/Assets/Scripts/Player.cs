using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            rigidBody.AddForce(new Vector3(0f,0f,6f));
        if (Input.GetKey(KeyCode.A))
            rigidBody.AddForce(new Vector3(-6f,0f,0f));
        if (Input.GetKey(KeyCode.S))
            rigidBody.AddForce(new Vector3(0f,0f,-6f));
        if (Input.GetKey(KeyCode.D))
            rigidBody.AddForce(new Vector3(6f,0f,0f));
    }
}
