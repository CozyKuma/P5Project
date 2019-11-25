using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrantCalc : MonoBehaviour
{

    public GameObject parentObject;
    public GameObject physObject;
    public int oldQuadrant = 0;
    public int newQuadrant;


    public class Quadrant
    {
        public Vector3 centerPos;
        public Vector3 size;
        public GameObject quadrantObject;
        public static List<Quadrant> allQuadrants = new List<Quadrant>();
        public static GameObject parentHolder;
        public int id;
        public static int counter = 1;

        public Quadrant()
        {

        }

        public Quadrant(GameObject quadrantObject)
        {
            this.quadrantObject = quadrantObject;
            Renderer render = this.quadrantObject.GetComponent<Renderer>();
            var bounds = render.bounds;
            this.centerPos = bounds.center;
            this.size = bounds.size;
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
            var allChildren = parentComponent.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child != allChildren[0])
                {
                    new Quadrant(child.gameObject);
                }
            }
        }

        public static int WithinWhichQuadrant(GameObject physObject)
        {
            foreach(Quadrant quad in allQuadrants)
            {
                Renderer physObjectRend = physObject.GetComponent<Renderer>();
                Vector3 physObjectPos = physObjectRend.bounds.center;
                if (physObjectPos.x < (quad.centerPos.x + quad.size.x/2) && physObjectPos.x > (quad.centerPos.x - quad.size.x/2) && physObjectPos.z < (quad.centerPos.z + quad.size.z/2) && physObjectPos.z > (quad.centerPos.z - quad.size.z/2))
                {

                    //Debug.Log(string.Format("Object Pos: {0}, Quad Pos {1} & Size: {2}", physObjectPos, quad.centerPos, quad.size));
                    //Debug.Log(string.Format("Within Quadrant {0}", quad.id));
                    return quad.id;
                }
            }

            return 0;

        }

        public static int FindOpposite(int quadrant) // Returns the quadrant on the diagonal opposite of the parameter.
        {
            switch (quadrant)
            {
                case 1:
                    return 3;
                case 2:
                    return 4;
                case 3:
                    return 1;
                case 4:
                    return 2;
                default:
                    return 0;
            }
        }

        public static int FindNeighbour(int quadrant, string direction = "horizontal")
        {
            switch (quadrant)
            {
                case 1:
                    if (direction == "horizontal")
                    {
                        return 2;
                    } else if (direction == "vertical")
                    {
                        return 4;
                    }
                    break;
                case 2:
                    if (direction == "horizontal")
                    {
                        return 1;
                    }
                    else if (direction == "vertical")
                    {
                        return 3;
                    }
                    break;
                case 3:
                    if (direction == "horizontal")
                    {
                        return 4;
                    }
                    else if (direction == "vertical")
                    {
                        return 2;
                    }
                    break;
                case 4:
                    if (direction == "horizontal")
                    {
                        return 3;
                    }
                    else if (direction == "vertical")
                    {
                        return 1;
                    }
                    break;
                default:
                    return 0;
            }
            return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Quadrant.SetQuadrantParent(parentObject);
        Quadrant.FindAllChildren();

        newQuadrant = Quadrant.WithinWhichQuadrant(physObject);
        if (newQuadrant != oldQuadrant && newQuadrant != 0)
        {
            Debug.Log(string.Format("Within Quadrant {0}", newQuadrant));
            oldQuadrant = newQuadrant;
        }
    }

    // Update is called once per frame
    void Update()
    {
        newQuadrant = Quadrant.WithinWhichQuadrant(physObject);

        if (newQuadrant != oldQuadrant && newQuadrant != 0)
        {
            Debug.Log(string.Format("Within Quadrant {0}", newQuadrant));
            oldQuadrant = newQuadrant;
        }
    }
}
