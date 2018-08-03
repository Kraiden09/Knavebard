using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {
    // Exploration Mode (0) or Bard Mode (1)
    private int mode;
    GameObject character = null;
    // Reference to Init Script for Character Creation
    Init initDone;
    bool pressed = false;
    // Use this for initialization
    void Start () {
        // Get Reference to Init Script
        initDone = GameObject.FindObjectOfType(typeof(Init)) as Init;
        mode = 0;
        // Wait for character to be initialized
        StartCoroutine(WaitForInit());
    }
	
	// Update is called once per frame
	void Update () {
        if (character != null) {
            // Exploration Mode
            if (mode == 0) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    character.transform.Translate(0.05f, 0, 0);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    character.transform.Translate(-0.05f, 0, 0);
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    character.transform.Rotate(0, -2.5f, 0);
                }
                if (Input.GetKey(KeyCode.RightArrow)) {
                    character.transform.Rotate(0, 2.5f, 0);
                }
                if (Input.GetKeyDown(KeyCode.Space) && !pressed) {
                    StartCoroutine(Jump());
                    // https://www.gamedev.net/forums/topic/490713-jump-formula/
                }
                // Bard Mode
            } else if (mode == 1) {
                // Gets implemented with note generation
            } 
        }
    }

    // Temporary for Mode Change / will be changed later
    void ModeChange() {

    }

    // Wait for character to be initialized
    IEnumerator WaitForInit() {
        while (!initDone.initialized) {
            yield return new WaitForSeconds(0.1f);
        }
        character = GameObject.Find("Bard");
        character.transform.Translate(-4.23f, 0.606f, 0.98f);
    }

    IEnumerator Jump() {
        pressed = true;
        float jumpStart = character.transform.position.y, aktPos = jumpStart;
        while (aktPos < (jumpStart + 0.5f)) {
            aktPos += 0.05f;
            character.transform.Translate(0, 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        while (aktPos > jumpStart) {
            aktPos -= 0.05f;
            character.transform.Translate(0, -0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        pressed = false;
    }
}
