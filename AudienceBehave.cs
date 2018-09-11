using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* script for audience-behavior and "jam", determining the behavior
 * 
 * Manual: simply attach to an GameObject called "AudienceBehave"
*/
public class AudienceBehave : MonoBehaviour {
    //arrays with all audience-members and hocker
    GameObject[] crowd, crowd0, crowd50, crowd75, crowd99, guys, empty;
    GameObject[] allHocker;
    GameObject spawn;

    //is there happy audience?
    bool  happy, happy0, happy50, happy75, happy99, happy100;

    //is everything set?
    bool tavernReady;

    //getting the grades
    NoteBoard NoteBoard;
    //get the numbers to spawn
    taverne Taverne;

    //quantification of the mood
    public int jam;
    //Grades given by NoteBoard
    int great, good, bad;

    float height;
    int look;
    bool notInThere;
    int randSeed;

    // Use this for initialization
    void Start() {
        GameObject mpty = new GameObject();
        empty = new GameObject[] {mpty };
        tavernReady = false;
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

    //returns a percentage of good/bad-mooded audiencemembers PROBABLY NOT NEEDED
    public GameObject[] getAudiencePart(int percentage) {

        //returns all Members of the audience
        return crowd;
    }

    //jumping with jam-meter MAYBE NOT NEEDED
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
        

        //TESTING
        /*
        StartCoroutine(Jump(crowd[4]));
        */


        //INSERT SEVERAL IF-CASES LATER ON!!!!!!!!!!!
        if (jam > 40) //highest mark => everybody parties
        {
            //StartCoroutine(Jump(crowd));

            //guys = crowd;
            happy0 = false;
            happy50 = false;
            happy75 = false;
            happy99 = false;
            happy100 = true;

        }

        if (jam > 30) 
        {
            //everyone except bartender
            //StartCoroutine(Jump(crowd99));

            //guys = crowd99;
            happy0 = false;
            happy50 = false;
            happy75 = false;
            happy99 = true;
            happy100 = false;
        }
        else if (jam > 20){
            //75%
            //StartCoroutine(Jump(crowd75));

            //guys = crowd75;
            happy0 = false;
            happy50 = false;
            happy75 = true;
            happy99 = false;
            happy100 = false;
        }
        else if (jam > 10)
        {
            //50%
            //StartCoroutine(Jump(crowd50));

            //guys = crowd50;
            happy0 = false;
            happy50 = true;
            happy75 = false;
            happy99 = false;
            happy100 = false;
        }
        else if (jam > 0)
        {
            //1 or 2
            //StartCoroutine(Jump(crowd0));
            
            guys = crowd0;
            happy = true;
            happy0 = false;
            happy50 = false;
            happy75 = false;
            happy100 = false;
        }
        
        else
        {
            happy = false;
            happy0 = false;
            happy50 = false;
            happy75 = false;
            happy100 = false;
        }
        //lowest mark => nothing happens
        

        if (tavernReady)
        {
            StartCoroutine(Jump());
            tavernReady = false;
        }

        //infinite measuremeant
        StartCoroutine(WaitBeat());

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
        crowd = new GameObject[(Taverne.getHocker().Length / 2) /*+1 for Bartender*/];
        spawn = Taverne.getSpawn();


        //Spawn all Peasants
        crowd[0] = (GameObject)Instantiate(Resources.Load("Prefab/Bartender"), Taverne.getBarkeep(), Quaternion.identity);
        crowd[0].transform.Translate(Vector3.up * 0.25f);


        for (int i = 1; i < crowd.Length; i++)
        {

            //random choosing of seats
            randSeed = UnityEngine.Random.Range(0, 100);
            if (randSeed % 2 == 0)
            {
                crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 2].transform.position.x, 0.6f, allHocker[i * 2 - 2].transform.position.z), Quaternion.identity);
                crowd[i].name = "Audience" + i;

                if (i > 3)
                {

                    //Looking-Direction
                    look = (int)allHocker[i * 2 - 2].transform.eulerAngles.y;
                    switch (look)
                    {
                        case 0:
                            crowd[i].transform.Rotate(new Vector3(0, 90, 0));
                            break;
                        case 180:
                            crowd[i].transform.Rotate(new Vector3(0, -90, 0));
                            break;
                        case 90:
                            crowd[i].transform.Rotate(new Vector3(0, 0, 0));
                            break;
                        case 270:
                            crowd[i].transform.Rotate(new Vector3(0, 180, 0));
                            break;
                        default:
                            break;
                    }

                }
            }

            else
            {
                crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 1].transform.position.x, 0.62f, allHocker[i * 2 - 1].transform.position.z), Quaternion.identity);
                crowd[i].name = "Audience" + i;

                /*
                //Looking-Direction
                crowd[i].transform.Rotate(new Vector3(0, allHocker[i * 2 - 1].transform.eulerAngles.y - 90, 0));
                */

                if (i > 3)
                {

                    //Looking-Direction
                    look = (int)allHocker[i * 2 - 1].transform.eulerAngles.y;


                    switch (look)
                    {
                        case 0:
                            crowd[i].transform.Rotate(new Vector3(0, 90, 0));
                            break;
                        case 180:
                            crowd[i].transform.Rotate(new Vector3(0, -90, 0));
                            break;
                        case 90:
                            crowd[i].transform.Rotate(new Vector3(0, 0, 0));
                            break;
                        case 270:
                            crowd[i].transform.Rotate(new Vector3(0, +180, 0));
                            break;
                        default:
                            break;
                    }

                }
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
        

        //unnecesserily complicated crowd percentages:
        crowd0 = new GameObject[] {crowd[1],crowd[UnityEngine.Random.Range(3,crowd.Length)] };

        //make safe there is no object twice
        do {

            crowd50 = new GameObject[] { crowd0[0], crowd0[1], crowd[UnityEngine.Random.Range(3, crowd.Length)], crowd[UnityEngine.Random.Range(3, crowd.Length)] };

        } while (crowd50[1] == crowd50[2] || crowd50[1] == crowd50[3] || crowd50[2] == crowd50[3]);

        // PROBABLY AN ERROR SOMEWHERE HERE
        crowd75 = new GameObject[crowd50.Length + 2];
        for (int j = 0; j < crowd50.Length + 2; j++)
        {
            //stuff from crowd50
            if (j < crowd50.Length)
            {
                crowd75[j] = crowd50[j];
            }
            else
            {
                //check if crowd75[j] = crowd[]
                for(int k = 1; k < crowd.Length; k++)
                {
                    notInThere = true;

                    for(int l = 0; l < crowd75.Length; l++)
                    {
                        if (crowd75[l] == crowd[k])
                        {
                            notInThere = false;
                        }
                    }

                    // if object wasnt already in crowd75
                    if (notInThere)
                    {
                        crowd75[j] = crowd[k];
                    }
                }
            }
        }

        crowd99 = new GameObject[crowd.Length - 1];
        for (int anotherInt = 1; anotherInt < crowd.Length; anotherInt++)
        {
            crowd99[anotherInt - 1] = crowd[anotherInt];
        }
        tavernReady = true;
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

    /*
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
    */


    //jumping for parts of the crowd
    IEnumerator Jump(/*GameObject[] guys*/)
    {
        float acceleration = 0;
        
        while (true)
        {
            
            if (jam > 40) //highest mark => everybody parties
            {
                //StartCoroutine(Jump(crowd));

                guys = crowd;
                happy = true;

            }

            if (jam > 30)
            {
                //everyone except bartender
                //StartCoroutine(Jump(crowd99));

                guys = crowd99;
                happy = true;
            }
            else if (jam > 20)
            {
                //75%
                //StartCoroutine(Jump(crowd75));

                guys = crowd75;
                happy = true;
            }
            else if (jam > 10)
            {
                //50%
                //StartCoroutine(Jump(crowd50));

                guys = crowd50;
                happy = true;
            }
            else if (jam > 0)
            {
                //1 or 2
                //StartCoroutine(Jump(crowd0));

                guys = crowd0;
                happy = true;
                
            }

            else
            {
                happy = false;
                
            }

            if (happy)
            {

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
                    acceleration = acceleration + 0.12f;
                }
                acceleration = 0;
            }
            yield return new WaitForSeconds(0.49f);
        }

        //end
        //yield return 0;
    }
}