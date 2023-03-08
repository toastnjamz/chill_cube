using System.Collections.Generic;
using UnityEngine;

public class ReadCube3x1x1 : MonoBehaviour
{
    public Transform tUp;
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tFront;
    public Transform tBack;

    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    private int layerMask = 1 << 8; // this layer mask is for the faces of the cube only, which are on layer 8
    CubeState3x1x1 cubeState;
    //CubeMap3x1x1 cubeMap;
    public GameObject emptyGO;

    // Start is called before the first frame update
    void Start()
    {
        SetRayTransforms();
        cubeState = FindObjectOfType<CubeState3x1x1>();
        //cubeMap = FindObjectOfType<CubeMap3x1x1>();
        //ReadState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState3x1x1>();
        //cubeMap = FindObjectOfType<CubeMap3x1x1>();

        // set the state of each position in the list of sides
        // so we know what color is in what position
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        // update the map with the found positions
        //cubeMap.Set();
    }

    void SetRayTransforms()
    {
        // populate the ray lists with raycassts eminating from the transform, angled toward the cube.
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        // the ray count is used to name the rays so we can be sure they are in the right order.
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        // this creates rays that fit the 3x3x1 shape of the puzzle
        // if the left or right side of the puzzle, just create one ray
        if (rayTransform == tLeft || rayTransform == tRight)
        {
            Vector3 startPos = new Vector3(rayTransform.localPosition.x,
                                              rayTransform.localPosition.y,
                                              rayTransform.localPosition.z);
            GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
            rayStart.name = "0";
            rays.Add(rayStart);
        }
        else
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                                               rayTransform.localPosition.y,
                                               rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();
        
        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            // does the ray intersect any objects in the layerMask?
            // if so, fire a yellow Raycast from tFront's transform in the right direction relative to tFront
            // that goes to infinity and only hits objects in the layerMask
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
        return facesHit;
    }
}
