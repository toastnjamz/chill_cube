using UnityEngine;

// Scrambles the 2x1 puzzle at the start of the level by instantly applying
// one random 90° or 180° barrel-roll to one of the two pieces.
// This is applied instantly (no animation) before the player sees the puzzle,
// so it looks like the puzzle arrived already scrambled.
//
// Attach to any GO in the scene. Drag the two piece Pivot GOs into the Inspector.

public class ScramblePuzzle2x1 : MonoBehaviour
{
    [Header("The two piece Pivot GameObjects")]
    public Transform leftPiecePivot;
    public Transform rightPiecePivot;

    [Tooltip("Degrees to scramble by. 180 means one flip (symmetric, always 1 move to solve). " +
             "90 or 270 are also 1 move away from solved.")]
    public float scrambleAngle = 180f;

    void Awake()
    {
        // Pick one piece at random
        Transform piece = Random.value > 0.5f ? leftPiecePivot : rightPiecePivot;
        // Apply a barrel-roll around world X instantly
        float angle = scrambleAngle;
        piece.Rotate(Vector3.right, angle, Space.World);
    }
}
