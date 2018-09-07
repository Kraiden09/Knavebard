using System;
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
    int index;
    bool isLit, prev, lastChange;

    // Use this for initialization
    void Start () {
        interaction = GameObject.Find("InteractionText");
        cs = GameObject.Find("CandleSpawner").GetComponent<CandleSpawner>();
        text = interaction.GetComponent<Text>();
        bardInit = GameObject.FindObjectOfType(typeof(Init)) as Init;
        index = (Int32.Parse(gameObject.name.Substring(gameObject.name.Length - 1))) - 1;
        isLit = true;
        prev = false;
        lastChange = false;
        WaitForBard();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        ShowText();
    }

    private void OnTriggerStay(Collider other) {
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

    private void OnTriggerExit(Collider other) {
        text.text = "";
        //text.text = "Hello";
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
