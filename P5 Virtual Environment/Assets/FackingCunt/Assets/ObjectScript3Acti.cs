using System;
using UnityEngine;
public class ObjectScript3Acti : MonoBehaviour
{
    public bool fireSolved, waterSolved, doorOpened;
    public bool redActive, greenActive, blueActive;
    public BallScript ball;
    public GameObject door;
    public float height, doorSpeed;
    private float counter;
    

    private void Start()
    {
        counter = 0;
    }

    void OnTriggerEnter(Collider collision)
    {
        print("collision");
        switch (collision.name)
        {
            case ("Fire"):
                print("Fire Collision");
                redActive = ball.GetConditionsRGB()[0];
                if (redActive == true)
                {
                    fireSolved = true;
                    ball.Unfreeze();
                    print("fire solved");
                }
                break;
            case ("Water"):
                print("water collision");
                blueActive = ball.GetConditionsRGB()[2];
                if (blueActive == true)
                {
                    waterSolved = true;
                    ball.Unfreeze();
                    print("water solved");
                }
                break;
            case ("Door"):
                greenActive = ball.GetConditionsRGB()[1];
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
        if (doorOpened && counter < height)
        {
            Vector3 temp = door.transform.position;
            temp.y += doorSpeed;
            door.transform.position = temp;
            counter += doorSpeed;
        }
    }
}