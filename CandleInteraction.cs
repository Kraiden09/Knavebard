using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleInteraction : MonoBehaviour {
    CandleSpawner cs;
    GameObject[] candles, triggerAreas;
    bool[] isLit;

    // Use this for initialization
    void Start () {
        transform.Translate(new Vector3(0, 10, 0));
		cs = GameObject.Find("CandleSpawner").GetComponent<CandleSpawner>();
        StartCoroutine(GetCandles());
    }
	
	// Update is called once per frame
	void Update () {
		
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
            area.transform.Translate(new Vector3(xAdjust, -2.388f, 0));
            area.transform.parent = gameObject.transform;
            area.AddComponent<CandleTrigger>();
            triggerAreas[i] = area;
        }
    }

    IEnumerator GetCandles() {
        while (!cs.IsInit()) {
            yield return new WaitForSeconds(0.1f);
        }
        candles = cs.GetCandles();
        triggerAreas = new GameObject[candles.Length];
        isLit = new bool[candles.Length];
        for (int i = 0; i < isLit.Length; i++) {
            isLit[i] = true;
        }
        CreateTriggerArea();
    }
}
