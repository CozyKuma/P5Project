using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrantCalc : MonoBehaviour
{

    public GameObject parentObject;
    public GameObject physObject;

    public class Quadrant
    {
        public Vector3 centerPos;
        public float size;
        public GameObject quadrantObject;
        public static List<Quadrant> allQuadrants = new List<Quadrant>();
        public static GameObject parentHolder;
        public int id;
        public static int counter = 1;

        Quadrant(GameObject quadrantObject)
        {
            this.quadrantObject = quadrantObject;
            Renderer render = this.quadrantObject.GetComponent<Renderer>();
            this.centerPos = render.bounds.center;
            this.size = render.bounds.extents.magnitude;
            this.id = counter;
            counter += 1;
            allQuadrants.Add(this);
        }

        public static void SetQuadrantParent(GameObject parent)
        {
            Quadrant.parentHolder = parent;
        }

        public static void FindAllChildren()
        {
            Transform parentComponent = parentHolder.GetComponent<Transform>();
            Transform[] allChildren;
            allChildren = parentComponent.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child != allChildren[0])
                {
                    new Quadrant(child.gameObject);
                }
            }
        }

        public static int WithinWhichQuadrant(GameObject PhysObject)
        {
            foreach(Quadrant quad in allQuadrants)
            {
                Renderer physObjectRend = PhysObject.GetComponent<Renderer>();
                Vector3 physObjectPos = physObjectRend.bounds.center;
                if (physObjectPos.x < (quad.centerPos.x + quad.size) && physObjectPos.x > (quad.centerPos.x - quad.size) && physObjectPos.z < (quad.centerPos.z + quad.size) && physObjectPos.z > (quad.centerPos.z - quad.size))
                {
                    Debug.Log(string.Format("Within Quadrant {0}", quad.id));
                    return quad.id;
                }
            }

            return 0;

        }
    }

    // Start is called before the first frame update
    void Start()
    {

        Quadrant.SetQuadrantParent(parentObject);
        Quadrant.FindAllChildren();
    }

    // Update is called once per frame
    void Update()
    {
        Quadrant.WithinWhichQuadrant(physObject);
    }
}
