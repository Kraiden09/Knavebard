using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {
    NoteBoard noteBoard;
    List<GameObject> notes;
    // Exploration Mode (0) or Bard Mode (1)
    private int mode;
    GameObject character = null;
    // Reference to Init Script for Character Creation
    Init initDone;
    bool pressed = false;
    // Use this for initialization
    void Start () {
        // Get Reference to Init Script
        noteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        notes = noteBoard.notes;
        initDone = GameObject.FindObjectOfType(typeof(Init)) as Init;
        mode = 0;
        // Wait for character to be initialized
        StartCoroutine(WaitForInit());
    }
	
	// Update is called once per frame
	void Update () {
        if (character != null) {
            // Exploration Mode
            if (Input.GetKeyDown(KeyCode.Keypad0)) {
                ModeChange();
            }
            if (Input.GetKeyDown(KeyCode.Keypad5)) {
                ShowScore();
            }
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
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    CheckNote("NoteUp");
                }
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    CheckNote("NoteDown");
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    CheckNote("NoteLeft");
                }
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    CheckNote("NoteRight");
                }
            } 
        }
    }

    void CheckNote(string arrowType) {
        if (notes.Count != 0) {
            if (notes[0].name.Equals(arrowType)) {
                //Debug.Log(noteBoard.ScorePos());
                noteBoard.ScorePos();
            } else {
                noteBoard.bad++;
                noteBoard.ExtDropCall();
                noteBoard.BadScoreExtCall();
            }
        } else {
            noteBoard.bad++;
            noteBoard.BadScoreExtCall();
        }
    }

    // Temporary for Mode Change / will be changed later
    void ModeChange() {
        if (mode == 0) {
            Debug.Log("Changed to Bard Mode");
            mode = 1;
        } else {
            Debug.Log("Changed to Exploration Mode");
            mode = 0;
        }
    }

    void ShowScore() {
        Debug.Log("Great: " + noteBoard.great);
        Debug.Log("Good: " + noteBoard.good);
        Debug.Log("Bad: " + noteBoard.bad);
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
