using System.Collections.Generic;
using UnityEngine;

public class CubeState3x1x1 : MonoBehaviour
{
    // sides
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    // pieces
    public GameObject centerPiece;
    public GameObject leftPiece;
    public GameObject rightPiece;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // this works, but it's not what I want because it will always be true
/*        if (front.TrueForAll(x => x.name.Equals("Orange")))
        {
            gameManager.levelIsComplete = true;
        }*/

        //TODO: find a way to check if all faces in a side are the same color
        
        
    }
}
