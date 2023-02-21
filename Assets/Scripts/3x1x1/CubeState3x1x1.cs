using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        /*centerPiece = GameObject.Find("CenterPiece");
        leftPiece = GameObject.Find("LeftPiece");
        rightPiece = GameObject.Find("RightPiece");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: remove
/*    public void PickUp(GameObject face)
    {
        // start the piece rotation logic
        face.transform.parent.GetComponent<PivotRotation3x1x1>().Rotate(face);
    }*/
}
