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
    GameObject spawn;

    //getting the grades
    NoteBoard NoteBoard;
    //get the numbers to spawn
    taverne Taverne;

    //quantification of the mood
    public int jam;
    //Grades given by NoteBoard
    int great, good, bad;

    float height;

    // Use this for initialization
    void Start() {
        //creating Audience
        StartCoroutine(WaitForTavern());


        //inital notes
        great = 0;
        good = 0;
        bad = 0;

        //measure jam after each beat
        StartCoroutine(WaitForNoteBoard());
        StartCoroutine(WaitBeat());

    }

    /* UPDATE ONLY TO TEST STUFF
    // Update is called once per frame
    void Update() {
        //ONLY FOR TESTING
        if (Input.GetKeyDown("j")) {
            StartCoroutine(Jump(crowd[4]));
        }
    }
    */

    //returns a percentage of good/bad-mooded audiencemembers
    public GameObject[] getAudiencePart(int percentage) {

        //returns all Members of the audience
        return crowd;
    }

    //jumping with jam-meter
    public void Rave() {

        //... jump

    }


    //formula for jam-meter
    public void MeasureJam() {
        bad = NoteBoard.bad;
        good = NoteBoard.good;
        great = NoteBoard.great;
        jam = (great * 3 + good) - bad * 5;

        //for add. Information
        //print("Jam =" + jam);
    }

    //continuous measuremeant after each beat
    IEnumerator WaitBeat() {
        yield return new WaitForSeconds(0.49180327868f * 2);
        //new Jam
        MeasureJam();
        //infinite measuremeant
        StartCoroutine(WaitBeat());

        //TESTING
        /*
        StartCoroutine(Jump(crowd[4]));
        */
        

        //INSERT SEVERAL IF-CASES LATER ON!!!!!!!!!!!
        if(jam == 0) //highest mark
        {
            StartCoroutine(Jump(crowd));
        }
        //...
        //...
        //...
        //lowest mark => nothing happens

    }

    IEnumerator WaitForNoteBoard() {
        yield return new WaitForSeconds(0.49180327868f);
        NoteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
    }

    //waiting for Tavern and getting the crowdsize and hocker
    IEnumerator WaitForTavern() {
        yield return new WaitForSeconds(0.49180327868f);
        Taverne = GameObject.Find("Tavern").GetComponent<taverne>();
        allHocker = Taverne.getHocker();
        crowd = new GameObject[(Taverne.getHocker().Length / 2) + 1 /*+1 for Bartender*/];
        spawn = Taverne.getSpawn();


        //Spawn all Peasants
        crowd[0] = (GameObject)Instantiate(Resources.Load("Prefab/Bartender"), Taverne.getBarkeep(), Quaternion.identity);
        crowd[0].transform.Translate(Vector3.up * 0.25f);


        for (int i = 1; i < crowd.Length; i++) {
            crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 1].transform.position.x, /*allHocker[i].transform.position.y + 0.5f*/ 0.6f, allHocker[i * 2 - 1].transform.position.z), Quaternion.identity);
            crowd[i].name = "Audience" + i;

            //Looking-Direction
            if (i > 3) {
                //crowd[i].transform...
            }
        }


        //variable for jumping
        height = crowd[1].transform.position.y;

        //rotation for Bartender and people at the bar
        if (crowd[0].transform.position.x > 0) {
            crowd[0].transform.Rotate(new Vector3(0, 180, 0));
        } else {
            crowd[1].transform.Rotate(new Vector3(0, 180, 0));
            crowd[2].transform.Rotate(new Vector3(0, 180, 0));
            crowd[3].transform.Rotate(new Vector3(0, 180, 0));
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
    public void HopTo(Vector3 destination) {
        transform.Translate(destination);
    }


    //wiggle to the beat from side to side (while sitting)
    public void Wiggle() {
        transform.Rotate(Vector3.forward * Time.deltaTime);
    }

    //little jumps of excitement; probably not needed anymore
    IEnumerator Jump(GameObject guy) {
        float acceleration = 0;


        //forced jumps, so they dont get stuck in the chair
        guy.transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
        yield return new WaitForEndOfFrame();
        guy.transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
        yield return new WaitForEndOfFrame();


        while (guy.transform.position.y > height) {
            guy.transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration) * 1.5f);


            //loop
            yield return new WaitForEndOfFrame();
            acceleration += 0.1f;
        }

        //end
        yield return 0;
    }

    //jumping for parts of the crowd
    IEnumerator Jump(GameObject[] guys)
    {
        float acceleration = 0;


        //forced jumps, so they dont get stuck in the chair
        for (int i = 0; i < guys.Length; i++)
        {
            guys[i].transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
        }

        yield return new WaitForEndOfFrame();
        for (int i = 0; i < guys.Length; i++)
        {
            guys[i].transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
        }
        yield return new WaitForEndOfFrame();


        while (guys[1].transform.position.y > height)
        {
            for (int i = 0; i < guys.Length; i++)
            {
                guys[i].transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
            }
            
            //loop
            yield return new WaitForEndOfFrame();
            acceleration += 0.1f;
        }

        //end
        yield return 0;
    }
}