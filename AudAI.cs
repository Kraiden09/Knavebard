using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // moving by little hops (be as cute as possible)
    public void HopTo(Vector3 destination)
    {
        transform.Translate(destination);
    }


    //wiggle to the beat from side to side (while sitting)
    public void Wiggle()
    {

    }
}
