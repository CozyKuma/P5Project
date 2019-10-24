using UnityEngine;
public class ObjectScript3Acti : MonoBehaviour
{
    public bool fireSolved, waterSolved, doorOpened = false;
    public bool redActive, greenActive, blueActive = false;
    public BallScript ball;
    void OnTriggerEnter(Collider collision)
    {
        print("collision");
        switch (collision.name)
        {
            case ("Fire"):
                print("Fire Collision");
                if (redActive == true)
                {
                    fireSolved = true;
                    print("fire solved");
                }
                break;
            case ("Water"):
                if (blueActive == true)
                {
                    waterSolved = true;
                    print("water solved");
                }
                break;
            case ("Door"):
                if (fireSolved == true && waterSolved == true && greenActive == true)
                {
                    doorOpened = true;
                    print("door open");
                }
                break;
        }
    }
    void Update()
    {
        redActive = ball.GetConditionsRGB()[0];
        blueActive = ball.GetConditionsRGB()[1];
        greenActive = ball.GetConditionsRGB()[2];
    }
}