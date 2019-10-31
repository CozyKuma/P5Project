﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    public int tilePressed = 0;
    public bool good;
    public bool win;
    public bool pressed;
    private float pressedPos = 0.9f;
    public GameObject tileController;
    Vector3 originalPos;
    public GameObject[] cubeArr;  
        
    
 

    void Start()
    {
           
    }

    void Update()
    {
        if (GameObject.Find("/Map").GetComponent<TileRes>().tileState == false)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
            pressed = false;



        }
        


        if (win == true)
        {
            print("Spørg sundhedsstyrelsen!");
        }

        print("The tile is pressed: " + pressed);
    }

    public void OnTriggerEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {   


            pressed = true;
            transform.position = new Vector3(transform.position.x, pressedPos, transform.position.z);
            if (good == false)
            {
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = false;
                
            }
            
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {


            if (good == true)
            {

                GameObject.Find("/Map").GetComponent<TileRes>().tileState = true;
                print("Spørg for helvede!");



            }
        }
    }

    public bool getGood()
    {
        return good;
    }
}

