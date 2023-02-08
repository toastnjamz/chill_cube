using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap3x1x1 : MonoBehaviour
{
    CubeState3x1x1 cubeState;

    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // sets the colors of the CubeMap 
    // uses UpdateMap() method on each of the transforms, passing in the side of the cube
    // from the CubeState script that we want to compare
    public void Set()
    {
        cubeState = FindObjectOfType<CubeState3x1x1>();
        
        UpdateMap(cubeState.front, front);
/*        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);*/
    }

    // loops through each of the children in a side of the CubeMap (the faces named 0-8)
    // and uses the first character of the name of the face at the same index in the list of faces 
    // to update the color of the CubeMap
    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach (Transform map in side)
        {
            // index out of range exception
            if (face[0].name[0] == 'F')
            {
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1); // orange
            }
/*            if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = Color.red;
            }
            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.white;
            }
            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = Color.blue;
            }*/
            i++;
        }
    }
}
