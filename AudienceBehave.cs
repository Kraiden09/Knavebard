using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* script for audience-behavior and "jam", determining the behavior
 * 
 * Manual: simply attach to an GameObject called "AudienceBehave"
*/
public class AudienceBehave : MonoBehaviour, IObserver {
    AudioSource voices;
    AudioClip[] differentVoices;

    //arrays with all audience-members and hocker
    GameObject[] crowd, crowd0, crowd50, crowd75, crowd99, guys, empty;
    GameObject[] allHocker;

    //is there happy audience?
    bool happy;

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

    //for use of AudAI too
    public bool jumping;

    public void UpdateObserver(Subject subject) {
        if (subject is NoteBoard) {
            //measure jam after each beat
            StartCoroutine(WaitBeat());
        } else if (subject is taverne) {
            WaitForTavern();
        }
    }

    // Use this for initialization
    void Start() {
        NoteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        NoteBoard.Subscribe(this);

        Taverne = GameObject.Find("Tavern").GetComponent<taverne>();
        Taverne.Subscribe(this);

        GameObject mpty = new GameObject();
        empty = new GameObject[] { mpty };
        tavernReady = false;
        //creating Audience
        //StartCoroutine(WaitForTavern());

        differentVoices = new AudioClip[4];

        // Audio from Asset Store -> Universal Sound FX by imphenzia
        voices = gameObject.AddComponent<AudioSource>();
        // Bad
        differentVoices[0] = Resources.Load<AudioClip>("Universal Sound FX/AUDIENCES/Medieval_Jousting_Tournament/AUDIENCE_Bhoos_01_stereo");
        // Good
        differentVoices[1] = Resources.Load<AudioClip>("Universal Sound FX/AUDIENCES/Medieval_Jousting_Tournament/AUDIENCE_Claps_and_Cheers_02_stereo");
        // Great
        differentVoices[2] = Resources.Load<AudioClip>("Universal Sound FX/AUDIENCES/Medieval_Jousting_Tournament/AUDIENCE_Claps_and_Cheers_05_stereo");
        // Great to Good
        differentVoices[3] = Resources.Load<AudioClip>("Universal Sound FX/AUDIENCES/Medieval_Jousting_Tournament/AUDIENCE_Claps_and_Bhoos_01_stereo");
        voices.volume = 0.30f;

        //inital notes
        great = 0;
        good = 0;
        bad = 0;

        //measure jam after each beat
        //StartCoroutine(WaitForNoteBoard());


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

    /*NOT NEEDED
//returns a percentage of good/bad-mooded audiencemembers PROBABLY NOT NEEDED
public GameObject[] getAudiencePart(int percentage) {

    //returns all Members of the audience
    return crowd;
}

//jumping with jam-meter MAYBE NOT NEEDED
public void Rave() {

    //... jump

}
*/
    int prevJam = 0;
    bool badPlayed = false, goodPlayed = false, greatPlayed = false;

    void PlayVoice() {
        if (!voices.isPlaying) {
            if (jam < -10) {
                // Bad
                voices.clip = differentVoices[0];
                if (!badPlayed) {
                    voices.Play();
                    badPlayed = true;
                    goodPlayed = false;
                    greatPlayed = false;
                }
            } else if (jam >= -10 && jam <= 10 && jam != 0) {
                // Good
                if (prevJam > jam) voices.clip = differentVoices[3];
                else voices.clip = differentVoices[1];
                if (!goodPlayed) {
                    voices.Play();
                    badPlayed = false;
                    goodPlayed = true;
                    greatPlayed = false;
                }
            } else if (jam > 10) {
                // Great
                voices.clip = differentVoices[2];
                if (!greatPlayed) {
                    voices.Play();
                    badPlayed = false;
                    goodPlayed = false;
                    greatPlayed = true;
                }
            }
        prevJam = jam;
        }
    }


    //formula for jam-meter
    public void MeasureJam() {
        bad = NoteBoard.bad;
        good = NoteBoard.good;
        great = NoteBoard.great;
        jam = (great * 3 + good) - bad * 5;

        PlayVoice();

        //for add. Information
        //print("Jam =" + jam);
    }

    //continuous measuremeant after each beat
    IEnumerator WaitBeat() {
        yield return new WaitForSeconds(0.49180327868f * 2);
        //new Jam
        MeasureJam();

        if (tavernReady) {
            StartCoroutine(Jump());
            tavernReady = false;
        }

        //infinite measuremeant
        StartCoroutine(WaitBeat());
    }

    /*IEnumerator WaitForNoteBoard() {
        yield return new WaitForSeconds(0.49180327868f);
        NoteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
    }*/

    //waiting for Tavern and getting the crowdsize and hocker
    void WaitForTavern() {
        allHocker = Taverne.getHocker();
        crowd = new GameObject[(Taverne.getHocker().Length / 2)]; /*+1 for Bartender*/


        //Spawn all Peasants
        crowd[0] = (GameObject)Instantiate(Resources.Load("Prefab/Bartender"), Taverne.getBarkeep(), Quaternion.identity);
        crowd[0].transform.Translate(Vector3.up * 0.25f);


        for (int i = 1; i < crowd.Length; i++) {

            //random choosing of seats
            randSeed = UnityEngine.Random.Range(0, 100);
            if (randSeed % 2 == 0) {
                crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 2].transform.position.x, 0.514f, allHocker[i * 2 - 2].transform.position.z), Quaternion.identity);
                crowd[i].name = "Audience" + i;

                if (i > 3) {

                    //Looking-Direction
                    look = (int)allHocker[i * 2 - 2].transform.eulerAngles.y;
                    switch (look) {
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
            } else {
                crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 1].transform.position.x, 0.514f, allHocker[i * 2 - 1].transform.position.z), Quaternion.identity);
                crowd[i].name = "Audience" + i;

                /*
                //Looking-Direction
                crowd[i].transform.Rotate(new Vector3(0, allHocker[i * 2 - 1].transform.eulerAngles.y - 90, 0));
                */

                if (i > 3) {

                    //Looking-Direction
                    look = (int)allHocker[i * 2 - 1].transform.eulerAngles.y;


                    switch (look) {
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

            /*
            //Collider for Crowd-Interaction
            crowd[i].GetComponent<BoxCollider>().size = new Vector3(4,4,4);
            crowd[i].GetComponent<BoxCollider>().isTrigger = true;
            */
            crowd[i].GetComponent<AudAI>().id = i;
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
        crowd0 = new GameObject[] { crowd[1], crowd[UnityEngine.Random.Range(3, crowd.Length)] };

        //make safe there is no object twice
        do {

            crowd50 = new GameObject[] { crowd0[0], crowd0[1], crowd[UnityEngine.Random.Range(3, crowd.Length)], crowd[UnityEngine.Random.Range(3, crowd.Length)] };

        } while (crowd50[1] == crowd50[2] || crowd50[1] == crowd50[3] || crowd50[2] == crowd50[3]);

        // PROBABLY AN ERROR SOMEWHERE HERE
        crowd75 = new GameObject[crowd50.Length + 2];
        for (int j = 0; j < crowd50.Length + 2; j++) {
            //stuff from crowd50
            if (j < crowd50.Length) {
                crowd75[j] = crowd50[j];
            } else {
                //check if crowd75[j] = crowd[]
                for (int k = 1; k < crowd.Length; k++) {
                    notInThere = true;

                    for (int l = 0; l < crowd75.Length; l++) {
                        if (crowd75[l] == crowd[k]) {
                            notInThere = false;
                        }
                    }

                    // if object wasnt already in crowd75
                    if (notInThere) {
                        crowd75[j] = crowd[k];
                    }
                }
            }
        }

        crowd99 = new GameObject[crowd.Length - 1];
        for (int anotherInt = 1; anotherInt < crowd.Length; anotherInt++) {
            crowd99[anotherInt - 1] = crowd[anotherInt];
        }
        tavernReady = true;
    }

    //Old IEnumerator

    /*IEnumerator WaitForTavern() {
        yield return new WaitForSeconds(0.49180327868f);
        Taverne = GameObject.Find("Tavern").GetComponent<taverne>();
        allHocker = Taverne.getHocker();
        crowd = new GameObject[(Taverne.getHocker().Length / 2)]; /*+1 for Bartender*/
    /*


    //Spawn all Peasants
    crowd[0] = (GameObject)Instantiate(Resources.Load("Prefab/Bartender"), Taverne.getBarkeep(), Quaternion.identity);
    crowd[0].transform.Translate(Vector3.up * 0.25f);


    for (int i = 1; i < crowd.Length; i++) {

        //random choosing of seats
        randSeed = UnityEngine.Random.Range(0, 100);
        if (randSeed % 2 == 0) {
            crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 2].transform.position.x, 0.6f, allHocker[i * 2 - 2].transform.position.z), Quaternion.identity);
            crowd[i].name = "Audience" + i;

            if (i > 3) {

                //Looking-Direction
                look = (int)allHocker[i * 2 - 2].transform.eulerAngles.y;
                switch (look) {
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
        } else {
            crowd[i] = (GameObject)Instantiate(Resources.Load("Prefab/Aud"), new Vector3(allHocker[i * 2 - 1].transform.position.x, 0.62f, allHocker[i * 2 - 1].transform.position.z), Quaternion.identity);
            crowd[i].name = "Audience" + i;

            /*
            //Looking-Direction
            crowd[i].transform.Rotate(new Vector3(0, allHocker[i * 2 - 1].transform.eulerAngles.y - 90, 0));
            */

    /*if (i > 3) {

        //Looking-Direction
        look = (int)allHocker[i * 2 - 1].transform.eulerAngles.y;


        switch (look) {
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
crowd0 = new GameObject[] { crowd[1], crowd[UnityEngine.Random.Range(3, crowd.Length)] };

//make safe there is no object twice
do {

crowd50 = new GameObject[] { crowd0[0], crowd0[1], crowd[UnityEngine.Random.Range(3, crowd.Length)], crowd[UnityEngine.Random.Range(3, crowd.Length)] };

} while (crowd50[1] == crowd50[2] || crowd50[1] == crowd50[3] || crowd50[2] == crowd50[3]);

// PROBABLY AN ERROR SOMEWHERE HERE
crowd75 = new GameObject[crowd50.Length + 2];
for (int j = 0; j < crowd50.Length + 2; j++) {
//stuff from crowd50
if (j < crowd50.Length) {
    crowd75[j] = crowd50[j];
} else {
    //check if crowd75[j] = crowd[]
    for (int k = 1; k < crowd.Length; k++) {
        notInThere = true;

        for (int l = 0; l < crowd75.Length; l++) {
            if (crowd75[l] == crowd[k]) {
                notInThere = false;
            }
        }

        // if object wasnt already in crowd75
        if (notInThere) {
            crowd75[j] = crowd[k];
        }
    }
}
}

crowd99 = new GameObject[crowd.Length - 1];
for (int anotherInt = 1; anotherInt < crowd.Length; anotherInt++) {
crowd99[anotherInt - 1] = crowd[anotherInt];
}
tavernReady = true;
}*/

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

    //little jumps of excitement
    public IEnumerator Jump(GameObject guy) {
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

        jumping = false;
        //end
        yield return 0;
    }



    //jumping for parts of the crowd
    IEnumerator Jump(/*GameObject[] guys*/) {
        float acceleration = 0;

        while (true) {

            if (jam > 40) //highest mark => everybody parties
            {
                //StartCoroutine(Jump(crowd));

                guys = crowd;
                happy = true;

            }

            if (jam > 30) {
                //everyone except bartender
                //StartCoroutine(Jump(crowd99));

                guys = crowd99;
                happy = true;
            } else if (jam > 20) {
                //75%
                //StartCoroutine(Jump(crowd75));

                guys = crowd75;
                happy = true;
            } else if (jam > 10) {
                //50%
                //StartCoroutine(Jump(crowd50));

                guys = crowd50;
                happy = true;
            } else if (jam > 0) {
                //1 or 2
                //StartCoroutine(Jump(crowd0));

                guys = crowd0;
                happy = true;

            } else {
                happy = false;

            }

            if (happy) {

                //forced jumps, so they dont get stuck in the chair
                for (int i = 0; i < guys.Length; i++) {
                    guys[i].transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
                }

                yield return new WaitForEndOfFrame();
                for (int i = 0; i < guys.Length; i++) {
                    guys[i].transform.Translate(Vector3.up * Time.deltaTime * Mathf.Cos(acceleration));
                }
                yield return new WaitForEndOfFrame();

                while (guys[1].transform.position.y > height) {
                    for (int i = 0; i < guys.Length; i++) {
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