﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private bool usePrototypeCharacter = false;
    public CorridorSystemV2.Corridor.typeOfCorridor typeOfDoor;
    public CorridorSystemV2 CorrSystem;
    public RoomStateController roomStateController;
    public float doorSpeed = 1.4f, height = 3f;
    public bool doorOpened = true;
    private Vector3 originalPosition;
    [SerializeField] private float distanceFromPlayerThreshold = 0.8f;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float distanceFromTrackedObjectThreshold = 1.2f;
    [SerializeField] private float distanceFromTrackedObject;
    [SerializeField] private float distanceFromCenterThreshold = 0.2f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject trackedObject;
    [SerializeField] private GameObject roomCenterObject;
    private bool trackedObjectBool = false;
    
    AudioSource audioData;

    
    // Start is called before the first frame update
    private void Start()
    {
        trackedObject = GameObject.FindWithTag("TrackedObject");
        if (trackedObject != null)
        {
            trackedObjectBool = true;
        } else if (trackedObject == null)
        {
            roomStateController.onRoomActivateEvent += SetTrackedObject;
        }
        
        audioData = GetComponent<AudioSource>();
    }

    void Awake()
    {
        originalPosition = transform.position;
        roomCenterObject = GameObject.Find("CorridorSystemCenter");
        CorrSystem = GameObject.Find("CorridorSystem").GetComponent<CorridorSystemV2>();
        roomStateController = GameObject.Find("CorridorSystem").GetComponent<RoomStateController>();
        if (player != null) return;
        player = !usePrototypeCharacter ? GameObject.Find("Player") : GameObject.Find("PrototypePlayerCharacter");
        /*if (trackedObject != null) return;
        trackedObject = GameObject.FindGameObjectWithTag("TrackedObject");*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!trackedObjectBool) return;
        distanceFromTrackedObject = Vector3.Distance(originalPosition, trackedObject.transform.position);
        distanceFromPlayer = Vector2.Distance(new Vector2(originalPosition.x, originalPosition.z), new Vector2(player.transform.position.x, player.transform.position.z));
        if (typeOfDoor == CorridorSystemV2.Corridor.typeOfCorridor.ENTRANCE &&
            distanceFromPlayer < distanceFromPlayerThreshold)
        {
            OpenDoor();
        } else if (typeOfDoor == CorridorSystemV2.Corridor.typeOfCorridor.EXIT &&
                   roomStateController.GetLevelComplete() &&
                   distanceFromTrackedObject > distanceFromTrackedObjectThreshold)
        {
            OpenDoor();
        } else if (typeOfDoor == CorridorSystemV2.Corridor.typeOfCorridor.ENTRANCE &&
                   (Vector3.Distance(player.transform.position, roomCenterObject.transform.position) +
                    distanceFromCenterThreshold
                    < Vector3.Distance(originalPosition, roomCenterObject.transform.position)))
        {
            LockDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    
    public void OpenDoor()
    {
        Vector3 temp = transform.position;
        if (doorOpened && transform.position.y <= height)
        {
            temp.y += doorSpeed * Time.deltaTime;
            transform.position = temp;
            audioData.Play(0);
            //counter += doorSpeed * Time.deltaTime;
            if ((height - transform.position.y) < doorSpeed * Time.deltaTime)
            {
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
            }
        }
    }

    public void CloseDoor()
    {
        if (!(transform.position.y > originalPosition.y)) return;
        Vector3 temp = transform.position;
        temp.y -= doorSpeed * Time.deltaTime;
        transform.position = temp;
        if ((transform.position.y - originalPosition.y) < doorSpeed * Time.deltaTime)
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
        }
    }

    private void LockDoor()
    {
        doorOpened = false;
        if (!(transform.position.y > originalPosition.y)) return;
        Vector3 temp = transform.position;
        temp.y -= doorSpeed * Time.deltaTime;
        transform.position = temp;
        if ((transform.position.y - originalPosition.y) < doorSpeed * Time.deltaTime)
        {
            transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
        }
    }

    public void SetTrackedObject()
    {
        Debug.Log("Setting Tracked Object");
        roomStateController.onRoomActivateEvent -= SetTrackedObject;
        trackedObject = GameObject.FindWithTag("TrackedObject");
        trackedObjectBool = true;
    }
}