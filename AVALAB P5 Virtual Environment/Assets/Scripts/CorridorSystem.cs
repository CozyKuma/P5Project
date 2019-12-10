using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorSystem : MonoBehaviour
{
    public int oldEntrancePosition = 0;
    public int entrancePosition = 4;

    public int exitQuad;

    public GameObject corrPrefab;
    public static QuadrantCalc QuadrantCalculator;
    public WayPointGrid WayPointSystem;
    public int physObjQuadrant;
    public float minXScale = 1f;
    public float minYScale = 0.5f;
    public float minZScale = 1f;

    public class Corridor
    {
        public enum corridorType { DEFAULT = 0, EXIT, ENTRANCE, SIDE, BRIDGE }
        public enum lastPosition { NONE, LEFT, RIGHT }
        private static lastPosition lastCorrPosition = lastPosition.NONE;
        public static Vector3 lastPosVector;

        public corridorType type;
        public Vector3 size;
        public static Vector3 minSize;
        public bool isActive = true;
        public Vector3 center;
        public static List<Corridor> listOfCorridors = new List<Corridor>();
        public Vector3 startPoint;
        public Vector3 endPoint;
        public GameObject parentObject;
        public GameObject prefabObject;

        public Corridor(GameObject parent, corridorType corrType = corridorType.DEFAULT)
        {
            this.parentObject = parent;
            this.type = corrType;
            this.CalculateCorridor();
            listOfCorridors.Add(this);
        }

        public Corridor(GameObject start, GameObject end, corridorType corrType = corridorType.DEFAULT)
        {
            this.parentObject = null;
            this.startPoint = start.transform.position;
            this.endPoint = end.transform.position;
            this.type = corrType;
            this.CalculateCorridor();
            listOfCorridors.Add(this);
        }

        public Corridor(Vector3 start, Vector3 end, corridorType corrType = corridorType.DEFAULT)
        {
            this.parentObject = null;
            this.startPoint = start;
            this.endPoint = end;
            this.type = corrType;
            this.CalculateCorridor();
            listOfCorridors.Add(this);
        }

        private void CalculateCorridor()
        {
            /*if (type == corridorType.BRIDGE)
            {
                var bridgePositions = CalculateBridge();
                startPoint = bridgePositions.Item1;
                endPoint = bridgePositions.Item2;
            }*/
            /*else if (type == corridorType.SIDE)
            {
                var sidePositions = CalculateSide();
                startPoint = sidePositions.Item1;
                endPoint = sidePositions.Item2;
            }*/

            center = Vector3.Lerp(startPoint, endPoint, 0.5f);
            Vector3 tempSize = new Vector3();

            if (endPoint.x - startPoint.x == 0)
            {
                tempSize.x = Corridor.minSize.x;
            }
            else
            {
                tempSize.x = endPoint.x - startPoint.x;
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

            if (endPoint.z - startPoint.z == 0)
            {
                tempSize.z = Corridor.minSize.z;
            }
            else
            {
                tempSize.z = endPoint.z - startPoint.z;
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

        public static (Vector3, Vector3) CalculateBridge()
        {
            Vector3[] bridgeWaypoints = new Vector3[2];
            int tempQuad = 0;
            int physObjQuad = QuadrantCalc.Quadrant.WithinWhichQuadrant(QuadrantCalculator.physObject);
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                if (physObjQuad == 1 || physObjQuad == 2)
                {
                    bridgeWaypoints[0] = GetQuadrantBasedPosition(4, "vertical");
                    tempQuad = 4;
                }
                else if (physObjQuad == 3 || physObjQuad == 4)
                {
                    bridgeWaypoints[0] = GetQuadrantBasedPosition(1, "vertical");
                    tempQuad = 1;
                }
            } else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                if (physObjQuad == 1 || physObjQuad == 2)
                {
                    bridgeWaypoints[0] = GetQuadrantBasedPosition(3, "vertical");
                    tempQuad = 3;
                } else if (physObjQuad == 3 || physObjQuad == 4)
                {
                    bridgeWaypoints[0] = GetQuadrantBasedPosition(2, "vertical");
                }
            }
            bridgeWaypoints[1] = GetQuadrantBasedPosition(QuadrantCalc.Quadrant.FindNeighbour(tempQuad, "horizontal"), "vertical");

            return (bridgeWaypoints[0], bridgeWaypoints[1]);
        }

        public static (Vector3, Vector3) CalculateSide()
        {
            Vector3[] sideWaypoints = new Vector3[2];
            int physObjQuad = QuadrantCalc.Quadrant.WithinWhichQuadrant(QuadrantCalculator.physObject);
            sideWaypoints[0] = Corridor.lastPosVector;
            if (Corridor.getLastPosition() == Corridor.lastPosition.RIGHT)
            {
                if (Corridor.lastPosVector == GetQuadrantBasedCorner(1))
                {
                    if (physObjQuad == 1 || physObjQuad == 2)
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(4, "vertical");
                    }
                    else
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(1, "vertical");
                    }
                }
                else if (Corridor.lastPosVector == GetQuadrantBasedCorner(4))
                {
                    if (physObjQuad == 1 || physObjQuad == 2)
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(4, "vertical");
                    }
                    else
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(1, "vertical");
                    }
                }
                else if (Corridor.lastPosVector == GetQuadrantBasedPosition(4, "vertical"))
                {
                    sideWaypoints[1] = GetQuadrantBasedCorner(1);
                }
                else if (Corridor.lastPosVector == GetQuadrantBasedPosition(1, "vertical"))
                {
                    sideWaypoints[1] = GetQuadrantBasedCorner(4);
                }
            }
            else if (Corridor.getLastPosition() == Corridor.lastPosition.LEFT)
            {
                if (Corridor.lastPosVector == GetQuadrantBasedCorner(2))
                {
                    if (physObjQuad == 1 || physObjQuad == 2)
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(3, "vertical");
                    }
                    else
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(2, "vertical");
                    }
                }
                else if (Corridor.lastPosVector == GetQuadrantBasedCorner(3))
                {
                    if (physObjQuad == 1 || physObjQuad == 2)
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(3, "vertical");
                    }
                    else
                    {
                        sideWaypoints[1] = GetQuadrantBasedPosition(2, "vertical");
                    }
                }
                else if (Corridor.lastPosVector == GetQuadrantBasedPosition(3, "vertical"))
                {
                    sideWaypoints[1] = GetQuadrantBasedCorner(2);
                }
                else if (Corridor.lastPosVector == GetQuadrantBasedPosition(2, "vertical"))
                {
                    sideWaypoints[1] = GetQuadrantBasedCorner(3);
                }
            }
            return (sideWaypoints[0], sideWaypoints[1]);
        }

        public void ReCalculatePrefab()
        {
            CalculateCorridor();
            prefabObject.transform.localScale = this.size;
            prefabObject.transform.position = this.center;
        }

        public static void ReCalculateAllPrefabs()
        {
            foreach (Corridor corr in listOfCorridors)
            {
                corr.ReCalculatePrefab();
            }
        }

        public static lastPosition getLastPosition()
        {
            return lastCorrPosition;
        }

        public static void setLastPosition(lastPosition lastPos)
        {
            lastCorrPosition = lastPos;
        }

        public static void CreateCorridorBetween(GameObject start, GameObject end, GameObject prefab, corridorType type)
        {
            Corridor tempCorr = new Corridor(start, end, type);
            GameObject tempObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempObject.transform.localScale = tempCorr.size;
            tempObject.transform.position = tempCorr.center;
            tempCorr.prefabObject = tempObject;
            Corridor.lastPosVector = tempCorr.endPoint;
        }

        public static void CreateCorridorBetween(Vector3 start, Vector3 end, GameObject prefab, corridorType type)
        {
            Corridor tempCorr = new Corridor(start, end, type);
            GameObject tempObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempObject.transform.localScale = tempCorr.size;
            tempObject.transform.position = tempCorr.center;
            tempCorr.prefabObject = tempObject;
            Corridor.lastPosVector = tempCorr.endPoint;
        }

        public static void DestroyAllOfType(corridorType type)
        {
            List<Corridor> instancesToRemove = new List<Corridor>();
            foreach (Corridor corr in Corridor.listOfCorridors)
            {
                if (corr.type == type)
                {
                    corr.prefabObject.SetActive(false);
                    Destroy(corr.prefabObject);
                    instancesToRemove.Add(corr);
                }
            }

            foreach (Corridor removeCorr in instancesToRemove)
            {
                Corridor.listOfCorridors.Remove(removeCorr);
            }

            instancesToRemove.Clear();
        }

        public static void ChangeActiveAllOfType(corridorType type)
        {
            foreach (Corridor corr in Corridor.listOfCorridors)
            {
                if (corr.type == type)
                {
                    if (corr.isActive)
                    {
                        corr.prefabObject.SetActive(false);
                        corr.isActive = false;
                    }
                    else if (!corr.isActive)
                    {
                        corr.prefabObject.SetActive(true);
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
        Corridor.minSize = new Vector3(minXScale, minYScale, minZScale);

        GenerateInitialSetup();

        Debug.Log("Corridor Script - Start Done!");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            Corridor.ReCalculateAllPrefabs();
        }
        else if (Input.GetKeyDown("b"))
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
        } else if (Input.GetKeyDown("c"))
        {
            Corridor.DestroyAllOfType(Corridor.corridorType.BRIDGE);
            Corridor.DestroyAllOfType(Corridor.corridorType.DEFAULT);
            Corridor.DestroyAllOfType(Corridor.corridorType.SIDE);
        }
    }

    public void GenerateInitialSetup()
    {
        Corridor.CreateCorridorBetween(GetQuadrantBasedPosition(4), WayPointGrid.WayPoint.getSpecificWaypoint(5, 5).position, corrPrefab, Corridor.corridorType.ENTRANCE);
        entrancePosition = 4;
        GenerateExit();
    }

    public void GenerateExit()
    {
        exitQuad = QuadrantCalc.Quadrant.FindOpposite(entrancePosition); // Place exit at the diagonal opposite of the entrance.
        Corridor.CreateCorridorBetween(GetQuadrantBasedPosition(exitQuad), GetQuadrantBasedCorner(QuadrantCalc.Quadrant.FindNeighbour(exitQuad)), corrPrefab, Corridor.corridorType.EXIT);
        Debug.Log(Corridor.getLastPosition());
    }

    public void GenerateBridge()
    {
        Corridor.DestroyAllOfType(Corridor.corridorType.ENTRANCE);
        Corridor.DestroyAllOfType(Corridor.corridorType.SIDE);
        Corridor.DestroyAllOfType(Corridor.corridorType.BRIDGE);
        Corridor.DestroyAllOfType(Corridor.corridorType.DEFAULT);
        var sidePoints = Corridor.CalculateSide();
        Corridor.CreateCorridorBetween(sidePoints.Item1, sidePoints.Item2, corrPrefab, Corridor.corridorType.SIDE);
        var bridgePoints = Corridor.CalculateBridge();
        Corridor.CreateCorridorBetween(Corridor.lastPosVector, bridgePoints.Item2, corrPrefab, Corridor.corridorType.BRIDGE);
        Debug.Log(Corridor.lastPosVector);
        Debug.Log(Corridor.getLastPosition());
    }

    public void GenerateEntrance()
    {
        int objQuadrantInt = QuadrantCalc.Quadrant.WithinWhichQuadrant(QuadrantCalculator.physObject);
        var sidePoints = Corridor.CalculateSide();
        Corridor.CreateCorridorBetween(sidePoints.Item1, sidePoints.Item2, corrPrefab, Corridor.corridorType.SIDE);

        Corridor.CreateCorridorBetween(Corridor.lastPosVector, GetQuadrantBasedPosition(objQuadrantInt), corrPrefab, Corridor.corridorType.ENTRANCE);
        entrancePosition = objQuadrantInt;
        Corridor.ChangeActiveAllOfType(Corridor.corridorType.EXIT);
    }

    public static Vector3 GetQuadrantBasedPosition(int quadId, string direction = "horizontal")
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

    public static Vector3 GetQuadrantBasedCorner(int quadId)
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
}
