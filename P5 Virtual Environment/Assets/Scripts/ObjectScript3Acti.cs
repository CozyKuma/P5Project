using System;
using UnityEngine;
public class ObjectScript3Acti : MonoBehaviour
{
    public bool fireSolved, waterSolved;
    public bool redActive, greenActive, blueActive;
    public BallScript ball;
    //public GameObject door;
    
    private RoomStateController roomStateController;
    [SerializeField] private GameObject CorrSystem;
    

    private void Start()
    {
        roomStateController = CorrSystem.GetComponent<RoomStateController>();
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
                    print("door open");
                    completePuzzle();
                }
                break;
        }
    }
    void Update()
    {

    }

    private void completePuzzle()
    {
        roomStateController.SetLevelComplete(true);
    }
}