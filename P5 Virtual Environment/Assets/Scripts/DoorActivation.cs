using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivation : MonoBehaviour
{
    public bool doorOpened;

    public float doorSpeed, counter, height;

    public GameObject gameObject;

    private Vector3 originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpened && counter < height)
        {
            Vector3 temp = transform.position;
            temp.y += doorSpeed;
            transform.position = temp;
            counter += doorSpeed;
        }
        else if (!doorOpened && counter == height)
        {
            transform.position = originalPosition;
            counter = 0;
        }
    }

    public void openDoor()
    {
        if (doorOpened && counter < height)
        {
            Vector3 temp = transform.position;
            temp.y += doorSpeed;
            transform.position = temp;
            counter += doorSpeed;
        }
        else if (!doorOpened && counter == height)
        {
            transform.position = originalPosition;
            counter = 0;
        }
    }
    
}
