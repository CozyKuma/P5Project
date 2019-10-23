using System;
using UnityEngine;
public class BallScript : MonoBehaviour
{
    public float velocity;
    private float multiplier = 10;
    public bool redCondition, blueCondition, greenCondition = false;
    public bool redActive, blueActive, greenActive = false;

    private void Start()
    {
    multiplier = velocity;
    }

    void MoveBall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left    * Time.deltaTime * multiplier);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right   * Time.deltaTime * multiplier);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.forward * Time.deltaTime * multiplier);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.back    * Time.deltaTime * multiplier);
    }
    //void OnCollisionEnter(Collision collision)
    void OnTriggerEnter(Collider collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        switch (collision.name)
        {
            case ("Red"):
                if (redCondition == false)
                {
                    multiplier = 0;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    redActive = true;
                }
                break;
            case ("Blue"):
                if (blueCondition == false)
                {
                    multiplier = 0;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    blueActive = true;
                }
                break;
            case ("Green"):
                if (greenCondition == false)
                {
                    multiplier = 0;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    greenActive = true;
                }
                break;
        }
    }
    void Update()
    {
        MoveBall();
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            multiplier = velocity;
            if (redActive == true)
            {
                redCondition = true;
                redActive = false;
                print(redCondition+" red");
            }
            if (blueActive == true)
            {
                blueCondition = true;
                blueActive = false;
                print(blueCondition+" blue");
            }
            if (greenActive == true)
            {
                greenCondition = true;
                greenActive = false;
                print(greenCondition+" green");
            }
        }
        if(greenCondition == true && redCondition == true && blueCondition == true)
        {
            print("ez win");
        }
    }
}