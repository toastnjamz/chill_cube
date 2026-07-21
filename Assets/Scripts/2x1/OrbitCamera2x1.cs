using UnityEngine;

// Orbits the CAMERA around the puzzle rather than rotating the puzzle root GO.
// This avoids the bug where rotating CubePuzzle (which parents both pieces)
// causes both pieces to spin together as if they're one object.
//
// Attach this script to the Main Camera.
// Set "pivot" to an empty GO at the puzzle's centre (0,0,0).
// The camera will orbit around that point.
//
// Two-finger drag on mobile → orbit.
// Right-click drag in editor → orbit.
// When idle, the camera slowly orbits for ambiance instead of the puzzle spinning.

public class OrbitCamera2x1 : MonoBehaviour
{
    [Header("Orbit pivot (empty GO at puzzle centre, i.e. world origin)")]
    public Transform pivot;

    [Header("Starting orbit distance")]
    public float distance = 5f;

    [Header("Feel")]
    public float orbitSensitivity  = 0.3f;   // degrees per pixel dragged
    public float snapSpeed         = 120f;    // degrees/sec when snapping to home

    [Header("Ambient slow orbit when idle")]
    public bool  ambientRotation      = true;
    public float ambientDegreesPerSec = 8f;

    // Home angles — set these to taste for the default viewing angle
    [Header("Home viewing angle (Euler degrees)")]
    public float homeYaw   = 30f;
    public float homePitch = 20f;

    private float currentYaw;
    private float currentPitch;
    private Vector2 prevMidpoint;
    private bool orbiting = false;

    void Start()
    {
        currentYaw   = homeYaw;
        currentPitch = homePitch;
        ApplyOrbit();
    }

    void Update()
    {
        bool playerOrbiting = false;

        // --- Two-finger orbit (mobile) ---
        if (Input.touchCount == 2)
        {
            playerOrbiting = true;
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);
            Vector2 midpoint = (t0.position + t1.position) * 0.5f;

            if (t1.phase == TouchPhase.Began)
            {
                prevMidpoint = midpoint;
                orbiting = true;
            }

            if (orbiting && (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved))
            {
                Vector2 delta = midpoint - prevMidpoint;
                currentYaw   -= delta.x * orbitSensitivity;
                currentPitch += delta.y * orbitSensitivity;
                currentPitch  = Mathf.Clamp(currentPitch, -80f, 80f);
                prevMidpoint  = midpoint;
                ApplyOrbit();
            }
        }
        else
        {
            orbiting = false;

            // --- Editor / mouse fallback (right-click drag) ---
#if UNITY_EDITOR
            if (Input.GetMouseButton(1))
            {
                playerOrbiting = true;
                float mx = Input.GetAxis("Mouse X");
                float my = Input.GetAxis("Mouse Y");
                currentYaw   -= mx * orbitSensitivity * 10f;
                currentPitch += my * orbitSensitivity * 10f;
                currentPitch  = Mathf.Clamp(currentPitch, -80f, 80f);
                ApplyOrbit();
            }
#endif
        }

        // --- Ambient slow orbit when nothing is happening ---
        if (ambientRotation && !playerOrbiting && !CubeState2x1.autoRotating
            && Input.touchCount == 0
#if UNITY_EDITOR
            && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)
#endif
        )
        {
            currentYaw += ambientDegreesPerSec * Time.deltaTime;
            ApplyOrbit();
        }
    }

    // Positions and orients the camera to orbit around the pivot at (currentYaw, currentPitch)
    void ApplyOrbit()
    {
        if (pivot == null) return;

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 offset      = rotation * new Vector3(0f, 0f, -distance);
        transform.position  = pivot.position + offset;
        transform.LookAt(pivot.position);
    }
}
