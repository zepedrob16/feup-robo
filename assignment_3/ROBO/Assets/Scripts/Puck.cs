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
        //rb.AddForce(new Vector3(0f,0f,40f));
        //Physics.IgnoreCollision(GetComponent<BoxCollider>(), invisibleWall.GetComponent<BoxCollider>());
    }

    

    // Update is called once per frame
    void Update()
    {  
        /*if (rb.velocity.x >= 7.0f)
            rb.velocity = new Vector3(7.0f,0.0f,rb.velocity.z);
        if (rb.velocity.z >= 5.0f)
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, 5.0f);
*/
        //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, Time.deltaTime * 6);
       /* if (goingUp)
            gameObject.transform.position += new Vector3(0.05f,0,0.05f);
        if (!goingUp)
            gameObject.transform.position += new Vector3(0.05f,0,-0.05f);

        if (gameObject.transform.position.y <= -3) {
            gameObject.transform.position = new Vector3(Random.value * 8, 0.1f, -Random.value * 4);
        }*/
//if (Input.GetMouseButtonDown(0))
           // rb.AddForce(new Vector3(10f,0f,20f));
    }

    void OnCollisionEnter(Collision collision) {
        /*if (collision.gameObject.tag == "Wall") {
            if (goingUp)
                goingUp = false;
            else
                goingUp = true;
        }*/
        if (collision.gameObject.tag == "Invisible") {
            //Debug.Log("AAAAAAAAAAAAAAAAAAAA");
           // gameObject.transform.position = new Vector3(0f,0f,0f);
            //rb.AddForce(new Vector3(20f,0f,0f));
        }
    }
}
