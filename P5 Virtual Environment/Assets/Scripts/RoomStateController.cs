using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateController : MonoBehaviour
{

    public enum State { RoomState1, RoomState2, RoomState3 }

    public static State oldState = State.RoomState1;
    public static State currentState = State.RoomState1;

    [SerializeField]
    public GameObject room1, room2, room3;

    // Start is called before the first frame update
    void Start()
    {
        ActivateRoom(getCurrentState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaceDoor(Vector3 pos, Vector3 orientation)
    {

    }

    public void ActivateRoom(State currRoom)
    {
        GameObject roomObj = currRoom == State.RoomState1 ? room1 :
            currRoom == State.RoomState2 ? room2 : room3;
        roomObj.SetActive(true);
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
        ActivateRoom(currentState);
    }

    public void NextState()
    {
        oldState = currentState;
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

    public static void RoomComplete()
    {
        Debug.Log("LEVEL COMPLETE!");
        LevelTransition();
    }

    public static void LevelTransition()
    {
        // Open Door to Corridor if Object is not close to door
        
        // When player in Corridor, Remove Room
        // When player is in SIDE2 Corridor, Place next roomstate (NextState())
    }
}
