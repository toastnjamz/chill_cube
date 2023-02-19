using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace3x1x1 : MonoBehaviour
{
    CubeState3x1x1 cubeState;
    ReadCube3x1x1 readCube;
    int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState3x1x1>();
        readCube = FindObjectOfType<ReadCube3x1x1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        {
            // read the current state of the cube
            readCube.ReadState();

            // fire a  raycast from the mouse towards the cube to see if a face is hit
            RaycastHit hit;
            // the start point of the ray will be the mouse's position in world units
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                // save the face we hit
                GameObject face = hit.collider.gameObject;

                // rotate the piece the face it attached to
                cubeState.PickUp(face);
            }
        }
    }
}
