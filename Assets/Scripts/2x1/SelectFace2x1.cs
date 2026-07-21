using System.Collections.Generic;
using UnityEngine;

// Detects which piece the player taps/swipes and delegates rotation to
// the PivotRotation2x1 on that piece's pivot GameObject.
// Uses the face GO's direct parent chain to find the correct pivot,
// avoiding the bug where GetComponentInParent walks too far up the tree.

public class SelectFace2x1 : MonoBehaviour
{
    [Header("Piece Pivots (drag LeftPiece and RightPiece here)")]
    public PivotRotation2x1 leftPiecePivot;
    public PivotRotation2x1 rightPiecePivot;

    [Header("Piece Meshes (drag LeftPieceMesh and RightPieceMesh here)")]
    public Transform leftPieceMesh;
    public Transform rightPieceMesh;

    private CubeState2x1 cubeState;
    private int layerMask = 1 << 8;

    [Tooltip("Pixels the finger must move before we commit to a piece rotation")]
    public float swipeThreshold = 10f;

    private bool touchStarted = false;
    private Vector3 touchStartPos;
    private PivotRotation2x1 activePivot = null;

    void Start()
    {
        cubeState = FindObjectOfType<CubeState2x1>();
    }

    void Update()
    {
        if (CubeState2x1.autoRotating) return;
        if (Input.touchCount >= 2) return;

        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            touchStarted  = true;
            activePivot   = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                GameObject faceGO = hit.collider.gameObject;

                // Determine which piece was hit by checking if the face's
                // grandparent (face → PieceMesh → PiecePivot) matches
                // the known left or right mesh transforms directly.
                Transform faceMesh = faceGO.transform.parent;
                if (faceMesh == leftPieceMesh)
                    activePivot = leftPiecePivot;
                else if (faceMesh == rightPieceMesh)
                    activePivot = rightPiecePivot;
            }
        }

        // Once the finger has moved far enough, commit the drag to the pivot
        if (touchStarted && activePivot != null && !Input.GetMouseButtonDown(0))
        {
            float moved = Vector3.Distance(Input.mousePosition, touchStartPos);
            if (moved >= swipeThreshold)
            {
                activePivot.StartDrag(touchStartPos);
                touchStarted = false;
                activePivot  = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchStarted = false;
            activePivot  = null;
        }
    }
}
