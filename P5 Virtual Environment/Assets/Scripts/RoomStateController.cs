using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateController : MonoBehaviour
{

    public delegate void RoomSpawnDelegate();
    public event RoomSpawnDelegate onRoomActivateEvent;
    
    public enum State { RoomState0, RoomState1, RoomState2, RoomState3 }

    private CorridorSystemV2 CorridorSystem;
    private QuadrantCalc QuandrantCalculator;

    private static State oldState = State.RoomState0;
    private static State currentState = State.RoomState0;

    [SerializeField]
    public GameObject room0, room1, room2, room3;

    private List<GameObject> DoorWalls = new List<GameObject>();
    private GameObject[,] WallArray = new GameObject[4,2];

    [SerializeField] private GameObject _DoorWallPrefab, _RoomWallPrefab;
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private GameObject globalFloor;
    [SerializeField] private GameObject CorridorRoofHole;
    [SerializeField] private GameObject CorridorRoofFull;

    [SerializeField] private bool levelComplete = false;
    

    // Start is called before the first frame update
    void Start()
    {
        parentGameObject = GameObject.Find("ScaleContainer");
        QuandrantCalculator = GameObject.Find("RoomParent").GetComponent<QuadrantCalc>();

        CreateRoomWalls();

        ActivateRoom(getCurrentState());
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
                CorridorRoofHole.SetActive(true);
            }
        } else if (Input.GetKeyDown("x"))
        {
            levelComplete = true;
        }
    }

    public void ActivateRoom(State roomToActivate)
    {
        oldState = currentState;
        GameObject roomObj = roomToActivate == State.RoomState0 ? room0 : roomToActivate == State.RoomState1 ? room1 :
            roomToActivate == State.RoomState2 ? room2 : room3;
        roomObj.SetActive(true);
        currentState = roomToActivate;
        if (currentState == State.RoomState0) return;
        ActivateRoomWalls();
        CorridorRoofFull.SetActive(false);
        CorridorRoofHole.SetActive(true);
        QuandrantCalculator.physObject = GameObject.FindWithTag("TrackedObject");
        globalFloor.SetActive(true);
        if (onRoomActivateEvent != null)
        {
            onRoomActivateEvent();
            Debug.Log("On Room Activate Event Called");
        }
        else
        {
            Debug.Log("No Event Called - Null");
        }
    }

    public void DeactivateRoom(State roomToDeactivate)
    {
        GameObject roomObj = roomToDeactivate == State.RoomState0 ? room0 : roomToDeactivate == State.RoomState1 ? room1 :
            roomToDeactivate == State.RoomState2 ? room2 : room3;
        roomObj.SetActive(false);
        DeactivateRoomWalls();
        CorridorRoofHole.SetActive(false);
        CorridorRoofFull.SetActive(true);
    }

    public void ChangeState(State newState)
    {
        DeactivateRoom(oldState);
        ActivateRoom(newState);
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

    public bool GetLevelComplete()
    {
        return levelComplete;
    }

    public void SetLevelComplete()
    {
        levelComplete = !levelComplete;
    }

    public void SetLevelComplete(bool myBool)
    {
        levelComplete = myBool;
    }

    public void CreateRoomWalls()
    {
        // Door Room Walls
        GameObject DoorWall1 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        //DoorWall1.transform.parent = parentGameObject.transform;
        DoorWall1.transform.localPosition = new Vector3(1.625f, 4f, -1.75f / 2f);
        GameObject DoorWall2 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        //DoorWall2.transform.parent = parentGameObject.transform;
        DoorWall2.transform.localPosition = new Vector3(1.625f, 4f, 1.75f / 2f);
        GameObject DoorWall3 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        //DoorWall3.transform.parent = parentGameObject.transform;
        DoorWall3.transform.localPosition = new Vector3(-1.625f, 4f, 1.75f / 2f);
        GameObject DoorWall4 = Instantiate(_DoorWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        //DoorWall4.transform.parent = parentGameObject.transform;
        DoorWall4.transform.localPosition = new Vector3(-1.625f, 4f, -1.75f / 2f);
        DoorWalls.Add(DoorWall1);
        DoorWalls.Add(DoorWall2);
        DoorWalls.Add(DoorWall3);
        DoorWalls.Add(DoorWall4);
        
        // Simple Room Walls
        WallArray[0,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[0,0].transform.localPosition = new Vector3(1.625f, 4f, 1.75f / 2f);
        WallArray[0,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[0,1].transform.localPosition = new Vector3(1.625f, 4f, -1.75f / 2f);
        WallArray[1,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[1,0].transform.localPosition = new Vector3(1.75f / 2f, 4f, 1.625f);
        WallArray[1,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[1,1].transform.localPosition = new Vector3(1.75f / 2f, 4f, -1.625f);
        WallArray[2,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[2,0].transform.localPosition = new Vector3(-1.75f / 2f, 4f, 1.625f);
        WallArray[2,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.Euler(0, 90, 0), parentGameObject.transform);
        WallArray[2,1].transform.localPosition = new Vector3(-1.75f / 2f, 4f, -1.625f);
        WallArray[3,0] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[3,0].transform.localPosition = new Vector3(-1.625f, 4f, 1.75f / 2f);
        WallArray[3,1] = Instantiate(_RoomWallPrefab, Vector3.zero, Quaternion.identity, parentGameObject.transform);
        WallArray[3,1].transform.localPosition = new Vector3(-1.625f, 4f, -1.75f / 2f);

        foreach (var doorWall in DoorWalls)
        {
            doorWall.SetActive(false);
        }

        foreach (var wall in WallArray)
        {
            wall.SetActive(false);
        }
    }
    
    public void DeactivateRoomWalls()
    {
        foreach (var doorWall in DoorWalls)
        {
            doorWall.SetActive(false);
        }

        foreach (var wall in WallArray)
        {
            wall.SetActive(false);
        }
    }

    public void ActivateRoomWalls()
    {
        foreach (GameObject wall in WallArray)
        {
            wall.SetActive(true);
        }

        switch (CorridorSystem.exitQuad)
        {
            case 1:
                DoorWalls[0].SetActive(true);
                WallArray[0,1].SetActive(false);
                break;
            case 2:
                DoorWalls[1].SetActive(true);
                WallArray[0,0].SetActive(false);
                break;
            case 3:
                DoorWalls[2].SetActive(true);
                WallArray[3,0].SetActive(false);
                break;
            case 4:
                DoorWalls[3].SetActive(true);
                WallArray[3,1].SetActive(false);
                break;
        }
        
        switch (CorridorSystem.entranceQuad)
        {
            case 1:
                DoorWalls[0].SetActive(true);
                WallArray[0,1].SetActive(false);
                break;
            case 2:
                DoorWalls[1].SetActive(true);
                WallArray[0,0].SetActive(false);
                break;
            case 3:
                DoorWalls[2].SetActive(true);
                WallArray[3,0].SetActive(false);
                break;
            case 4:
                DoorWalls[3].SetActive(true);
                WallArray[3,1].SetActive(false);
                break;
        }
    }
}
