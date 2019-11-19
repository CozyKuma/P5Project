using System;
using UnityEngine;
public class CubesChild : MonoBehaviour
{
    public bool good, pressed;
    public float pressedPos = -1.265f;
    public float zeroPos = -1.25f;
    private Vector3 parentPos;
    private TileRes manager;
    private Material parentMaterial;

    private void Start()
    {
        manager = GameObject.Find("/ScaleContainer/FloorTiles/Map").GetComponent<TileRes>();
        parentMaterial = gameObject.GetComponentsInParent<Renderer>()[1].material;
        parentPos = transform.parent.localPosition;
    }

    private void Update()
    {
    }

    public void Reset()
    {
        if (manager.tileState == false && manager.winState == false)
        {//In case the player collides with an incorrect tile, this if-statement is true.
            //Resets the position of all tiles, so no tile is lowered.
            transform.parent.localPosition = new Vector3(parentPos.x, zeroPos, parentPos.z);
            pressed = false;
            parentMaterial.DisableKeyword("_EMISSION");
        }
    }

    //Checks if something (in this case, the player), collides with the collision trigger over the tiles
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {//If a tile is pressed, the "pressed" boolean from "Cubes" class is set to true.
            //gameObject.GetComponentInParent<CubesChild>().pressed = true;
            pressed = true;
            if (good == false)
            {
                //If the player hits a tile, where the "good" boolean is false, the "tilestate" is set to false, which activates line 8
                manager.tileState = false;
            }
            if (good)
            {
                //If the player hits a tile, where the "good" boolean is true, the "tilestate" is set to true, the tile is lowered, and emission is enabled on the parent
                manager.tileState = true;
                parentMaterial.EnableKeyword("_EMISSION");
                transform.parent.localPosition = new Vector3(parentPos.x, pressedPos, parentPos.z);
            }
        }
    }
}