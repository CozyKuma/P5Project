using UnityEngine;
public class BallScript : MonoBehaviour
{
    public float multiplier = 500;
    public bool redCondition, blueCondition, greenCondition = false;
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
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.name)
        {
            case ("Red"):
                multiplier = 0;
                if (Input.GetKeyDown("space"))
                {
                    redCondition = true;
                    print(redCondition);
                }
               break;
            case ("Blue"):
                multiplier = 0;
                if (Input.GetKeyDown("space"))
                {
                    blueCondition = true;
                    print(blueCondition);
                }
                break;
            case ("Green"):
                multiplier = 0;
                if (Input.GetKeyDown("space"))
                {
                    greenCondition = true;
                    print(greenCondition);
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
            multiplier = 500;
        }
        if(greenCondition == true && redCondition == true && blueCondition == true)
        {
            print("ez win");
        }
    }
}