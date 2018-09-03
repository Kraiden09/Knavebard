using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* script for audience-behavior and "jam", determining the behavior
 * 
 * Manual: simply attach to an GameObject called "AudienceBehave"
*/
public class AudienceBehave : MonoBehaviour {
    //arrays with all audience-members and hocker
    GameObject[] crowd;
    GameObject[] allHocker;

    //getting the grades
    NoteBoard NoteBoard;
    //get the numbers to spawn
    taverne Taverne;

    //quantification of the mood
    public int jam;
    //Grades given by NoteBoard
    int great, good, bad;

    // Use this for initialization
    void Start() {
        great = 0;
        good = 0;
        bad = 0;

        //measure jam after each beat
        StartCoroutine(WaitForNoteBoard());
        StartCoroutine(WaitBeat());


        //creating Audience


    }

    // Update is called once per frame
    void Update() {
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
        jam = (great * 3 + good) - bad * 5;
        print("Jam =" + jam);
    }

    //continuous measuremeant after each beat
    IEnumerator WaitBeat()
    {
        yield return new WaitForSeconds(0.49180327868f);
        //new Jam
        MeasureJam();
        //infinite measuremeant
        StartCoroutine(WaitBeat());
    }

    IEnumerator WaitForNoteBoard()
    {
        yield return new WaitForSeconds(0.49180327868f);
        NoteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
    }

    //waiting for Tavern and getting the crowdsize and hocker
    IEnumerator WaitForTavern()
    {
        yield return new WaitForSeconds(0.49180327868f);
        Taverne = GameObject.Find("Tavern").GetComponent<taverne>();
        allHocker = Taverne.getHocker();
        crowd = new GameObject[Taverne.getHocker().Length];
    }

    /*
    public struct Person{
        public GameObject Audience;

        

        // moving by little hops (be as cute as possible)
        public void HopTo(Vector3 destination)
        {
            this.Audience.transform.Translate(destination);   
        }
    }
    */
}
