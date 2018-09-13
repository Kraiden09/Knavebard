using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudAI : MonoBehaviour {
    public int id;
    AudienceBehave audienceBehave;
    GameObject iam;
    bool inZone;

	// Use this for initialization
	void Start () {
        GetComponent<BoxCollider>().size = new Vector3(6, 4, 6);
        GetComponent<BoxCollider>().isTrigger = true;
        audienceBehave = GameObject.Find("AudienceBehave").GetComponent<AudienceBehave>();
        iam = transform.gameObject;
        inZone = false;
        
    }
	
    /*
	// Update is called once per frame
	void Update () {
        //could eat performance
        if (inZone)
        {
            if (Input.GetKeyDown("return"))
            {
                StartCoroutine(audienceBehave.Jump(iam));
            }
        }
    }
    */

        /*
    // moving by little hops (be as cute as possible)
    public void HopTo(Vector3 destination)
    {
        transform.Translate(destination);
    }

    
    //wiggle to the beat from side to side (while sitting)
    public void Wiggle()
    {

    }
    */
    /*
    public void OnTriggerEnter(Collider other)
    {
        inZone = true;
    }

    public void OnTriggerExit(Collider other)
    {
        inZone = false;
    }
    */

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("return"))
        {
            StartCoroutine(audienceBehave.Jump(iam));
        }
    }
}
