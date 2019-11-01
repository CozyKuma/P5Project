using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorSystemV2 : MonoBehaviour
{
    public int entranceQuad = 4;
    public int exitQuad = 2;
    public int physObjQuad;
    private static List<Vector3> listOfPoints = new List<Vector3>();

    [SerializeField]
    private GameObject serializeHelperPrefab;
    private static GameObject corrPrefab;
    [SerializeField]
    private QuadrantCalc QuadrantCalculator;
    [SerializeField]
    private WayPointGrid WayPointSystem;
    [SerializeField]
    private float minXScale = 1f, minYScale = 0.5f, minZScale = 1f;

    [System.Serializable]
    public class Corridor
    {
        // enums for position and type control
        public enum typeOfCorridor { DEFAULT, EXIT, ENTRANCE, SIDE1, SIDE2, BRIDGE }
        public enum lastPosition { NONE, LEFT, RIGHT }

        // Static Variables
        private static Vector3 lastPosVector;
        private static lastPosition lastPos;
        private static Vector3 minSize;
        public static Vector3 MinSize { get { return minSize; } set { minSize = value; } }
        private static List<Corridor> listOfCorridors = new List<Corridor>();

        // Variables
        private typeOfCorridor type;
        private Vector3 size;
        private Vector3 center;
        private Vector3 start;
        private Vector3 end;
        private bool isActive = true;
        private GameObject obj;

        public Corridor(Vector3 start, Vector3 end, typeOfCorridor corrType = typeOfCorridor.DEFAULT)
        {
            this.type = corrType;
            this.start = start;
            this.end = end;
            this.CalculateCorridor();
            listOfCorridors.Add(this);
        }

        public static void setLastPosition(lastPosition pos)
        {
            lastPos = pos;
        }

        public static lastPosition getLastPosition()
        {
            return lastPos;
        }

        private void CalculateCorridor()
        {
            center = Vector3.Lerp(start, end, 0.5f);
            Vector3 tempSize = new Vector3();

            if (end.x - start.x == 0)
            {
                tempSize.x = Corridor.minSize.x;
            }
            else
            {
                tempSize.x = end.x - start.x;
                if (tempSize.x < 0)
                {
                    tempSize.x -= 1.0f;
                }
                else if (tempSize.x > 0)
                {
                    tempSize.x += 1.0f;
                }
            }

            tempSize.y = 0.5f;

            if (end.z - start.z == 0)
            {
                tempSize.z = Corridor.minSize.z;
            }
            else
            {
                tempSize.z = end.z - start.z;
                if (tempSize.z < 0)
                {
                    tempSize.z -= 1.0f;
                }
                else if (tempSize.z > 0)
                {
                    tempSize.z += 1.0f;
                }
            }

            size = new Vector3(Mathf.Abs(tempSize.x), tempSize.y, Mathf.Abs(tempSize.z));
        }

        public static void CreateCorridorBetween(Vector3 start, Vector3 end, typeOfCorridor type) // Instantiates the prefab of a specific size to work as a corridor.
        {
            Corridor tempCorr = new Corridor(start, end, type);
            GameObject tempObject = Instantiate(CorridorSystemV2.corrPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempObject.transform.localScale = tempCorr.size;
            tempObject.transform.position = tempCorr.center;
            tempCorr.obj = tempObject;
            CorridorSystemV2.listOfPoints.Add(tempCorr.end);
        }

        public static void DestroyAllOfType(typeOfCorridor type) // Hides AND Destroys corridor objects of given type
        {
            List<Corridor> instancesToRemove = new List<Corridor>();
            foreach (Corridor corr in Corridor.listOfCorridors)
            {
                if (corr.type == type)
                {
                    corr.obj.SetActive(false);
                    Destroy(corr.obj);
                    instancesToRemove.Add(corr);
                }
            }

            foreach (Corridor removeCorr in instancesToRemove)
            {
                Corridor.listOfCorridors.Remove(removeCorr);
            }

            instancesToRemove.Clear();
        }

        public static void ChangeActiveAllOfType(typeOfCorridor type) // Deactivates/Hides corridor objects of given type
        {
            foreach (Corridor corr in Corridor.listOfCorridors)
            {
                if (corr.type == type)
                {
                    if (corr.isActive)
                    {
                        corr.obj.SetActive(false);
                        corr.isActive = false;
                    }
                    else if (!corr.isActive)
                    {
                        corr.obj.SetActive(true);
                        corr.isActive = true;
                    }
                }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        QuadrantCalculator = GameObject.Find("RoomParent").GetComponent<QuadrantCalc>();
        WayPointSystem = GameObject.Find("GridContainer").GetComponent<WayPointGrid>();
        Corridor.MinSize = new Vector3(minXScale, minYScale, minZScale);

        // Set static variable to something from editor
        OnAfterDeserialize();

        // Find which Quadrant the object is within
        physObjQuad = updatePhysObjQuad();

        // Generate the Initial entrance corridor - this will be deleted when the first "bridge" is created.
        Corridor.CreateCorridorBetween(GetQuadrantBasedPosition(4), WayPointGrid.WayPoint.getSpecificWaypoint(5, 5).position, Corridor.typeOfCorridor.ENTRANCE);

        // Create the point from which to build the initial exit.
        CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(2)); // First exit
        Corridor.setLastPosition(Corridor.lastPosition.LEFT);

        GenerateExit();
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("b"))
        {
            GenerateBridge();
        }
        else if (Input.GetKeyDown("n"))
        {
            GenerateEntrance();
        }
        else if (Input.GetKeyDown("m"))
        {
            GenerateExit();
        }
        else if (Input.GetKeyDown("c"))
        {
            Corridor.DestroyAllOfType(Corridor.typeOfCorridor.BRIDGE);
            Corridor.DestroyAllOfType(Corridor.typeOfCorridor.DEFAULT);
            Corridor.DestroyAllOfType(Corridor.typeOfCorridor.SIDE1);
        }
    }

    public void GenerateExit()
    {
        exitQuad = QuadrantCalc.Quadrant.FindOpposite(entranceQuad);
        Corridor.CreateCorridorBetween(GetQuadrantBasedPosition(exitQuad, "horizontal"), GetQuadrantBasedCorner(QuadrantCalc.Quadrant.FindNeighbour(exitQuad)), Corridor.typeOfCorridor.EXIT);
        CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(QuadrantCalc.Quadrant.FindNeighbour(exitQuad)));
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.BRIDGE);
    }

    public void GenerateBridge()
    {
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.ENTRANCE);
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.SIDE1);
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.SIDE2);
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.BRIDGE);
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.DEFAULT);

        physObjQuad = updatePhysObjQuad(); // Used to calculate where the bridge should "cross" the room.

        // From corner to bridge start
        if (physObjQuad == 1 || physObjQuad == 2)
        {
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(4, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(4, "vertical"));
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(3, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(3, "vertical"));
            }
        }
        else if (physObjQuad == 3 || physObjQuad == 4)
        {
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(1, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(1, "vertical"));
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(2, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(2, "vertical"));
            }
        }

        // bridge
        if (physObjQuad == 1 || physObjQuad == 2)
        {
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(3, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(3, "vertical"));
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(4, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(4, "vertical"));
            }
        }
        else if (physObjQuad == 3 || physObjQuad == 4)
        {
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(2, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(2, "vertical"));
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(1, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(1, "vertical"));
            }
        }
    }

    public void GenerateEntrance()
    {
        physObjQuad = updatePhysObjQuad();

        Corridor.ChangeActiveAllOfType(Corridor.typeOfCorridor.EXIT);
        Corridor.ChangeActiveAllOfType(Corridor.typeOfCorridor.SIDE1);

        // Side corridor to entrance corner
        if (physObjQuad == 1 || physObjQuad == 2)
        {
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(1), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(1));
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(2), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(2));
            }
        }
        else if (physObjQuad == 3 || physObjQuad == 4)
        {
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(4), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(4));
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(3), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(3));
            }
        }

        // Entrance corridor
        Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(physObjQuad, "horizontal"), Corridor.typeOfCorridor.ENTRANCE);
        CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(physObjQuad, "horizontal"));
        entranceQuad = physObjQuad;
    }

    public static Vector3 GetQuadrantBasedPosition(int quadId, string direction = "horizontal") // Returns Vector3 based on the vertical or horizontal position of a quadrant.
    {
        Vector3 tempVector = new Vector3(0, 0, 0);
        switch (quadId)
        {
            case 1:
                if (direction == "horizontal")
                {
                    tempVector = new Vector3(2.5f, 0f, -1f);
                    Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                }
                else if (direction == "vertical")
                {
                    tempVector = new Vector3(1f, 0f, -2.5f);
                    Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                }
                break;
            case 2:
                if (direction == "horizontal")
                {
                    tempVector = new Vector3(2.5f, 0f, 1f);
                    Corridor.setLastPosition(Corridor.lastPosition.LEFT);
                }
                else if (direction == "vertical")
                {
                    tempVector = new Vector3(1f, 0f, 2.5f);
                    Corridor.setLastPosition(Corridor.lastPosition.LEFT);
                }
                break;
            case 3:
                if (direction == "horizontal")
                {
                    tempVector = new Vector3(-2.5f, 0f, 1f);
                    Corridor.setLastPosition(Corridor.lastPosition.LEFT);
                }
                else if (direction == "vertical")
                {
                    tempVector = new Vector3(-1f, 0f, 2.5f);
                    Corridor.setLastPosition(Corridor.lastPosition.LEFT);
                }
                break;
            case 4:
                if (direction == "horizontal")
                {
                    tempVector = new Vector3(-2.5f, 0f, -1f);
                    Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                }
                else if (direction == "vertical")
                {
                    tempVector = new Vector3(-1f, 0f, -2.5f);
                    Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                }
                break;
        }
        return tempVector;
    }

    public static Vector3 GetQuadrantBasedCorner(int quadId) // Returns Vector3 based on the corner.
    {
        WayPointGrid.WayPoint tempWayPoint = new WayPointGrid.WayPoint();
        switch (quadId)
        {
            case 1:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(0, 5);
                Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                break;
            case 2:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(0, 0);
                Corridor.setLastPosition(Corridor.lastPosition.LEFT);
                break;
            case 3:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(5, 0);
                Corridor.setLastPosition(Corridor.lastPosition.LEFT);
                break;
            case 4:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(5, 5);
                Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                break;
            default:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(0, 5);
                Corridor.setLastPosition(Corridor.lastPosition.RIGHT);
                break;
        }
        return tempWayPoint.position;
    }

    public Vector3 getLastListElement(List<Vector3> myList)
    {
        int size = myList.Count;
        Vector3 myElement = myList[size - 1];
        return myElement;
    }

    public int updatePhysObjQuad()
    {
        return QuadrantCalc.Quadrant.WithinWhichQuadrant(QuadrantCalculator.physObject);
    }

    public void OnAfterDeserialize()
    {
        corrPrefab = serializeHelperPrefab;
    }
}
