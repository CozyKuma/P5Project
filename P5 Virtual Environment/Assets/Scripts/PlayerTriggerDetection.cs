using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDetection : MonoBehaviour
{
    public GameObject mainRoom;
    private ActiveRoomController activeRoomController;

    // Start is called before the first frame update
    void Start()
    {
        activeRoomController = mainRoom.GetComponent<ActiveRoomController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Trigger")
        {
            ActiveRoomController.Room roomVar = activeRoomController.FindTrigger(other.gameObject);
            ActivateRoom(roomVar);
            //Debug.Log(roomVar.GetroomNum());
            //Debug.Log(string.Format("Collision Detected - Room {0}", roomToActivate.GetroomNum()));
        } else
        {
            // nothing
        }
    }

    private void ActivateRoom(ActiveRoomController.Room room)
    {
        ActiveRoomController.Room.FlipRooms(room);
        ActiveRoomController.Wall.changeState((room.GetroomNum() - 1));
    }
}
