using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* script for audience-behavior and "jam", determining the behavior
 * 
 * Manual: simply attach to an GameObject called "AudienceBehave"
*/
public class AudienceBehave : MonoBehaviour
{
    //arrays with all audience-members and hocker
    GameObject[] crowd;
    GameObject[] allHocker;
    GameObject spawn;

    //getting the grades
    NoteBoard NoteBoard;
    //get the numbers to spawn
    taverne Taverne;

    //quantification of the mood
    public int jam;
    //Grades given by NoteBoard
    int great, good, bad;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(WaitForTavern());



        great = 0;
        good = 0;
        bad = 0;

        //measure jam after each beat
        StartCoroutine(WaitForNoteBoard());
        StartCoroutine(WaitBeat());


        //creating Audience


    }

    // Update is called once per frame
    void Update()
    {
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

        //for add. Information
        //print("Jam =" + jam);
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
        crowd = new GameObject[(Taverne.getHocker().Length / 2) + 1 /*+1 for Bartender*/];
        spawn = Taverne.getSpawn();

        
        //Spawn all Peasants
        crowd[0] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), Taverne.getBarkeep(), Quaternion.identity);
        crowd[0].name = "Bartender";


        for (int i = 1; i < crowd.Length; i++)
        {
            crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3( allHocker[i * 2 -1].transform.position.x, /*allHocker[i].transform.position.y + 0.5f*/ 0.6f, allHocker[i * 2 -1].transform.position.z), Quaternion.identity);
            crowd[i].name = "Audience" + i;
        }
        
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

    //METHODS FOR MOVING AUDIENCE (formerly in AudAI)

    // moving by little hops (be as cute as possible)
    public void HopTo(Vector3 destination)
    {
        transform.Translate(destination);
    }


    //wiggle to the beat from side to side (while sitting)
    public void Wiggle()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime);
    }

    //little jumps of excitement
    public void Jump()
    {
        //seems bad
        //transform.Translate(Vector3.up * Time.deltaTime * 1.5f);


        //Jump by adding force
        GetComponent<Rigidbody>().AddForce(transform.up * Time.deltaTime);
    }
}
