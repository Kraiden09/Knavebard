﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartBoardTrigger : MonoBehaviour, IObserver, IMinigame {
    Control control;

    GameObject interaction, bard, dart, rightHand, dartBoard;
    GameObject[] dartInBoard;

    int counter;

    Rigidbody handRB;

    Vector3 startPoint, samePosition, newPosLeft, newPosRight, handToBoard, throwVector;

    bool isPlaying = false, moving = false, controlInit = false, handsInThrowingPosition = false, finished = true, dartInHand = false, handsInMotion = false, throwing = false, secondsThrowMove = false, flying = false, allowSpawn = true;

    public Text text;

    public void UpdateObserver(Subject subject) {
        if (subject is Control) controlInit = true;
    }

    // Use this for initialization
    void Start () {
        counter = 0;
        control = GameObject.Find("Control").GetComponent<Control>();
        control.Subscribe(this);

        interaction = GameObject.Find("InteractionText");
        text = interaction.GetComponent<Text>();

        dartInBoard = new GameObject[15];

        startPoint = transform.position + new Vector3(0, 0, -((GetComponent<BoxCollider>().size.z) / 4));

        bard = GameObject.Find("Bard");

        samePosition = new Vector3(0, 0, 0);

        dartBoard = GameObject.Find("DartBoard");

        //GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //temp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //temp.transform.position = startPoint;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDartInBoard(GameObject obj) {
        if (dartInBoard[counter] != null) Destroy(dartInBoard[counter]);
        dartInBoard[counter] = obj;
        counter = (counter + 1) % dartInBoard.Length;
    }

    void ShowText() {
        if (!isPlaying) {
            text.text = "Press \"Return\" to start Playing.";
        } else {
            text.text = "Press \"Return\" to throw Dart or \"Esc\" to stop.";
        }
    }

    void MoveToStartPoint() {
        BardCol bc = bard.GetComponent<BardCol>();
        bc.MoveTo(startPoint, transform.parent.position, this);
    }

    public void InAction() {
        moving = !moving;
        Debug.Log("Moving" + moving);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Bard") {
            if (rightHand == null) rightHand = GameObject.Find("RightHandBard");
            if (handRB == null) handRB = rightHand.GetComponent<Rigidbody>();
            ShowText();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (control.GetAllowPlaying() && handsInMotion) {
            handsInMotion = false;
        }
        if (secondsThrowMove && flying && control.GetAllowPlaying()) {
            Debug.Log("1");
            secondsThrowMove = false;
            DartInAir();
            HandsToThrowingPosition();
        } else if (secondsThrowMove && control.GetAllowPlaying()) {
            Debug.Log("2");
            ThrowDart();
        }
        if (isPlaying && !moving && !handsInThrowingPosition && control.GetAllowPlaying()) {
            Debug.Log("4");
            handsInThrowingPosition = true;
            handsInMotion = false;
        }
        else if (isPlaying && !moving && !handsInThrowingPosition && !handsInMotion) {
            Debug.Log("3");
            HandsToThrowingPosition();
        }
        if (isPlaying && !moving && handsInThrowingPosition && !dartInHand && allowSpawn) {
            Debug.Log("5");
            SpawnDart();
            handsInMotion = false;
            allowSpawn = false;
        }
        if (Input.GetKeyDown(KeyCode.Return) && !isPlaying) {
            Debug.Log("6");
            EnterPlayMode();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isPlaying && !moving && !handsInMotion) {
            Debug.Log("7");
            StopPlaying();
        } else if (Input.GetKeyDown(KeyCode.Return) && isPlaying && !moving && dartInHand && !throwing && !handsInMotion) {
            Debug.Log("8");
            ThrowDart();
        }
        if (!isPlaying && !handsInThrowingPosition && !moving && !handRB.isKinematic && !finished) {
            Debug.Log("9");
            control.SetMinigameMode();
            finished = true;
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Debug.Log(isPlaying);
            Debug.Log(moving);
            Debug.Log(dartInHand);
            Debug.Log(throwing);
            Debug.Log(handsInMotion);
            Debug.Log(handsInThrowingPosition);
            Debug.Log(control.GetAllowPlaying());
        }
    }

    private void OnTriggerExit(Collider other) {
        text.text = "";
    }

    public void SetHandsInMotion() {
        handsInMotion = false;
    }

    public void SetSecondThrowMove() {
        secondsThrowMove = false;
        flying = false;
    }

    void SpawnDart() {
        Debug.Log("Spawn");
        dart = (GameObject)Instantiate(Resources.Load("Prefab/DartArrow"));
        dart.name = "DartArrowHand";
        dart.transform.position = rightHand.transform.position;
        dart.transform.Translate(new Vector3(0, 0.1635578f, 0));
        dart.transform.Rotate(new Vector3(0, bard.transform.eulerAngles.y, 0));
        Vector3 tempVec3 = new Vector3(dartBoard.transform.position.x, dart.transform.position.y, dartBoard.transform.position.z);
        float angle = -Vector3.Angle(tempVec3 - dart.transform.position, new Vector3(0, 0, 1));
        dart.transform.Rotate(new Vector3(0, angle, 0));
        dartInHand = true;
    }

    void ThrowDart() {
        Debug.Log("Throwing");
        if (handToBoard == Vector3.zero) {
            handToBoard = dartBoard.transform.position - rightHand.transform.position;
        }
        if (secondsThrowMove && throwing) {
            throwVector = rightHand.transform.position + (handToBoard / 8);
            control.MoveHands(0, 2, 0.01f, GameObject.Find("LeftHandBard").transform.position, throwVector, false);
            flying = true;
            dartInHand = false;
        } else if (!throwing) {
            dart.AddComponent<DartCol>();
            throwing = true;
            throwVector = rightHand.transform.position - (handToBoard / 16);
            if (dart == null) dart = GameObject.Find("DartArrowHand");
            dart.transform.parent = rightHand.transform;
            control.MoveHands(0, 1, 0.02f, GameObject.Find("LeftHandBard").transform.position, throwVector, false);
            secondsThrowMove = true;
        }

    }

    public void AllowSpawn() {
        allowSpawn = true;
    }

    void DartInAir() {
        dart.transform.parent = null;
        dart.transform.Translate(0, 0.05f, 0);
        dart.transform.forward = new Vector3(0, 0, 1);
        dart.transform.LookAt(dartBoard.transform);
        dart.transform.Rotate(0, -90, 0);
        Rigidbody rb = dart.AddComponent<Rigidbody>();
        // Mid : new Vector3(0, 305, 200)
        // Left : new Vector3(-80, 305, 200)
        // Right : new Vector3(0, 305, 200)
        // Bottom: new Vector3(-39, 270, 200)
        // Top: new Vector3(-39, 340, 200)
        rb.AddForce(new Vector3(UnityEngine.Random.Range(-80, 1), UnityEngine.Random.Range(270, 341), 200));
        //rb.AddForce(new Vector3(-39, 305, 200));
    }

    void HandsToThrowingPosition() {
        newPosLeft = bard.transform.position;
        newPosRight = bard.transform.position;
        newPosLeft += new Vector3(-0.601502f, -0.2329173f, 0.029349f);
        newPosRight += new Vector3(0.581498f, 0.2070827f, 0.014349f);
        control.SetAllowPlaying(false);
        handsInMotion = true;
        control.MoveHands(0, 1, 0.02f, newPosLeft, newPosRight, true);
        throwing = false;
    }

    void StopPlaying() {
        control.MoveHandsBack(0, 1, 0.02f);
        isPlaying = false;
        handsInThrowingPosition = false;
        dartInHand = false;
        samePosition = bard.transform.position;
        ShowText();
        for (int i = 0; i < dartInBoard.Length; i++) {
            if (dart != dartInBoard[i]) Destroy(dart);
        }
    }

    void EnterPlayMode() {
        while (Math.Abs(samePosition.z - bard.transform.position.z) < 0.3f) {
            control.MoveBard("Forward");
        }
        moving = false;
        controlInit = false;
        handsInThrowingPosition = false;
        dartInHand = false;
        handsInMotion = false;
        throwing = false;
        secondsThrowMove = false;
        flying = false;
        allowSpawn = true;
        finished = false;
        control.SetMinigameMode();
        isPlaying = true;
        ShowText();
        MoveToStartPoint();
    }
}
