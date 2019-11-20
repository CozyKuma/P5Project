﻿using System.Collections.Generic;
using UnityEngine;

public class TileRes : MonoBehaviour
{
    public bool tileState;
    public bool winState;

    public AudioClip waterSound;
    public AudioClip flameSound;
    public AudioSource audio;

    private bool waterPlay;
    private bool firePlay;

    public GameObject tile = null;
    public List<GameObject> tileList = new List<GameObject>();
    private List<CubesChild> goodTiles = new List<CubesChild>();
    private Animator thisAnim;

    private List<CubesChild> goodPressedTiles = new List<CubesChild>();
    void Start()
    {  
        //Loops through all tiles
        foreach (GameObject obj in tileList)
        {   //Checks each tile if they are supposed to be pressed, if they are, they are added to a list called "goodTiles"
            CubesChild tempCube = obj.GetComponentInChildren<CubesChild>();
            bool goodFromChild = tempCube.good;
            if (goodFromChild)
            {
                goodTiles.Add(tempCube);
            }
        }
        GameObject waterObj = GameObject.Find("WaterBasicDaytime");
        thisAnim = waterObj.GetComponent<Animator>();

    }
    private void Update()
    { 
        checkGoodTilesPressed();
        resetList();
        winCondition();
      //  print("GoodPressedTiles: " + goodPressedTiles.Count);
    }
    void completePuzzle()
    {
        print("Tile Puzzle Ez win");
        
    }

    void checkGoodTilesPressed()
    {
        foreach (CubesChild obj in goodTiles)
        { //Checks each tile in the list if they are pressed. When pressed during the program, they are added to the list "goodPressedTiles"
            //CubesChild tempCube = obj.GetComponentInChildren<CubesChild>();
            bool pressedFromChild = obj.pressed;
            if (pressedFromChild && !goodPressedTiles.Contains(obj))
            {
                goodPressedTiles.Add(obj);
                thisAnim.SetTrigger("Drain"); //Drains the object of water
                if (!firePlay)
                {
                    audio.PlayOneShot(flameSound, 0.5f); 
                    firePlay= true;
                    waterPlay = false;
                }
            }
        }
    }

    void resetList()
    {
        //If an incorrect tile is hit by the player, the goodPressedTiles list is cleared.
        if (tileState == false)
        {
            //Resets the "pressed" state to false and disables emission before removing the objects from the "goodPressedTiles" list
            foreach (CubesChild obj in goodPressedTiles)
            {
                obj.pressed = false;
                obj.Reset();
            }

            //Clears the "goodPressedTiles" list and fill the object with water
            goodPressedTiles.Clear();
            thisAnim.SetTrigger("Fill");
            if (!waterPlay)
            {
                audio.PlayOneShot(waterSound, 0.5f);
                waterPlay = true;
                firePlay = false;
            }
        }
    }

    void winCondition()
    {
        if (goodTiles.Count == goodPressedTiles.Count)
        {
            winState = true;
            completePuzzle();
        }
    }
    void win()
    {
        print("Ez win");
    }
}