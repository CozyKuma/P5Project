using UnityEngine;
public class CubesChild : MonoBehaviour
{
    public bool good, pressed;
    public float pressedPos = -1.265f;
    public float zeroPos = -1.25f;
    void Update()
    { //In case the player collides with an incorrect tile, this if-statement is true.
        if (GameObject.Find("/ScaleContainer/FloorTiles/Map").GetComponent<TileRes>().tileState == false && GameObject.Find("/ScaleContainer/FloorTiles/Map").GetComponent<TileRes>().winState == false)
        {
            //Resets the position of all tiles, so no tile is lowered.
            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, zeroPos, transform.parent.localPosition.z);
            pressed = false;
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
                GameObject.Find("/ScaleContainer/FloorTiles/Map").GetComponent<TileRes>().tileState = false;
            }
            if (good)
            {
                //If the player hits a tile, where the "good" boolean is true, the "tilestate" is set to true, the tile is lowered, and emission is enabled on the parent
                GameObject.Find("/ScaleContainer/FloorTiles/Map").GetComponent<TileRes>().tileState = true;
                gameObject.GetComponentsInParent<Renderer>()[1].material.EnableKeyword("_EMISSION");
                transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, pressedPos, transform.parent.localPosition.z);
            }
        }
    }
}