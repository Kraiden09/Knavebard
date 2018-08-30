using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceBehave : MonoBehaviour {

    GameObject[] crowd;
    //getting the grades
    NoteBoard NoteBoard;

    //quantification of the mood
    public int jam;
    //Grades given by NoteBoard
    int great, good, bad;

	// Use this for initialization
	void Start () {
        great = 0;
        good = 0;
        bad = 0;

        //measure jam after each beat
        StartCoroutine(WaitForNoteBoard());
        StartCoroutine(WaitBeat());
        
    }
	
	// Update is called once per frame
	void Update () {
    }

    //returns a percentage of good/bad-mooded audiencemembers
    public GameObject[] getAudiencePart(int percentage)
    {


        return crowd;
    }

    //jumping with jam-meter
    public void Rave()
    {

    }


    //formula for jam-meter
    public void MeasureJam()
    {
        bad = NoteBoard.bad;
        good = NoteBoard.good;
        great = NoteBoard.great;
        jam = (great * 2 + good) - bad * 4;
        print("Jam =" + jam);
    }

    //continuous measuremeant after each beat
    IEnumerator  WaitBeat()
    {
        print("JAMMMMM");
        
        yield return new WaitForSeconds(0.49180327868f);
        MeasureJam();
        StartCoroutine(WaitBeat());
    }

    IEnumerator WaitForNoteBoard()
    {
        yield return new WaitForSeconds(0.49180327868f);
        NoteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
    }


    // moving by little hops (be as cute as possible)
    public void HopTo(GameObject destination)
    {

    }
}
