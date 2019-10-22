using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{

    public bool good = false;
    public GameObject tileController;
    Vector3 originalPos;
    public GameObject cube;

    void Start()
    {
    }

    void Update()
    {
        if (GameObject.Find("/Map").GetComponent<TileRes>().tileState == false)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
        }
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.name == "Player")
        {
            transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);
            if (good == false)
            {
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = false;
            }

            else
            {
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = true;
                print("Big booty bitches");
            }
        }

    }



}
