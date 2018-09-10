﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CandleTrigger : MonoBehaviour {
    public Text text;
    GameObject interaction;
    GameObject bard;
    Init bardInit;
    CandleSpawner cs;
    CandleInteraction ci;
    String tempText;
    int index;
    bool isLit, prev, lastChange, isHidden, active;

    // Use this for initialization
    void Start () {
        interaction = GameObject.Find("InteractionText");
        cs = GameObject.Find("CandleSpawner").GetComponent<CandleSpawner>();
        ci = GameObject.Find("CandleInteraction").GetComponent<CandleInteraction>();
        text = interaction.GetComponent<Text>();
        bardInit = GameObject.FindObjectOfType(typeof(Init)) as Init;
        index = (Int32.Parse(gameObject.name.Substring(gameObject.name.Length - 1))) - 1;
        isLit = true;
        prev = false;
        lastChange = false;
        isHidden = false;
        active = false;
        tempText = "";
        WaitForBard();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Bard") {
            active = true;
            if (!isHidden) {
                SetActiveScript(this);
                ShowText();
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Bard" && !isHidden) {
            if (!lastChange) {
                if (prev == isLit) {
                    ShowText();
                    lastChange = true;
                } else if (Input.GetKeyDown(KeyCode.Return)) {
                    prev = isLit;
                    isLit = cs.ToggleOnOff(index);
                    ShowText();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Bard") {
            active = false;
            if (!isHidden) {
                SetActiveScript(null);
                text.text = "";
            }
        }
        //text.text = "Hello";
    }

    public void SetActiveScript(CandleTrigger ct) {
        ci.SetActiveTrigger(ct);
    }

    public void HideText() {
        if (active) {
            tempText = text.text;
            if (text.text.Equals("")) {
                // Do nothing
            } else {
                text.text = "";
            }
        }
    }

    public void UnhideText() {
        if (active) {
            text.text = tempText;
        }
    }

    public void IsHidden(bool val) {
        isHidden = val;
    }

    void ShowText() {
        if (prev == isLit) {
            text.text = "The candle burned out.";
        } else {
            if (!isLit) {
                text.text = "Press \"Return\" to light candle.";
            } else {
                text.text = "Press \"Return\" to extinguish flame.";
            }
        }
    }

    public void SetIsLit(bool state) {
        isLit = state;
    }

    IEnumerator WaitForBard() {
        while (!bardInit.initialized) {
            yield return new WaitForSeconds(0.1f);
        }
        bard = GameObject.Find("Bard");
    }
}