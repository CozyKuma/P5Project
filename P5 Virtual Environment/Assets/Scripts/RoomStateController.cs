﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateController : MonoBehaviour
{

    public enum State { RoomState0, RoomState1, RoomState2, RoomState3 }

    public CorridorSystemV2 CorridorSystem;

    public static State oldState = State.RoomState0;
    public static State currentState = State.RoomState0;

    [SerializeField]
    public GameObject room0, room1, room2, room3;

    private List<GameObject> DoorWalls = new List<GameObject>();
    private GameObject[,] WallArray = new GameObject[4,2];

    [SerializeField] private GameObject _DoorWallPrefab, _RoomWallPrefab;
    [SerializeField] private GameObject parentGameObject;
    

    // Start is called before the first frame update
    void Start()
    {
        ActivateRoom(getCurrentState());
        parentGameObject = GameObject.Find("ScaleContainer");
        
        CreateRoomWalls();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            ActivateRoom(State.RoomState0);
            DeactivateRoom(oldState);
        } else if (Input.GetKeyDown("1"))
        {
            ActivateRoom(State.RoomState1);
            DeactivateRoom(oldState);
            CorridorSystem.GenerateInitialSetup();
        } else if (Input.GetKeyDown("2"))
        {
            ActivateRoom(State.RoomState2);
            DeactivateRoom(oldState);
        } else if (Input.GetKeyDown("3"))
        {
            ActivateRoom(State.RoomState3);
            DeactivateRoom(oldState);
        } else if (Input.GetKeyDown("q"))
        {
            NextState();
            if (currentState == State.RoomState1)
            {
                CorridorSystem.GenerateInitialSetup();
            }
        }
    }

    public static void PlaceDoor(Vector3 pos, Vector3 orientation)
    {

    }

    public void ActivateRoom(State roomToActivate)
    {
        oldState = currentState;
        GameObject roomObj = roomToActivate == State.RoomState0 ? room0 : roomToActivate == State.RoomState1 ? room1 :
            roomToActivate == State.RoomState2 ? room2 : room3;
        roomObj.SetActive(true);
        currentState = roomToActivate;
    }

    public void DeactivateRoom(State roomToDeactivate)
    {
        GameObject roomObj = roomToDeactivate == State.RoomState0 ? room0 : roomToDeactivate == State.RoomState1 ? room1 :
            roomToDeactivate == State.RoomState2 ? room2 : room3;
        roomObj.SetActive(false);
    }

    public void ChangeState(State newState)
    {
        ActivateRoom(newState);
        DeactivateRoom(oldState);
    }

    public void NextState()
    {
        if (currentState == State.RoomState0)
        {
            ChangeState(State.RoomState1);
        } else if (currentState == State.RoomState1)
        {
            ChangeState(State.RoomState2);
        } else if (currentState == State.RoomState2)
        {
            ChangeState(State.RoomState3);
        }
    }

    public State getCurrentState()
    {
        return currentState;
    }

    public void RoomComplete()
    {
        Debug.Log("LEVEL COMPLETE!");
        LevelTransition();
    }

    public void LevelTransition()
    {
        // Open Door to Corridor if Object is not close to door
        
        // When player in Corridor, Remove Room

        // When player is in SIDE2 Corridor, Place next roomstate (NextState())
    }

    public void CreateRoomWalls()
    {
        // Door Room Walls
        /*GameObject DoorWall1 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        DoorWall1.transform.localPosition = new Vector3(1.625f, 4f, -1625f / 2f);
        GameObject DoorWall2 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        DoorWall2.transform.localPosition = new Vector3(1.625f, 4f, 1625f / 2f);
        GameObject DoorWall3 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        DoorWall3.transform.localPosition = new Vector3(-1.625f, 4f, 1625f / 2f);
        GameObject DoorWall4 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        DoorWall4.transform.localPosition = new Vector3(-1.625f, 4f, -1625f / 2f);
        DoorWalls.Add(DoorWall1);
        DoorWalls.Add(DoorWall2);
        DoorWalls.Add(DoorWall3);
        DoorWalls.Add(DoorWall4); */
        
        // Simple Room Walls
        WallArray[0,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[0,0].transform.localPosition = new Vector3(1.625f, 4f, 1.625f / 2f);
        WallArray[0,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[0,1].transform.localPosition = new Vector3(1.625f, 4f, -1.625f / 2f);
        WallArray[1,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[1,0].transform.localPosition = new Vector3(1.625f / 2f, 4f, 1.625f);
        WallArray[1,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[1,1].transform.localPosition = new Vector3(1.625f / 2f, 4f, -1.625f);
        WallArray[2,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[2,0].transform.localPosition = new Vector3(-1.625f / 2f, 4f, 1.625f);
        WallArray[2,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[2,1].transform.localPosition = new Vector3(-1.625f / 2f, 4f, -1.625f);
        WallArray[3,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[3,0].transform.localPosition = new Vector3(-1.625f, 4f, 1.625f / 2f);
        WallArray[3,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[3,1].transform.localPosition = new Vector3(-1.625f, 4f, -1.625f / 2f);

        foreach (var doorWall in DoorWalls)
        {
            doorWall.SetActive(false);
        }

        foreach (var wall in WallArray)
        {
            wall.transform.localScale = _RoomWallPrefab.transform.localScale;
            wall.SetActive(false);
        }
    }
}
