using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartCol : MonoBehaviour {
    DartBoardTrigger dbt;

    void Start() {
        dbt = GameObject.Find("DartBoardTrigger").GetComponent<DartBoardTrigger>();
    }

    void Update() {

    }

    private void OnCollisionEnter(UnityEngine.Collision col) {
        Debug.Log(col.gameObject);
        if (col.gameObject.name.Contains("DartBoard")) {
            Debug.Log("Destroyed1");
            dbt.AllowSpawn();
        } else {
            Debug.Log("Destroyed2");
            dbt.AllowSpawn();
            Destroy(gameObject);
            dbt.SetSecondThrowMove();
        }
        
    }

    private void OnCollisionExit(UnityEngine.Collision col) {
        
    }
    
}