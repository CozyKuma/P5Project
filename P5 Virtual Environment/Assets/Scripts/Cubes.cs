using UnityEngine;
public class Cubes : MonoBehaviour
{
    public int tilePressed = 0;
    public bool good, win, pressed;
    private float pressedPos = 0.9f;
    public GameObject tileController;
    Vector3 originalPos;
    public GameObject[] cubeArr;
    void Update()
    {
        if (GameObject.Find("/Map").GetComponent<TileRes>().tileState == false)
        {
            this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
            pressed = false;
        }
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {
            print("Yeah baby!");
            gameObject.GetComponentInParent<Cubes>().pressed = true;
            Debug.Log("parent is "+gameObject.GetComponentInParent<Cubes>());
            if (good == false)
            {
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = false;
                transform.parent.position = new Vector3(transform.position.x, pressedPos, transform.position.z);
            }
            if (good == true)
            {
                GameObject.Find("/Map").GetComponent<TileRes>().tileState = true;
                transform.parent.position = new Vector3(transform.position.x, pressedPos, transform.position.z);
            }
        }
    }
    public bool getGood()
    {
        return good;
    }
}