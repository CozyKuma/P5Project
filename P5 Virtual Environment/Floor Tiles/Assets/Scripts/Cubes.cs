using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    public int tilePressed = 0;
    public bool good = false;
    public GameObject tileController;
    Vector3 originalPos;
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public GameObject cube5;
    public GameObject cube6;
    public GameObject cube7;
    public GameObject cube8;
    public GameObject cube9;
    public int TileCounter;

    void Start()
    {
        cube1 = GameObject.Find("1,5");
        cube2 = GameObject.Find("2,5");
        cube3 = GameObject.Find("3,5");
        cube4 = GameObject.Find("3,4");
        cube5 = GameObject.Find("3,3");
        cube6 = GameObject.Find("3,2");
        cube7 = GameObject.Find("3,1");
        cube8 = GameObject.Find("4,1");
        cube9 = GameObject.Find("5,1");
        TileCounter = GameObject.Find("/Map").GetComponent<TileRes>().counter;
        
    }

    void Update()
    {
        if (GameObject.Find("/Map").GetComponent<TileRes>().tileState == false)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
            tilePressed = 0;
            print("Amount of tiles pressed: " + tilePressed);

        }

       /* if(cube1.transform.position.y == 0.9f && cube2.transform.position.y == 0.9f && cube3.transform.position.y == 0.9f && cube4.transform.position.y == 0.9f &&
            cube5.transform.position.y == 0.9f && cube6.transform.position.y == 0.9f && cube7.transform.position.y == 0.9f && cube8.transform.position.y == 0.9f &&
            cube9.transform.position.y == 0.9f)
        {
            print("Spørg sundhedsstyrelsen!");
        }
        */
        
        

        if (tilePressed == 9)
        {
            print("Tear that wall down, Gorbatjov!");
        }

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {
         
            

            transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);
            if (good == false)
            {
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = false;
                
            }
            if (good == true)
            {
                TileCounter++;
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = true;
                print("Big booty bitches");
                tilePressed++;  
                print("Amount of tiles pressed: " + TileCounter);

            }
        }

    }
}
