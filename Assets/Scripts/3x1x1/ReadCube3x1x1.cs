using System.Collections;
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

    private int layerMask = 1 << 8; // this layer mask is for the faces of the cube only, which are on layer 8
    CubeState3x1x1 cubeState;
    CubeMap3x1x1 cubeMap;
    public GameObject emptyGO;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState3x1x1>();
        cubeMap = FindObjectOfType<CubeMap3x1x1>();

        List<GameObject> facesHit = new List<GameObject>();
        Vector3 ray = tFront.transform.position;
        RaycastHit hit;

        // does the ray intersect any objects in the layerMask?
        // if so, fire a yellow Raycast from tFront's transform in the right direction relative to tFront
        // that goes to infinity and only hits objects in the layerMask
        if (Physics.Raycast(ray, tFront.right, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ray, tFront.right * hit.distance, Color.yellow);
            facesHit.Add(hit.collider.gameObject);
            print(hit.collider.gameObject.name);
        }
        else
        {
            Debug.DrawRay(ray, tFront.right * 1000, Color.green);
        }
        cubeState.front = facesHit;
        cubeMap.Set();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        // the ray count is used to name the rays so we can be sure they are in the right order.
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

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
}
