using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateController : MonoBehaviour
{

    public enum State { RoomState1, RoomState2, RoomState3 }

    public static State currentState = State.RoomState1;
    public GameObject doorPrefab;

    [SerializeField]
    private GameObject room1, room2, room3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaceDoor(Vector3 pos, Vector3 orientation)
    {

    }

    public static void ChangeState(State newState)
    {
        currentState = newState;
    }

    public static void NextState()
    {
        if (currentState == State.RoomState1)
        {
            ChangeState(State.RoomState2);
        } else if (currentState == State.RoomState2)
        {
            ChangeState(State.RoomState3);
        }
    }

    public static State getCurrentState()
    {
        return currentState;
    }
}
