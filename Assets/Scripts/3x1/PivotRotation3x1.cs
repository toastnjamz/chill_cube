using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation3x1 : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;
    private float sensitivity = 0.4f;
    private float speed = 300f;
    private Vector3 rotation;

    // for the target angle we want to automatically move to
    private Quaternion targetQuaternion;

    private ReadCube3x1 readCube;
    private CubeState3x1 cubeState;

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube3x1>();
        cubeState = FindObjectOfType<CubeState3x1>();
    }

    // Late Update is called once per frame at the end
    void LateUpdate()
    {
        if (dragging && !autoRotating)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }
        if (autoRotating)
        {
            AutoRotate();
        }
    }

    // calculate the rotation that is called on every frame we are dragging the side
    private void SpinSide(List<GameObject> side)
    {
        // reset the rotation
        rotation = Vector3.zero;
        // get the current mouse position minus the last mouse position
        // so we know how much to rotate the side
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        // rotate
        transform.Rotate(rotation, Space.Self);

        // store mouse for the next time we call this method
        mouseRef = Input.mousePosition;
    }

    // called once at the start of the rotation to set the variables
    // then actual rotation will be done in Update()
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        // keep track of the start position of the mouse so we know
        // how much to rotate the side by as the mouse moves away 
        // from the start position
        mouseRef = Input.mousePosition;
        dragging = true;
        // create a vector to rotate around based on the local position
        // of the piece we are rotating and the center of the cube (0, 0, 0)
        localForward = Vector3.zero - side[1].transform.parent.transform.localPosition;
    }

    // might be able to remove this or comment it out b/c we're not shuffling or auto-solving in this level
    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[1].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    // handles the automatic roation
    // call once to setup the variables, then do the actual rotation in Update()
    // to get the angle to rotate to when we let go of the mouse, rotate the current
    // rotation to the nearest 90 degrees, then set that as the target
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        // round vec to the nearest 90 degrees
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate()
    {
        // releasing the left mouse button turns dragging off, but
        // there may be other times when we want to call this method
        // so, make sure dragging is turned off if auto-rotating is turned on
        dragging = false;
        var step = speed * Time.deltaTime;
        // adjust the local rotation of the pivot by the step amount over time
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        // if within 1 degree, set angle to target angle and end the rotation
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            // unparent the little cubes
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();

            CubeState.autoRotating = false;
            autoRotating = false;
            // might not need to do this again
            dragging = false;
        }
    }
}
