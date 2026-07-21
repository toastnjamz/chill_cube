using System.Collections.Generic;
using UnityEngine;

public class ReadCube2x1 : MonoBehaviour
{
    // Distance from centre to fire rays from (should be outside the puzzle)
    public float rayDistance = 3f;

    // All face colliders must be on Layer 8
    private int layerMask = 1 << 8;

    private CubeState2x1 cubeState;

    // The two X positions of the pieces
    private float[] pieceX = { -0.5f, 0.5f };

    void Start()
    {
        cubeState = FindObjectOfType<CubeState2x1>();
        ReadState();
    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState2x1>();

        cubeState.up = ReadSide(Vector3.up, Vector3.down);
        cubeState.down = ReadSide(Vector3.down, Vector3.up);
        cubeState.front = ReadSide(Vector3.forward, Vector3.back);
        cubeState.back = ReadSide(Vector3.back, Vector3.forward);
        cubeState.left = ReadEndCap(Vector3.left, Vector3.right);
        cubeState.right = ReadEndCap(Vector3.right, Vector3.left);

        cubeState.CheckWinCondition();
    }

    // Fires two rays (one per piece) from the given direction
    List<GameObject> ReadSide(Vector3 fromDirection, Vector3 rayDirection)
    {
        var hits = new List<GameObject>();
        foreach (float x in pieceX)
        {
            Vector3 origin = new Vector3(x, 0f, 0f) + fromDirection * rayDistance;
            RaycastHit hit;

            if(Physics.Raycast(origin, rayDirection, out hit, rayDistance * 2f, layerMask))
{
                hits.Add(hit.collider.gameObject);
            }

            Debug.DrawRay(origin, rayDirection * rayDistance * 2f, Color.yellow, 1f);
            if (Physics.Raycast(origin, rayDirection, out hit, rayDistance * 2f, layerMask))
                hits.Add(hit.collider.gameObject);
        }
        return hits;
    }

    // Fires one ray for end caps (left and right ends of the puzzle)
    List<GameObject> ReadEndCap(Vector3 fromDirection, Vector3 rayDirection)
    {
        var hits = new List<GameObject>();
        Vector3 origin = fromDirection * rayDistance;
        RaycastHit hit;
        Debug.DrawRay(origin, rayDirection * rayDistance * 2f, Color.cyan, 1f);
        if (Physics.Raycast(origin, rayDirection, out hit, rayDistance * 2f, layerMask))
            hits.Add(hit.collider.gameObject);
        return hits;
    }
}