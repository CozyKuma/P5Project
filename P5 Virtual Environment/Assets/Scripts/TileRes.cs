﻿using System.Collections.Generic;
using UnityEngine;

public class TileRes : MonoBehaviour
{
    public bool tileState;

    public GameObject tile = null;
    public List<GameObject> tileList = new List<GameObject>();
    private List<GameObject> goodTiles = new List<GameObject>();

    private List<GameObject> goodPressedTiles = new List<GameObject>();
    void Start()
    {  //Loops through all tiles
        foreach (GameObject obj in tileList)
        {   //Checks each tile if they are supposed to be pressed, if they are, they are added to a list called "goodTiles"
            CubesChild tempCube = obj.GetComponentInChildren<CubesChild>();
            bool goodFromChild = tempCube.good;
            if (goodFromChild)
            {
                goodTiles.Add(obj);
            }
        }
        
    }
    void FixedUpdate()
    { 
        checkGoodTilesPressed();
        resetList();
        winCondition();
      //  print("GoodPressedTiles: " + goodPressedTiles.Count);
    }
    void win()
    {
        print("Ez win");
    }

    void checkGoodTilesPressed()
    {
        foreach (GameObject obj in goodTiles)
        { //Checks each tile in the list if they are pressed. When pressed during the program, they are added to the list "goodPressedTiles"
            CubesChild tempCube = obj.GetComponentInChildren<CubesChild>();
            bool pressedFromChild = tempCube.pressed;
            if (pressedFromChild && !goodPressedTiles.Contains(obj))
            {
                goodPressedTiles.Add(obj);
            }
        }
    }

    void resetList()
    {
        //If an incorrect tile is hit by the player, the goodPressedTiles list is cleared.
        if (tileState == false)
        { //Resets the "pressed" state to false before removing the objects from the "goodPressedTiles" list
            foreach (GameObject obj in goodPressedTiles)
            {
                obj.GetComponentInChildren<CubesChild>().pressed = false;
            }
            //Clears the "goodPressedTiles" list
            goodPressedTiles.Clear();
        }
    }

    void winCondition()
    {
        if (goodTiles.Count == goodPressedTiles.Count)
        {
            win();
        }
    }
}