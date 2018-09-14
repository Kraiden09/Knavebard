using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutTriggerScript : MonoBehaviour {
    private GameObject Hut;
    private GameObject Bard;

    // Use this for initialization
    void Start () {
        Hut = GameObject.Find("LightHut");
        Bard = GameObject.Find("Bard");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Bard") {

        }
    }

    // Bard zieht Hut an! 
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Bard") {
            if (Input.GetKeyDown(KeyCode.H)) {
                Hut.transform.position = Bard.transform.position + new Vector3(0, 0.6f, 0);
                Hut.transform.parent = Bard.transform;
                Debug.Log("Läuft bei mir!");
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Bard") {

        }
    }
}
