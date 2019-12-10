using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRoomController : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> roomGameObjects = new List<GameObject>();
    public List<GameObject> wallGameObjects = new List<GameObject>();

    public class Room
    {
        public List<GameObject> roomObjects = new List<GameObject>();
        public GameObject parentObject;
        private GameObject trigger;
        private static List<Room> roomsList = new List<Room>();
        public bool roomIsActive = false;
        private int roomNum;
        static int numberOfRooms = 0;


        public Room(GameObject parent)
        {
            numberOfRooms += 1;
            this.roomNum = numberOfRooms;
            this.parentObject = parent;
            this.FindChildrenObjects(); // sets children and trigger objects
            roomsList.Add(this);
            //Debug.Log(string.Format("Room number {0} created.", this.roomNum));
        }
        
        public void ActivateObjects()
        {
            roomIsActive = true;
            foreach (GameObject x in roomObjects)
            {
                x.SetActive(true);
            }
        }

        public void DeactivateObjects()
        {
            roomIsActive = false;
            foreach (GameObject x in roomObjects)
            {
                x.SetActive(false);
            }
        }

        public GameObject GetTrigger()
        {
            return trigger;
        }

        public int GetroomNum()
        {
            return roomNum;
        }

        private void FindChildrenObjects()
        {
            Transform parentComponent = parentObject.GetComponent<Transform>();
            //Debug.Log(string.Format("Parent Object - {0}", parentComponent.gameObject.name));
            Transform[] allChildren;
            allChildren = parentComponent.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.gameObject.name != "Trigger" && child != allChildren[0])
                {
                    roomObjects.Add(child.gameObject);
                    //Debug.Log(string.Format("{0} added in Room {1}", child.gameObject.name, parentComponent.gameObject));
                } else if (child.gameObject.name == "Trigger") {
                    trigger = child.gameObject;
                    //Debug.Log("Trigger Added");
                }
            }
        }

        public static List<Room> GetRoomList()
        {
            return roomsList;
        }

        public static void FlipRooms(Room activeRoom)
        {
            foreach (Room room in roomsList)
            {
                if (room.Equals(activeRoom)) {
                    room.ActivateObjects();
                } else
                {
                    room.DeactivateObjects();
                }
            }
        }
    }

    public class Wall
    {
        private List<Vector3> Wallposition = new List<Vector3>();
        private List<Vector3> Wallorientation = new List<Vector3>();
        private static List<Wall> wallList = new List<Wall>();
        private GameObject wallObject;

        public Wall(List<Vector3> wallPositions, List<Vector3> wallOrientations, GameObject wallObject)
        {
            this.Wallposition = wallPositions;
            this.Wallorientation = wallOrientations;
            this.wallObject = wallObject;
            wallList.Add(this);
        }

        public Vector3 getPositionForState(int state)
        {
            return Wallposition[state];
        }

        public Vector3 getOrientationForState(int state)
        {
            return Wallorientation[state];
        }

        public static void changeState(int state)
        {
            foreach (Wall wall in wallList)
            {
                Debug.Log(string.Format("Positions: {0}", wall.getPositionForState(state)));
                wall.wallObject.transform.localPosition = wall.getPositionForState(state);

                //wall.wallObject.transform.eulerAngles = wall.getOrientationForState(state);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //roomGameObjects.Add(GameObject.Find("Room1"));
        //roomGameObjects.Add(GameObject.Find("Room2"));

        foreach (GameObject room in roomGameObjects)
        {
            new Room(room);
        }

        foreach (GameObject wall in wallGameObjects)
        {
            List<Vector3> positions = new List<Vector3> { new Vector3(-0.133f, 0.5f, -0.35f), new Vector3(-0.133f, 0.5f, 0.35f) };
            List<Vector3> orientations = new List<Vector3> { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) };
            new Wall(positions, orientations, wall);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Room FindTrigger(GameObject trigger)
    {
        Room result = null;
        List<Room> roomsList = Room.GetRoomList();
        
        foreach (Room room in roomsList)
        {
            if(room.GetTrigger().Equals(trigger))
            {
                result = room; // returns the room corresponding to the trigger
                return result;
            }
        }

        return result;
    }
}
