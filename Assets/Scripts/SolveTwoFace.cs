using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;

public class SolveTwoFace : MonoBehaviour
{
    // make private later
    private ReadCube readCube;
    private CubeState cubeState;
    bool doOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Update is called once per frame
    void Update()
    {
        // the time it takes for the tables to load can cause an intermittent glitch
        // where the ReadCube method lags behind the Automate script causing the wrong sides to rotate
        if (CubeState.started && doOnce)
        {
            doOnce = false;
            Solver();
        }
    }

    public void Solver()
    {
        // get the state of the cube as a string from the Kociemba package
        string moveString = cubeState.GetStateString();
        print(moveString);

        // solve the cube
        string info = "";
        // Frist time build the tables
        //string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        // every other time use the method that reads from the pre-generated tables
        // tables take up about 4.2 MB, so decide if i want to build them in advance or make a part
        // of an insulation process, since the package that makes them is tiny in comparison (44KB)
        string solution = Search.solution(moveString, out info);

        // there are other parameters we can play with, like number of moves we can stop at
        // if we don't care about finding shorter solutions
        // experiment with this if releasing for mobile

        // convert the solved moves from a string to a list
        List<string> solutionList = StringToList(solution);

        // automate the list
        Automate.moveList = solutionList;

        print(info);
    }

    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

}
