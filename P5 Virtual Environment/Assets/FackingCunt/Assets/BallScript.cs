using UnityEngine;
public class BallScript : MonoBehaviour
{
    [SerializeField] [Range(1, 10)] public float velocity;
    public bool redCondition, greenCondition, blueCondition = false;
    public bool redActive, greenActive, blueActive = false;
    void MoveBall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
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
        Rigidbody rb = GetComponent<Rigidbody>();
        switch (collision.name)
        {
            case ("Red"):
                if (redCondition == false)
                {
                    velocity = 0;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    redActive = true;
                }
                break;
            case ("Green"):
                if (greenCondition == false)
                {
                    velocity = 0;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    greenActive = true;
                }
                break;
            case ("Blue"):
                if (blueCondition == false)
                {
                    velocity = 0;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    blueActive = true;
                }
                break;
            default:
                Debug.Log("Something went wrong.");
                break;
        }
    }
    void Update()
    {
        MoveBall();
        if (Input.GetKeyDown("space"))
        {
            print("Space key was pressed.");
            velocity = 1;
            if (redActive == true)
            {
                redCondition = true;
                redActive = false;
                print(redCondition + " red");
            }
            if (greenActive == true)
            {
                greenCondition = true;
                greenActive = false;
                print(greenCondition + " green");
            }
            if (blueActive == true)
            {
                blueCondition = true;
                blueActive = false;
                print(blueCondition + " blue");
            }
        }
        if(redCondition == true && greenCondition == true && blueCondition == true)
        {
            print("3Activations complete.");
        }
    }
    public bool[] GetConditionsRGB()
    {
        bool[] conditionArray = new bool[3] {redCondition, greenCondition, blueCondition};
        return conditionArray;
    }
}