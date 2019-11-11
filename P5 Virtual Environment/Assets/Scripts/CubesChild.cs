using UnityEngine;
public class CubesChild : MonoBehaviour
{
    public bool good, pressed;
    private float pressedPos = 0.9f;
    void Update()
    { //In case the player collides with an incorrect tile, this if-statement is true.
        if (GameObject.Find("/Map").GetComponent<TileRes>().tileState == false)
        {
            //Resets the position of all tiles, so no tile is lowered.
            transform.parent.position = new Vector3(transform.parent.position.x, 1, transform.parent.position.z);
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
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = false;
            }
            if (good == true)
            {
                //If the player hits a tile, where the "good" boolean is true, the "tilestate" is set to true and the tile is lowered
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = true;
                transform.parent.position = new Vector3(transform.parent.position.x, pressedPos, transform.parent.position.z);
            }
        }
    }
}