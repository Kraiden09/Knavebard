﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : Subject, IObserver {
    NoteBoard noteBoard;
    taverne tavern;
    BardCol colHandler;
    Collision col;
    // Reference to Init Script for Character Creation
    Init init;
    List<GameObject> notes;
    // Exploration Mode (0) or Bard Mode (1)
    private int mode;
    GameObject character = null;
    bool pressed = false;
    // Collision Protection stage
    bool colProt = false;
    bool allowScore = true;

    Vector3 startLeftHand, startRightHand;
    Rigidbody lhrb, rhrb;
    float step;

    Vector3[] spawnPos;

    // 1 = forward, -1 = backward
    int keyPressed = 1;

    GameObject leftHand, rightHand;
    Vector3 handRestingPos;
    bool usingGuitar, handsInit, allowPlaying;

    Rigidbody rb;

    bool debugging = false;

    float movement, rotation;

    public Coroutine jumping;

    AudioClip[] steps;
    AudioSource walking;

    public void UpdateObserver(Subject subject) {
        if (subject is Init) {
            movement = 0.05f;
            rotation = 2.5f;
            character = GameObject.Find("Bard");
            colHandler = character.AddComponent<BardCol>();
        } else if (subject is taverne) {
            spawnPos = tavern.getSpawnVert();
            character.transform.position = new Vector3(spawnPos[0].x, 0.6f, spawnPos[0].z);
            character.transform.Translate((spawnPos[3].x - spawnPos[0].x) / 2, 0, (spawnPos[3].z - spawnPos[0].z) / 2);
            // Set LookAt Point
            character.transform.LookAt(new Vector3(0, 0, 0));
            character.transform.Rotate(0, -90, 0);
            //init.UpdateJoint();
            NotifyAll();
        } else if (subject is Collision) {
            if (character.GetComponent<Rigidbody>() == null) col.SetBardRB();
            colHandler.AddColHandler(character, movement, rotation);
            colHandler.SetBardRB();
            rb = character.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            debugging = true;
        }
    }

    // Use this for initialization
    void Start () {
        // Get Reference to Init Script
        //StartCoroutine(BuildUp());
        noteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();

        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        tavern.Subscribe(this);

        col = GameObject.Find("Collision").GetComponent<Collision>();
        col.Subscribe(this);

        init = GameObject.Find("Init").GetComponent<Init>();
        init.Subscribe(this);

        // null
        keyPressed = 0;

        notes = noteBoard.notes;
        mode = 0;
        jumping = null;
        usingGuitar = false;
        handsInit = false;
        allowPlaying = false;

        steps = new AudioClip[3];

        // Audio from Asset Store -> Universal Sound FX by imphenzia
        walking = gameObject.AddComponent<AudioSource>();
        steps[0] = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/Footsteps/Leather_Wood_Hollow_Jog/FOOTSTEP_Leather_Wood_Hollow_Jog_RR3_mono");
        steps[1] = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/Footsteps/Leather_Wood_Hollow_Jog/FOOTSTEP_Leather_Wood_Hollow_Jog_RR5_mono");
        steps[2] = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/Footsteps/Leather_Wood_Hollow_Jog/FOOTSTEP_Leather_Wood_Hollow_Jog_RR6_mono");
        walking.volume = 0.40f;
    }
	
	// Update is called once per frame
	void Update () {
        if (debugging) {
            if (rb.velocity.x >= 0) {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
                debugging = !debugging;
            }
        }
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
                        keyPressed = 1;
                        MoveBard("forward");
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    if (!colProt) {
                        keyPressed = -1;
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
                    tavern.IsJumping = true;
                }
                if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
                    colProt = false;
                }
                // Bard Mode
            } else if (mode == 1) {
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    StartCoroutine(PlayGuitar());
                    CheckNote("NoteUp");
                }
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    StartCoroutine(PlayGuitar());
                    CheckNote("NoteDown");
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    StartCoroutine(PlayGuitar());
                    CheckNote("NoteLeft");
                }
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    StartCoroutine(PlayGuitar());
                    CheckNote("NoteRight");
                }
            } 
        }
    }

    public int GetButtonPressed() {
        return keyPressed;
    }

    public void MoveBard(string direction) {
        if (!walking.isPlaying) {
            walking.clip = steps[Random.Range(0, steps.Length)];
            walking.Play();
        }
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
        } else if (allowScore) {
            noteBoard.bad++;
            noteBoard.BadScoreExtCall();
        }
    }

    public void ChangeAllowScore() {
        allowScore = !allowScore;
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
            SetColProt(false);
            mode = 0;
        }
        UpdateMode();
        return mode;
    }

    public void SetMinigameMode() {
        if (mode == 0) mode = 3;
        else if (mode == 3) mode = 0;
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

    public void MoveHands(float fadeTimeMusic, float speed, float refreshRate, Vector3 positionLeft, Vector3 positionRight, bool setNewStartPos) {
        StartCoroutine(MoveHandsCR(fadeTimeMusic, speed, refreshRate, positionLeft, positionRight, setNewStartPos));
    }

    public void MoveHandsBack(float fadeTime, float speed, float refreshRate) {
        StartCoroutine(MoveHandsBackCR(fadeTime, speed, refreshRate));
    }

    public bool GetAllowPlaying() {
        return allowPlaying;
    }

    public void SetAllowPlaying(bool val) {
        allowPlaying = val;
    }

    IEnumerator PlayGuitar() {
        if (allowPlaying) {
            if (!usingGuitar) {
                float speed = 3f;
                float step = speed * Time.deltaTime;
                float waitTime = 0.01f;
                usingGuitar = true;
                handRestingPos = rightHand.transform.position;
                Vector3 endPos = handRestingPos + new Vector3(0.3674471f, -0.1562212f, -0.103152f);
                while (rightHand.transform.position != endPos) {
                    rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, endPos, step);
                    yield return new WaitForSeconds(waitTime);
                }
                while (rightHand.transform.position != handRestingPos) {
                    rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, handRestingPos, step);
                    yield return new WaitForSeconds(waitTime);
                }
                usingGuitar = false;
            }
        }
    }

    IEnumerator MoveHandsCR(float fadeTimeMusic, float speed, float refreshRate, Vector3 positionLeft, Vector3 positionRight, bool setNewStartPos) {
        allowPlaying = false;
        if (!handsInit) {
            rightHand = GameObject.Find("RightHandBard");
            leftHand = GameObject.Find("LeftHandBard");
            handsInit = true;
        }

        yield return new WaitForSeconds(fadeTimeMusic);

        lhrb = leftHand.GetComponent<Rigidbody>();
        rhrb = rightHand.GetComponent<Rigidbody>();

        lhrb.isKinematic = true;
        rhrb.isKinematic = true;

        if (setNewStartPos) {
            startLeftHand = leftHand.transform.position;
            startRightHand = rightHand.transform.position;
        }

        /*Vector3 endLeft = startLeftHand + new Vector3(-0.01484f, 0.302f, -0.453464f);
        Vector3 endRight = startRightHand + new Vector3(0.4039322f, 0.2888983f, -0.547344f);*/

        step = speed * Time.deltaTime;

        while (leftHand.transform.position != positionLeft || rightHand.transform.position != positionRight) {
            leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, positionLeft, step);
            rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, positionRight, step);
            //yield return new WaitForSeconds(0.02f);
            yield return new WaitForSeconds(refreshRate);
        }
        allowPlaying = true;
    }

    IEnumerator MoveHandsBackCR(float fadeTime, float speed, float refreshRate) {
        step = speed * Time.deltaTime;
        if (startLeftHand != Vector3.zero && startRightHand != Vector3.zero) {
            while (leftHand.transform.position != startLeftHand && rightHand.transform.position != startRightHand) {
                leftHand.transform.position = Vector3.MoveTowards(leftHand.transform.position, startLeftHand, step);
                rightHand.transform.position = Vector3.MoveTowards(rightHand.transform.position, startRightHand, step);
                yield return new WaitForSeconds(refreshRate);
            }
            lhrb.isKinematic = false;
            rhrb.isKinematic = false;
            allowPlaying = false;
        }
    }

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
