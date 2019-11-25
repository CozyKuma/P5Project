using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class light_Position_check : MonoBehaviour
{
    
    public GameObject destinationLight;
    public GameObject winText;
    public float range;
    public float maxAngleDiviation;
    
    Vector3 destinationLightPosition;

    private AudioSource audioData;
    
    private Vector3 destinationLightRotation;
    private Vector3 rotationMax;
    private Vector3 rotationMin;
    
    private bool solved = false;
    private RoomStateController roomStateController;
    [SerializeField] private GameObject CorrSystem;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        
        if (CorrSystem == null)
        {
            CorrSystem = GameObject.Find("CorridorSystem");
        }
        destinationLightPosition = destinationLight.transform.position;
        destinationLightRotation = destinationLight.transform.eulerAngles;
        roomStateController = CorrSystem.GetComponent<RoomStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerLightPosition = transform.position;
        Vector3 playerLightRotation = transform.eulerAngles;

        float angleDifX = 
            Mathf.Abs(Mathf.DeltaAngle(playerLightRotation.x, destinationLightRotation.x));
        float angleDifY =
            Mathf.Abs(Mathf.DeltaAngle(playerLightRotation.y, destinationLightRotation.y));

        if (angleDifX <= maxAngleDiviation && angleDifY <= maxAngleDiviation && Vector3.Distance(playerLightPosition, destinationLightPosition)<=range)
        {
            completePuzzle();
        }
    }

    private void completePuzzle()
    {
        Debug.Log("shadow won");
        if (!solved)
        {
            audioData.Play();
        }
        solved = true;
        winText.SetActive(true);
        roomStateController.SetLevelComplete(true);
    }
}

