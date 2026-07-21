using UnityEngine;

// Handles rotating one piece of the 2x1 puzzle.
// Attach to each piece's Pivot GameObject (LeftPiece / RightPiece).
// Barrel-roll axis is world X. Swipe up/down to spin.

public class PivotRotation2x1 : MonoBehaviour
{
    [Header("Feel")]
    public float sensitivity = 0.5f;   // drag pixels → degrees
    public float snapSpeed   = 200f;   // degrees per second when snapping

    private bool      dragging     = false;
    private bool      autoRotating = false;
    private Vector3   mouseRef;
    private Quaternion targetQuaternion;

    private ReadCube2x1 readCube;

    void Start()
    {
        readCube = FindObjectOfType<ReadCube2x1>();
    }

    void LateUpdate()
    {
        if (dragging && !autoRotating)
        {
            SpinPiece();

            bool fingerLifted = Input.touchCount == 0 && !Input.GetMouseButton(0);
            if (fingerLifted)
            {
                dragging = false;
                SnapToNearest90();
            }
        }

        if (autoRotating)
            AutoRotate();
    }

    // Called by SelectFace2x1 when a swipe starts on this piece
    public void StartDrag(Vector3 startMousePos)
    {
        mouseRef  = startMousePos;
        dragging  = true;
        CubeState2x1.autoRotating = true;
    }

    void SpinPiece()
    {
        Vector3 currentPos = Input.mousePosition;
        float delta = (currentPos.y - mouseRef.y) * sensitivity;
        transform.Rotate(Vector3.right, -delta, Space.World);
        mouseRef = currentPos;
    }

    void SnapToNearest90()
    {
        Vector3 euler = transform.localEulerAngles;
        euler.x = Mathf.Round(euler.x / 90f) * 90f;
        euler.y = Mathf.Round(euler.y / 90f) * 90f;
        euler.z = Mathf.Round(euler.z / 90f) * 90f;
        targetQuaternion = Quaternion.Euler(euler);
        autoRotating = true;
    }

    void AutoRotate()
    {
        dragging = false;
        float step = snapSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation, targetQuaternion, step);

        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1f)
        {
            transform.localRotation   = targetQuaternion;
            autoRotating              = false;
            CubeState2x1.autoRotating = false;

            if (readCube != null)
                readCube.ReadState();
        }
    }
}
