using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CorridorSystemV2 : MonoBehaviour
{
    public int entranceQuad = 4;
    public int exitQuad = 2;
    public int physObjQuad;
    private static List<Vector3> listOfPoints = new List<Vector3>();

    [SerializeField]
    private GameObject serializeHelperPrefabFloor;
    [SerializeField]
    private GameObject serializeHelperPrefabWall;
    [SerializeField]
    private GameObject serializeHelperPrefabCorner;
    private static GameObject _floorPrefab;
    private static GameObject _wallPrefab;
    private static GameObject _cornerPrefab;
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
        public static GameObject CorridorContainer { get; set; }
        public static Vector3 CorridorScale;

        // Variables
        private typeOfCorridor type;
        private Vector3 size;
        private Vector3 center;
        private Vector3 start;
        private Vector3 end;
        private bool isActive = true;
        private GameObject floorObject;
        private List<GameObject> cornerObjects = new List<GameObject>();
        private List<GameObject> wallObjects = new List<GameObject>();

        public Corridor(Vector3 start, Vector3 end, typeOfCorridor corrType = typeOfCorridor.DEFAULT, bool standardCorridor = true)
        {
            type = corrType;
            this.start = start;
            this.end = end;
            CalculateCorridor(standardCorridor);
            listOfCorridors.Add(this);
        }

        public static void SetLastPosition(lastPosition pos)
        {
            lastPos = pos;
        }

        public static lastPosition GetLastPosition()
        {
            return lastPos;
        }

        public static List<Corridor> GetListOfCorridors()
        {
            return listOfCorridors;
        }

        public GameObject GetFloorObject()
        {
            return floorObject;
        }

        private void CalculateCorridor(bool standardCorridor = true)
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
                    tempSize.x -= (1.0f);
                }
                else if (tempSize.x > 0)
                {
                    tempSize.x += (1.0f);
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
                    tempSize.z -= (1.0f);
                }
                else if (tempSize.z > 0)
                {
                    tempSize.z += (1.0f);
                }
            }
            size = new Vector3(Mathf.Abs(tempSize.x), tempSize.y, Mathf.Abs(tempSize.z));
            if (standardCorridor)
            {
                size.x = size.x > size.z ? size.x -= 2f : size.x;
                size.z = size.x < size.z ? size.z -= 2f : size.z;
                CreateWalls();
                CreateCorner();
                if (type == typeOfCorridor.EXIT)
                {
                    string cornerType = type == typeOfCorridor.EXIT ? "exit" :
                        type == typeOfCorridor.ENTRANCE ? "entrance" : "standard";
                    CreateCorner(cornerType);
                }
            }
            else
            {
                size.x = size.x > size.z ? size.x -= 1f : size.x;
                size.z = size.x < size.z ? size.z -= 1f : size.z;
                center.z -= 0.5f;
            }
        }

        private void SetFloorMaterial()
        {
            
            Material tempRenderer = floorObject.GetComponent<Renderer>().material;
            tempRenderer.mainTextureScale = new Vector2(2 * size.x, 2 * size.z);
        }

        private void SetWallMaterial()
        {
            foreach (GameObject wall in wallObjects)
            {
                Material tempRenderer = wall.GetComponent<Renderer>().material;
                tempRenderer.mainTextureScale = new Vector2(Mathf.Max(2 * wall.transform.localScale.z - 2 * CorridorScale.z, 1), 4);
            }
        }

        public void CreateWalls()
        {
            float offset = 0f;
            for(int i = 0; i < 2; i++)
            {
                offset = i == 0 ? (Corridor.minSize.x / 2f) : ((Corridor.minSize.x / 2f) * -1f);
                GameObject tempWall = Instantiate(CorridorSystemV2._wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                tempWall.transform.parent = CorridorContainer.transform;
                Vector3 tempSize = new Vector3(0, 0, 0);
                Vector3 tempPosition = new Vector3(0, 0, 0);
                if (start.z == end.z)
                {
                    tempSize = new Vector3(tempWall.transform.localScale.x * CorridorScale.x, tempWall.transform.localScale.y * CorridorScale.y, this.size.x);
                    tempWall.transform.Rotate(0, 90, 0);
                    tempPosition = new Vector3(this.center.x, tempSize.y / 2, this.center.z + offset);
                }
                else if (start.x == end.x)
                {
                    tempSize = new Vector3(tempWall.transform.localScale.x * CorridorScale.x, tempWall.transform.localScale.y * CorridorScale.y, this.size.z);
                    tempPosition = new Vector3(this.center.x + offset, tempSize.y / 2, this.center.z);
                }
                tempWall.transform.localScale = tempSize;
                tempWall.transform.localPosition = tempPosition;
                this.wallObjects.Add(tempWall);
                this.SetWallMaterial();
            }
        }

        public void CreateSingleWall(Vector3 pos, Vector3 rotation)
        {
            GameObject tempWall = Instantiate(_wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempWall.transform.parent = CorridorContainer.transform;
            tempWall.transform.localScale = new Vector3(tempWall.transform.localScale.x * CorridorScale.x, tempWall.transform.localScale.y * CorridorScale.y, tempWall.transform.localScale.z * CorridorScale.y);
            tempWall.transform.localPosition = pos;
            tempWall.transform.Rotate(rotation.x, rotation.y, rotation.z);
            wallObjects.Add(tempWall);
            SetWallMaterial();
        }

        public void CreateCorner(string type = "standard")
        {
            GameObject tempCorner = Instantiate(_cornerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempCorner.transform.parent = CorridorContainer.transform;
            Vector3 oppositeEnd = new Vector3();
            if (type == "standard" || type == "entrance")
            {
                tempCorner.transform.localPosition = end;
                oppositeEnd = start;
            } else if (type == "exit")
            {
                tempCorner.transform.localPosition = start;
                oppositeEnd = end;
            }
            Vector3 orientation = new Vector3(0, 0, 0);
            bool endAboveOrigin = (tempCorner.transform.localPosition.x > 0);
            if (oppositeEnd.x == tempCorner.transform.localPosition.x && oppositeEnd.z > tempCorner.transform.localPosition.z && endAboveOrigin) {
                orientation = new Vector3(0, 0, 0);
            } else if (oppositeEnd.x == tempCorner.transform.localPosition.x && oppositeEnd.z > tempCorner.transform.localPosition.z && !endAboveOrigin)
            {
                orientation = new Vector3(0, 90, 0);
            } else if (oppositeEnd.x == tempCorner.transform.localPosition.x && oppositeEnd.z < tempCorner.transform.localPosition.z && endAboveOrigin)
            {
                orientation = new Vector3(0, -90, 0);
            } else if (oppositeEnd.x == tempCorner.transform.localPosition.x && oppositeEnd.z < tempCorner.transform.localPosition.z && !endAboveOrigin)
            {
                orientation = new Vector3(0, 180, 0);
            } else if (oppositeEnd.x < tempCorner.transform.localPosition.x && oppositeEnd.z == tempCorner.transform.localPosition.z && lastPosition.LEFT == lastPos)
            {
                orientation = new Vector3(0, -90, 0);
            } else if (oppositeEnd.x < tempCorner.transform.localPosition.x && oppositeEnd.z == tempCorner.transform.localPosition.z && lastPosition.RIGHT == lastPos)
            {
                orientation = new Vector3(0, 0, 0);
            } else if (oppositeEnd.x > tempCorner.transform.localPosition.x && oppositeEnd.z == tempCorner.transform.localPosition.z && lastPosition.LEFT == lastPos)
            {
                orientation = new Vector3(0, 180, 0);
            } else if (oppositeEnd.x > tempCorner.transform.localPosition.x && oppositeEnd.z == tempCorner.transform.localPosition.z && lastPosition.RIGHT == lastPos)
            {
                orientation = new Vector3(0, 90, 0);
            }
            tempCorner.transform.Rotate(orientation);
            tempCorner.transform.localScale = new Vector3(minSize.x, tempCorner.transform.localScale.y * CorridorScale.y, minSize.z);
            cornerObjects.Add(tempCorner);
        }

        public static void CreateCorridorBetween(Vector3 start, Vector3 end, typeOfCorridor type, bool standardCorridor = true) // Instantiates the prefab of a specific size to work as a corridor.
        {
            Corridor tempCorr = new Corridor(start, end, type, standardCorridor);
            GameObject tempObject = Instantiate(_floorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempObject.transform.parent = CorridorContainer.transform;
            tempObject.transform.localScale = tempCorr.size;
            tempObject.transform.localPosition = tempCorr.center;
            tempCorr.floorObject = tempObject;
            tempCorr.SetFloorMaterial();
            listOfPoints.Add(tempCorr.end);
        }

        public static void DestroyAllOfType(typeOfCorridor type) // Hides AND Destroys corridor objects of given type
        {
            List<Corridor> instancesToRemove = new List<Corridor>();
            foreach (Corridor corr in Corridor.listOfCorridors)
            {
                if (corr.type == type)
                {
                    corr.floorObject.SetActive(false);
                    Destroy(corr.floorObject);
                    foreach (GameObject wall in corr.wallObjects)
                    {
                        wall.SetActive(false);
                        Destroy(wall);
                    }

                    foreach (GameObject corner in corr.cornerObjects)
                    {
                        corner.SetActive(false);
                        Destroy(corner);
                    }
                    instancesToRemove.Add(corr);
                }
            }

            foreach (Corridor removeCorr in instancesToRemove)
            {
                listOfCorridors.Remove(removeCorr);
            }

            instancesToRemove.Clear();
        }

        public static void ChangeActiveAllOfType(typeOfCorridor type) // Deactivates/Hides corridor objects of given type
        {
            foreach (Corridor corr in listOfCorridors)
            {
                if (corr.type == type)
                {
                    if (corr.isActive)
                    {
                        corr.floorObject.SetActive(false);
                        corr.isActive = false;
                        foreach (GameObject wall in corr.wallObjects)
                        {
                            wall.SetActive(false);
                        }

                        foreach (GameObject corner in corr.cornerObjects)
                        {
                            corner.SetActive(false);
                        }
                    }
                    else if (!corr.isActive)
                    {
                        corr.floorObject.SetActive(true);
                        corr.isActive = true;
                        foreach (GameObject wall in corr.wallObjects)
                        {
                            wall.SetActive(true);
                        }

                        foreach (GameObject corner in corr.cornerObjects)
                        {
                            corner.SetActive(true);
                        }
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
        Corridor.CorridorContainer = gameObject;
        Corridor.CorridorScale = Corridor.CorridorContainer.transform.localScale;
        Corridor.MinSize = new Vector3(minXScale, minYScale, minZScale);

        // Set static variable to something from editor
        OnAfterDeserialize();

        // Find which Quadrant the object is within
        physObjQuad = UpdatePhysObjQuad();

        // Generate the Initial entrance corridor - this will be deleted when the first "bridge" is created.
        Corridor.CreateCorridorBetween(WayPointGrid.WayPoint.getSpecificWaypoint(5, 5).position, GetQuadrantBasedPosition(4), Corridor.typeOfCorridor.ENTRANCE, false);
        Corridor initCorr = Corridor.GetListOfCorridors().Last();
        initCorr.CreateWalls();
        initCorr.CreateSingleWall(new Vector3(WayPointGrid.WayPoint.getSpecificWaypoint(5, 5).position.x, 1, WayPointGrid.WayPoint.getSpecificWaypoint(5,5).position.z - 0.5f), new Vector3(0, 90, 0));
        initCorr.CreateCorner("entrance");
        
        
        // Create the point from which to build the initial exit.
        CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(2)); // First exit
        Corridor.SetLastPosition(Corridor.lastPosition.LEFT);

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

        physObjQuad = UpdatePhysObjQuad(); // Used to calculate where the bridge should "cross" the room.

        // From corner to bridge start
        if (physObjQuad == 1 || physObjQuad == 2)
        {
            if (Corridor.GetLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(4, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(4, "vertical"));
            }
            else if (Corridor.GetLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(3, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(3, "vertical"));
            }
        }
        else if (physObjQuad == 3 || physObjQuad == 4)
        {
            if (Corridor.GetLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(1, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(1, "vertical"));
            }
            else if (Corridor.GetLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(2, "vertical"), Corridor.typeOfCorridor.SIDE1);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(2, "vertical"));
            }
        }

        // bridge
        if (physObjQuad == 1 || physObjQuad == 2)
        {
            if (Corridor.GetLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(3, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(3, "vertical"));
            }
            else if (Corridor.GetLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(4, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(4, "vertical"));
            }
        }
        else if (physObjQuad == 3 || physObjQuad == 4)
        {
            if (Corridor.GetLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(2, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(2, "vertical"));
            }
            else if (Corridor.GetLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedPosition(1, "vertical"), Corridor.typeOfCorridor.BRIDGE);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedPosition(1, "vertical"));
            }
        }
    }

    public void GenerateEntrance()
    {
        physObjQuad = UpdatePhysObjQuad();

        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.EXIT);
        Corridor.DestroyAllOfType(Corridor.typeOfCorridor.SIDE1);

        // Side corridor to entrance corner
        if (physObjQuad == 1 || physObjQuad == 2)
        {
            if (Corridor.GetLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(1), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(1));
            }
            else if (Corridor.GetLastPosition() == Corridor.lastPosition.LEFT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(2), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(2));
            }
        }
        else if (physObjQuad == 3 || physObjQuad == 4)
        {
            if (Corridor.GetLastPosition() == Corridor.lastPosition.RIGHT)
            {
                Corridor.CreateCorridorBetween(getLastListElement(listOfPoints), GetQuadrantBasedCorner(4), Corridor.typeOfCorridor.SIDE2);
                CorridorSystemV2.listOfPoints.Add(GetQuadrantBasedCorner(4));
            }
            else if (Corridor.GetLastPosition() == Corridor.lastPosition.LEFT)
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
                    //tempVector = new Vector3(2.5f, 0f, -1f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(0, 3).position, WayPointGrid.WayPoint.getSpecificWaypoint(0, 4).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
                }
                else if (direction == "vertical")
                {
                    //tempVector = new Vector3(1f, 0f, -2.5f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(1, 5).position, WayPointGrid.WayPoint.getSpecificWaypoint(2, 5).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
                }
                break;
            case 2:
                if (direction == "horizontal")
                {
                    //tempVector = new Vector3(2.5f, 0f, 1f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(0, 1).position, WayPointGrid.WayPoint.getSpecificWaypoint(0, 2).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.LEFT);
                }
                else if (direction == "vertical")
                {
                    //tempVector = new Vector3(1f, 0f, 2.5f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(1, 0).position, WayPointGrid.WayPoint.getSpecificWaypoint(2, 0).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.LEFT);
                }
                break;
            case 3:
                if (direction == "horizontal")
                {
                    //tempVector = new Vector3(-2.5f, 0f, 1f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(5, 1).position, WayPointGrid.WayPoint.getSpecificWaypoint(5, 2).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.LEFT);
                }
                else if (direction == "vertical")
                {
                    //tempVector = new Vector3(-1f, 0f, 2.5f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(3, 0).position, WayPointGrid.WayPoint.getSpecificWaypoint(4, 0).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.LEFT);
                }
                break;
            case 4:
                if (direction == "horizontal")
                {
                    //tempVector = new Vector3(-2.5f, 0f, -1f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(5, 3).position, WayPointGrid.WayPoint.getSpecificWaypoint(5, 4).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
                }
                else if (direction == "vertical")
                {
                    //tempVector = new Vector3(-1f, 0f, -2.5f);
                    tempVector = Vector3.Lerp(WayPointGrid.WayPoint.getSpecificWaypoint(3, 5).position, WayPointGrid.WayPoint.getSpecificWaypoint(4, 5).position, 0.5f);
                    Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
                }
                break;
        }
        return tempVector;
    }

    public static Vector3 GetQuadrantBasedCorner(int quadId) // Returns Vector3 based on the corner.
    {
        WayPointGrid.WayPoint tempWayPoint;
        switch (quadId)
        {
            case 1:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(0, 5);
                Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
                break;
            case 2:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(0, 0);
                Corridor.SetLastPosition(Corridor.lastPosition.LEFT);
                break;
            case 3:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(5, 0);
                Corridor.SetLastPosition(Corridor.lastPosition.LEFT);
                break;
            case 4:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(5, 5);
                Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
                break;
            default:
                tempWayPoint = WayPointGrid.WayPoint.getSpecificWaypoint(0, 5);
                Corridor.SetLastPosition(Corridor.lastPosition.RIGHT);
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

    public int UpdatePhysObjQuad()
    {
        return QuadrantCalc.Quadrant.WithinWhichQuadrant(QuadrantCalculator.physObject);
    }

    public void OnAfterDeserialize()
    {
        _floorPrefab = serializeHelperPrefabFloor;
        _wallPrefab = serializeHelperPrefabWall;
        _cornerPrefab = serializeHelperPrefabCorner;
    }
}
