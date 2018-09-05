﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {
    NoteBoard noteBoard;
    taverne tavern;
    BardCol colHandler;
    List<GameObject> notes;
    // Exploration Mode (0) or Bard Mode (1)
    private int mode;
    GameObject character = null;
    // Reference to Init Script for Character Creation
    Init initDone;
    bool pressed = false;
    // Collision Protection stage
    bool colProt = false;

    float movement, rotation;

    public Coroutine jumping;

    // Use this for initialization
    void Start () {
        // Get Reference to Init Script
        //StartCoroutine(BuildUp());
        noteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        notes = noteBoard.notes;
        initDone = GameObject.FindObjectOfType(typeof(Init)) as Init;
        mode = 0;
        jumping = null;
        movement = 0.05f;
        rotation = 2.5f;
        // Wait for character to be initialized
        StartCoroutine(WaitForInit());
    }
	
	// Update is called once per frame
	void Update () {
        if (character != null) {
            // Exploration Mode
            /*if (Input.GetKeyDown(KeyCode.Keypad0)) {
                ModeChange();
            }*/
            if (Input.GetKeyDown(KeyCode.Keypad5)) {
                ShowScore();
            }
            if (mode == 0) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    if (!colProt) {
                        MoveBard("forward");
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    if (!colProt) {
                        MoveBard("backward");
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    RotateBard("left");
                }
                if (Input.GetKey(KeyCode.RightArrow)) {
                    RotateBard("right");
                }
                if (Input.GetKeyDown(KeyCode.Space) && !pressed) {
                    jumping = StartCoroutine(Jump());
                }
                if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
                    colProt = false;
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

    public void MoveBard(string direction) {
        if (direction.Equals("forward")) {
            character.transform.Translate(movement, 0, 0);
        } else {
            character.transform.Translate(-movement, 0, 0);
        }
    }

    public void RotateBard(string direction) {
        if (direction.Equals("right")) {
            character.transform.Rotate(0, rotation, 0);
        } else {
            character.transform.Rotate(0, -rotation, 0);
        }
    }

    public void RotateBard(string direction, float rotationValue) {
        if (direction.Equals("right")) {
            character.transform.Rotate(0, rotationValue, 0);
        } else {
            character.transform.Rotate(0, -rotationValue, 0);
        }
    }

    void LateUpdate() {
        character.transform.localEulerAngles = new Vector3(0, character.transform.localEulerAngles.y, 0);
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

    public int ModeChange() {
        if (mode == 0) {
            Debug.Log("Changed to Climbing Mode");
            mode = 2;
        } else if (mode == 2) {
            Debug.Log("Changed to Bard Mode");
            mode = 1;
            noteBoard.StartNoteGeneration();
        } else { 
            Debug.Log("Changed to Exploration Mode");
            mode = 0;
        }
        UpdateMode();
        return mode;
    }

    void ShowScore() {
        Debug.Log("Great: " + noteBoard.great);
        Debug.Log("Good: " + noteBoard.good);
        Debug.Log("Bad: " + noteBoard.bad);
    }

    public void SetColProt(bool value) {
        colProt = value;
    }

    public void Landed() {
        pressed = false;
        jumping = null;
    }

    public void JumpExt() {
        StartCoroutine(Jump());
    }

    public int GetMode() {
        return mode;
    }

    public void UpdateMode() {
        colHandler.SetMode(mode);
    }

    // Wait for character to be initialized
    IEnumerator WaitForInit() {
        while (!initDone.initialized) {
            yield return new WaitForSeconds(0.1f);
        }
        character = GameObject.Find("Bard");
        StartCoroutine(WaitForTavern());
        StartCoroutine(WaitForRB());
        colHandler = character.AddComponent<BardCol>();
        colHandler.AddColHandler(character, movement, rotation);
        //character.transform.Translate(-4.23f, 0.606f, 0.98f);
    }

    IEnumerator WaitForTavern() {
        while (tavern.getSpawn() == null) {
            yield return new WaitForSeconds(0.1f);
        }
        // Define Spawn position
        Vector3[] spawnPos = tavern.getSpawnVert();
        character.transform.position = new Vector3(spawnPos[0].x, 0.4f, spawnPos[0].z);
        character.transform.Translate((spawnPos[3].x - spawnPos[0].x) / 2, 0, (spawnPos[3].z - spawnPos[0].z) / 2);
        // Set LookAt Point
        character.transform.LookAt(new Vector3(0, 0, 0));
        character.transform.Rotate(0, -90, 0);
    }

    // Wait for Rigid Body
    IEnumerator WaitForRB() {
        while (character.GetComponent<Rigidbody>() == null) {
            yield return new WaitForSeconds(0.1f);
        }
        Rigidbody rb = character.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    /*IEnumerator BuildUp() {
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
        yield return new WaitForSeconds(1);
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = true;
    }*/

    IEnumerator Jump() {
        pressed = true;
        if (jumping == null) {
            float jumpStart = character.transform.position.y, aktPos = jumpStart;
            while (aktPos < (jumpStart + 0.5f)) {
                aktPos += 0.05f;
                character.transform.Translate(0, 0.05f, 0);
                yield return new WaitForSeconds(0.01f);
            }
            /*while (aktPos > jumpStart) {
                aktPos -= 0.05f;
                character.transform.Translate(0, -0.05f, 0);
                yield return new WaitForSeconds(0.01f);
            }*/
        }
        /*pressed = false;
        jumping = null;*/
    }
}
