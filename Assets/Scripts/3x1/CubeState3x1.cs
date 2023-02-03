using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState3x1 : MonoBehaviour
{
    // sides
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    // might be able to remove this or comment it out b/c we're not shuffling or auto-solving in this level
    public static bool autoRotating = false;
    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            // attach the parent of each face (the little cube)
            // to the parent of the 1st index (the little cube in the middle)
            // unless it is already the 1st index, since you can't parent something to itself
            if (face != cubeSide[1])
            {
                face.transform.parent.transform.parent = cubeSide[1].transform.parent;
            }
        }
    }

    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[1])
            {
                littleCube.transform.parent.transform.parent = pivot;
                // could have named pivot script's parent transform better
            }
        }
    }

    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        foreach (GameObject face in side)
        {
            sideString += face.name[0].ToString();
        }
        return sideString;
    }

    // might be able to remove this or comment it out b/c we're not shuffling or auto-solving in this level
    public string GetStateString()
    {
        string stateString = "";
        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);
        return stateString;
    }
}

