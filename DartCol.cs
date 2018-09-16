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
        if (col.gameObject.name.Contains("DartBoard")) {
            dbt.AllowSpawn();
        } else {
            dbt.AllowSpawn();
            Destroy(gameObject);
            dbt.SetSecondThrowMove();
        }
        
    }

    private void OnCollisionExit(UnityEngine.Collision col) {
        
    }
    
}