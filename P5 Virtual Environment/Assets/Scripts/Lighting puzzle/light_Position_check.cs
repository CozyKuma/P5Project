using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class light_Position_check : MonoBehaviour
{
    
    public GameObject destinationLight;
    public float range;
    public int coneAngle;
    public int coneDiviation;
    Vector3 destinationLightPosition;
    private float maxAngleDiviation;
    private Vector3 rotationMax;
    private Vector3 rotationMin;


    public Light thisLight;

    // Start is called before the first frame update
    void Start()
    {
        destinationLightPosition = destinationLight.transform.position;

        thisLight.spotAngle = coneAngle;
        
        maxAngleDiviation = coneAngle-coneDiviation;
        
        
        Vector3 destinationLightRotation = destinationLight.transform.eulerAngles;

        rotationMax = destinationLightRotation + new Vector3(maxAngleDiviation, maxAngleDiviation, maxAngleDiviation);
        rotationMin = destinationLightRotation - new Vector3(maxAngleDiviation, maxAngleDiviation, maxAngleDiviation);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentLight = transform.position;
        
        

        if (Input.GetKeyDown("space"))
        {
            print(transform.eulerAngles.ToString());
            print(rotationMax);
            print(rotationMin);
        }

        if (transform.eulerAngles.x <= rotationMax.x && transform.eulerAngles.x >=rotationMin.x &&
        transform.eulerAngles.y <= rotationMax.y && transform.eulerAngles.y >=rotationMin.y 
        && Vector3.Distance(currentLight, destinationLightPosition)<=range) {
            print("wooooo");
        }
        

    }
}

