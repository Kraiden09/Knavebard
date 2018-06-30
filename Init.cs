using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {
    public bool initialized = false;

	// Use this for initialization
	void Start () {
        GameObject bard = (GameObject)Instantiate(Resources.Load("Prefab/Character"));
        bard.name = "Bard";
        initialized = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
