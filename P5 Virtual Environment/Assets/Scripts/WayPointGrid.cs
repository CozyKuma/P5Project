using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointGrid : MonoBehaviour
{

    public int colSize = 6;
    public int rowSize = 6;
    public List<GameObject> gridList = new List<GameObject>();

    public class WayPoint
    {
        public static GameObject gridParent;
        public static int gridColumnSize;
        public static int gridRowSize;
        public static List<WayPoint> listOfWaypoints = new List<WayPoint>();

        public int gridRowIndex;
        public int gridColIndex;

        public Vector3 position = new Vector3();
        public GameObject gridObj;

        public WayPoint(int rowIndex, int colIndex, GameObject obj)
        {
            this.gridObj = obj;
            this.gridRowIndex = rowIndex;
            this.gridColIndex = colIndex;
            this.position = obj.transform.localPosition;
            WayPoint.listOfWaypoints.Add(this);
        }

        public WayPoint()
        {

        }

        public static WayPoint getSpecificWaypoint(int rowIndex, int colIndex)
        {
            foreach (WayPoint point in listOfWaypoints)
            {
                if (point.gridRowIndex == rowIndex && point.gridColIndex == colIndex)
                {
                    return point; // Found point with corresponding indexes.
                } else
                {
                    // Found nothing.
                }
            }

            return null;
        }

        public static void debugPrintList()
        {
            foreach (WayPoint point in listOfWaypoints)
            {
                Debug.Log(string.Format("{0},{1}", point.gridRowIndex, point.gridColIndex));
            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        if (gridList.Count < 1)
        {
            findChildren();
        }

        WayPoint.gridParent = transform.gameObject;
        WayPoint.gridColumnSize = colSize;
        WayPoint.gridRowSize = rowSize;

        populateWayPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void populateWayPoints()
    {
        int rowCount = 0;
        int colCount = 0;

        foreach (GameObject obj in gridList)
        {
            WayPoint tempHolder = new WayPoint(rowCount, colCount, obj);
            tempHolder.gridObj.name = string.Format("{0}.{1}", rowCount, colCount);
            colCount++;
            if (colCount == 6)
            {
                rowCount++;
                colCount = 0;
            }
        }
    }
    public void findChildren()
    {
        Transform parentComponent = WayPoint.gridParent.GetComponent<Transform>();
        Transform[] allChildren;
        allChildren = parentComponent.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child != allChildren[0])
            {
                gridList.Add(child.gameObject);
            }
        }
    }
}
