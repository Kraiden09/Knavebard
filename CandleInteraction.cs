using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleInteraction : MonoBehaviour, IObserver {
    CandleSpawner cs;
    GameObject[] candles, triggerArea;
    CandleTrigger activeTrigger;
    bool[] isLit;

    public void UpdateObserver(Subject subject) {
        if (subject is CandleSpawner) {
            InitCandles();
        }
    }

    void InitCandles() {
        candles = cs.GetCandles();
        triggerArea = new GameObject[candles.Length];
        isLit = new bool[candles.Length];
        for (int i = 0; i < isLit.Length; i++) {
            isLit[i] = true;
        }
        CreateTriggerArea();
    }

    // Use this for initialization
    void Start () {
        cs = GameObject.Find("CandleSpawner").GetComponent<CandleSpawner>();
        cs.Subscribe(this);

        transform.Translate(new Vector3(0, 10, 0));
        //StartCoroutine(GetCandles());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetActiveTrigger(CandleTrigger ct) {
        activeTrigger = ct;
    }

    public void HideText(float duration) {
        StartCoroutine(ChangeText(duration));
    }

    void CreateTriggerArea() {
        GameObject area;
        BoxCollider col;
        float xLen = 1.5f;
        float yLen = 1;
        float zLen = 2;
        float xAdjust = (xLen - 1) / 2;
        for (int i = 0; i < candles.Length; i++) {
            area = new GameObject("TriggerCandle" + (i + 1));
            area.transform.position = candles[i].transform.position;
            col = area.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(xLen, yLen, zLen);
            if (i == (candles.Length / 2)) xAdjust *= -1;
            area.transform.Translate(new Vector3(xAdjust, -12.388f, 0));
            area.transform.parent = gameObject.transform;
            area.AddComponent<CandleTrigger>();
            triggerArea[i] = area;
        }
    }

    IEnumerator ChangeText(float duration) {
        if (activeTrigger != null) {
            activeTrigger.IsHidden(true);
            activeTrigger.HideText();
            yield return new WaitForSeconds(duration);
            activeTrigger.UnhideText();
            activeTrigger.IsHidden(false);
        } else {
            CandleTrigger[] candleTrigger = new CandleTrigger[triggerArea.Length];
            for (int i = 0; i < triggerArea.Length; i++) {
                candleTrigger[i] = triggerArea[i].GetComponent<CandleTrigger>();
                candleTrigger[i].IsHidden(true);
                candleTrigger[i].HideText();
            }
            yield return new WaitForSeconds(duration);
            for (int i = 0; i < candleTrigger.Length; i++) {
                candleTrigger[i].UnhideText();
                candleTrigger[i].IsHidden(false);
            }
        }
    }

}
