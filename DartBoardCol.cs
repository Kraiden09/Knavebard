using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBoardCol : MonoBehaviour {
    DartBoardTrigger dbt;

    GameObject dart, dartBoard;
    float correctZ;
    Vector3 contactPos;

    void Start() {
        dbt = GameObject.Find("DartBoardTrigger").GetComponent<DartBoardTrigger>();
        correctZ = 0;
    }

    void Update() {

    }

    private void OnCollisionEnter(UnityEngine.Collision col) {
        if (col.gameObject.name.Contains("Dart")) {
            Debug.Log("Col");
            dart = (GameObject)Instantiate(Resources.Load("Prefab/DartArrow"));
            if (correctZ == 0) CorrectContactPoint();
            contactPos = new Vector3(col.contacts[0].point.x, col.contacts[0].point.y, correctZ);
            dart.transform.Rotate(new Vector3(0, -90, 0));
            dart.transform.position = contactPos;
            if (col.contacts.Length > 0) dbt.SetDartInBoard(dart);
            else dbt.SetDartInBoard(null);
            dbt.SetHandsInMotion();
            dart.name = "DartArrowBoard";
            Debug.Log("Destr");
            Destroy(col.gameObject);
            dbt.SetSecondThrowMove();
        }
        
    }

    private void OnCollisionExit(UnityEngine.Collision col) {
        
    }

    void CorrectContactPoint() {
        dartBoard = GameObject.Find("DartBoard");
        float dartBoardBoundsZ = dartBoard.GetComponent<Renderer>().bounds.size.z;
        correctZ = dartBoard.transform.position.z - dartBoardBoundsZ - 0.01f;
    }
}