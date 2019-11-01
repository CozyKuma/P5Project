using System;
using UnityEditor;
using UnityEngine;
public class BallScript : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] public float speed;
    private float velocity;
    private Rigidbody rb;
    public bool redCondition, greenCondition, blueCondition = false;
    public bool redActive, greenActive, blueActive = false;
    
    private void Start()
    {
        velocity = speed;
        rb = GetComponent<Rigidbody>();
    }

    void MoveBall()
    {
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left     * velocity);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right    * velocity);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.forward  * velocity);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.back     * velocity);
    }
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.name)
        {
            case ("Red"):
                if (redCondition == false)
                {
                    FreezeBall();
                    redCondition = true;
                    redActive = true;
                }
                break;
            case ("Green"):
                if (greenCondition == false)
                {
                    FreezeBall();
                    greenActive = true;
                    greenCondition = true;
                }
                break;
            case ("Blue"):
                if (blueCondition == false)
                {
                    FreezeBall();
                    blueActive = true;
                    blueCondition = true;
                }
                break;
        }
    }

    public void Unfreeze()
    {
        velocity = speed;
    }
    void FreezeBall()
    {
        velocity = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    void Update()
    {
        MoveBall();
    }
    public bool[] GetConditionsRGB()
    {
        bool[] conditionArray = new bool[3] {redCondition, greenCondition, blueCondition};
        return conditionArray;
    }
}