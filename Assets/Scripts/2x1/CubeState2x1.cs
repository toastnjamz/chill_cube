using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState2x1 : MonoBehaviour
{
    // Populated by ReadCube2x1 after each move
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back  = new List<GameObject>();
    public List<GameObject> up    = new List<GameObject>();
    public List<GameObject> down  = new List<GameObject>();
    public List<GameObject> left  = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public static bool autoRotating = false;

    [Header("Win Condition")]
    [Tooltip("The face GO the astronaut sits on (e.g. U_Left)")]
    public GameObject astronautFace;

    [Tooltip("The face GO the target circle sits on (e.g. B_Right)")]
    public GameObject targetFace;

    GameManager gameManager;
    private bool winCheckEnabled = false;
    private bool levelComplete   = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(EnableWinCheckNextFrame());
    }

    IEnumerator EnableWinCheckNextFrame()
    {
        yield return null;
        winCheckEnabled = true;
    }

    public void CheckWinCondition()
    {
        if (!winCheckEnabled || levelComplete) return;
        if (astronautFace == null || targetFace == null) return;

        // Win if both faces appear in the same side list —
        // meaning they are currently pointing in the same direction
        List<List<GameObject>> allSides = new List<List<GameObject>>
        {
            up, down, front, back, left, right
        };

        foreach (List<GameObject> side in allSides)
        {
            if (side.Contains(astronautFace) && side.Contains(targetFace))
            {
                levelComplete = true;
                if (gameManager != null)
                    gameManager.levelIsComplete = true;
                return;
            }
        }
    }

    public void PickUp(List<GameObject> cubeSide, Transform pivotParent)
    {
        foreach (GameObject face in cubeSide)
            face.transform.parent.SetParent(pivotParent);
    }

    public void PutDown(List<GameObject> faces, Transform cubeRoot)
    {
        foreach (GameObject face in faces)
            face.transform.parent.SetParent(cubeRoot);
    }
}
