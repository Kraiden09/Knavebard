using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceBehave : MonoBehaviour {

    GameObject[] crowd;

    //quantification of the mood
    int jam;

	// Use this for initialization
	void Start () {
        //measure jam after each beat
        measureJam();
        waitBeat();
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
    public void rave()
    {

    }


    //formula for jam-meter
    public void measureJam()
    {
        // jam =
        
    }

    //continuous measuremeant after each beat
    IEnumerator waitBeat()
    {
        yield return new WaitForSeconds(0.49180327868f);
        measureJam();
        waitBeat();
    }


    // moving by little hops (be as cute as possible)
    public void hopTo(GameObject destination)
    {
     
    }

}
