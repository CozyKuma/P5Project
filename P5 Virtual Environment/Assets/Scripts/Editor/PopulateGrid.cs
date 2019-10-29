using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PopulateGrid : EditorWindow
{

    public GameObject prefabObject = null;
    public GameObject gridParent = null;
    public GameObject tempObject = null;
    public List<GameObject> listOfCells = new List<GameObject>();
    public int numberOfPrefabs = 1;
    public int rowSize = 1;
    public int columnSize = 1;

    [MenuItem("Window/Populate Grid")]
    static void OpenWindow()
    {
        PopulateGrid window = (PopulateGrid)GetWindow(typeof(PopulateGrid));
        window.minSize = new Vector2(400, 200);
        window.Show();
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        gridParent = EditorGUILayout.ObjectField("Select Parent Grid Object: ", gridParent, typeof(GameObject), true) as GameObject;
        prefabObject = EditorGUILayout.ObjectField("Select Prefab: ", prefabObject, typeof(GameObject), true) as GameObject;
        rowSize = EditorGUILayout.IntField("Number of Rows: ", rowSize);
        columnSize = EditorGUILayout.IntField("Number of Columns", columnSize);

        //numberOfPrefabs = EditorGUILayout.IntField("Select Number of Prefabs you want to create: ", numberOfPrefabs);

        if (gridParent && prefabObject)
        {
            if (GUILayout.Button("Populate Grid"))
            {
                numberOfPrefabs = rowSize * columnSize;
                for (int i = 0; i < rowSize; i++)
                {
                    for (int j = 0; j < columnSize; j++)
                    {
                        Vector3 prefabPosition = new Vector3((float)(2.5f - (1f * i)), 0, (float)(2.5f - (1f * j)));
                        tempObject = (GameObject)Instantiate(prefabObject, prefabPosition, Quaternion.identity);
                        tempObject.name = prefabObject.name;
                        tempObject.transform.SetParent(gridParent.transform, true);
                        listOfCells.Add(tempObject);
                    }
                }
            }
        }

        if(GUILayout.Button("Create List of Current Children"))
        {
            Transform parentObject = gridParent.transform;
            Transform[] childrenOfParent;
            childrenOfParent = parentObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in childrenOfParent)
            {
                if (child != childrenOfParent[0])
                {
                    listOfCells.Add(child.gameObject);
                }
            }
        }

        if (GUILayout.Button("Clear Grid")) {
            foreach (GameObject cellObject in listOfCells)
            {
                DestroyImmediate(cellObject);
            }
        } else {
            EditorGUILayout.LabelField("You must fill in the objects first.");
        }
    }
}
