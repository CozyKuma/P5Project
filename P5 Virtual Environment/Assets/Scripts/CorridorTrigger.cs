using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorTrigger : MonoBehaviour
{
    public CorridorSystemV2.Corridor.typeOfCorridor typeOfTrigger;
    public CorridorSystemV2 CorrSystem;
    public RoomStateController roomStateController;
    public bool activated;

    private void Start()
    {
        CorrSystem = GameObject.Find("CorridorSystem").GetComponent<CorridorSystemV2>();
        roomStateController = GameObject.Find("CorridorSystem").GetComponent<RoomStateController>();
    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name != "player" && other.name != "Player" && other.name != "PLAYER" &&
            other.name != "PrototypePlayerCharacter") return;
        
        if (activated) return;
        
        switch (typeOfTrigger)
        {
            case CorridorSystemV2.Corridor.typeOfCorridor.EXIT:
                CorrSystem.GenerateBridge();
                activated = true;
                break;
            case CorridorSystemV2.Corridor.typeOfCorridor.SIDE1:
                roomStateController.DeactivateRoom(roomStateController.getCurrentState());
                activated = true;
                roomStateController.SetLevelComplete(false);
                break;
            case CorridorSystemV2.Corridor.typeOfCorridor.BRIDGE:
                CorrSystem.GenerateEntrance();
                activated = true;
                break;
            case CorridorSystemV2.Corridor.typeOfCorridor.ENTRANCE:
                CorrSystem.GenerateExit();
                roomStateController.NextState();
                activated = true;
                break;
            default:
                // DO NOTHING
                break; 
        }
    }
}
