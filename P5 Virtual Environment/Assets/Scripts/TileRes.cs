using System.Collections.Generic;
using UnityEngine;
public class TileRes : MonoBehaviour
{
    public bool tileState;
    public int tilesX, tilesZ;
    public GameObject tile = null;
    public List<GameObject> tileList = new List<GameObject>();
    private List<GameObject> goodTiles = new List<GameObject>();
    private List<GameObject> badTiles = new List<GameObject>();
    private List<GameObject> goodPressedTiles = new List<GameObject>();
    void Start()
    {
        foreach (GameObject obj in tileList)
        {
            Cubes tempCube = obj.GetComponent<Cubes>();
            bool goodFromChild = tempCube.good;
            if (goodFromChild)
            {
                goodTiles.Add(obj);
            }
        }
    }
    void Update()
    {
        foreach (GameObject obj in goodTiles)
        {
            Cubes tempCube = obj.GetComponent<Cubes>();
            bool pressedFromChild = tempCube.pressed;
            if (pressedFromChild && !goodPressedTiles.Contains(obj))
            {
                print("Yeah Bitches!");
                goodPressedTiles.Add(obj);
            }
        }
        print("GoodPressedTiles " + goodPressedTiles);
        print("GoodTiles " + goodTiles.Count);
        if (goodTiles.Count == goodPressedTiles.Count)
        {
            //win();
        }
    }
    void win()
    {
        print("Ez win");
    }
}