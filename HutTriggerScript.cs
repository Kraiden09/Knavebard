using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HutTriggerScript : MonoBehaviour {
    private GameObject Hut;
    private GameObject Bard;

    public Text text;
    GameObject interaction;

    // Use this for initialization
    void Start() {
        interaction = GameObject.Find("InteractionText");
        text = interaction.GetComponent<Text>();

        Hut = GameObject.Find("LightHut");
        Bard = GameObject.Find("Bard");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Bard") {
            text.text = "Press H to wear hat";
        }
    }

    // Bard zieht Hut an! 
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Bard") {
            if (Input.GetKeyDown(KeyCode.H)) {
                Hut.transform.position = Bard.transform.position + new Vector3(0, 0.4f, 0);
                Hut.transform.parent = Bard.transform;
                Debug.Log("Läuft bei mir!");
                Destroy(this);
                text.text = "";
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Bard") {
            text.text = "";
        }
    }
}