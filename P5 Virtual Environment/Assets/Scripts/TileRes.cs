using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRes : MonoBehaviour
{
    public bool tileState;
    
    public int tilesX;
    public int tilesZ;
    public GameObject tile = null;
    public List<GameObject> tileList = new List<GameObject>();
    private List<GameObject> goodTiles = new List<GameObject>();
    private List<GameObject> badTiles = new List<GameObject>();
    private List<GameObject> goodPressedTiles = new List<GameObject>();
    





    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in goodTiles)
        {
            Cubes tempCube = obj.GetComponent<Cubes>();
            bool pressedFromChild = tempCube.pressed;
            if (pressedFromChild && !goodPressedTiles.Contains(obj))
            {
                goodPressedTiles.Add(obj);
            }
        }

        if (goodTiles.Count == goodPressedTiles.Count)
        {
            win();
        }
    }

    void win()
    {
        //Whatever needs to be done
        print("Glerup er en cunt");
    }

}
