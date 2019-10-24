using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript3Acti : MonoBehaviour
{
    public bool fireSolved = false;
    public bool waterSolved = false;
    public bool doorOpened = false;
    public bool redActive = false;
    public bool blueActive = false;
    public bool greenActive = false;
    public BallScript ball;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    void OnTriggerEnter(Collider collision)
    {
        print("collision");
        switch (collision.name)
        {
            case ("Water"):
                if (blueActive == true)
                {
                    waterSolved = true;
                    print("water solved");
                }
                break;
            case ("Fire"):
                print("Fire Collision");
                if (redActive == true)
                {
                    fireSolved = true;
                    print("fire solved");
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

    // Update is called once per frame
    void Update()
    {
        redActive = ball.GetConditionsRBG()[0];
        blueActive = ball.GetConditionsRBG()[1];
        greenActive = ball.GetConditionsRBG()[2];
    }
}
