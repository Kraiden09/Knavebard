using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : MonoBehaviour {
    GameObject LightHut, HutZylinder;
    private AudienceBehave jam;

    private int tmp = 0;
    private Color red;
    private Color yellow;
    private Color green;

    public int jamSchwabi;

    // Use this for initialization
    void Start () {
        red = new Color(1, 0, 0, 1);
        yellow = new Color(1, 1, 0, 1);
        green = new Color(0, 1, 0, 1);

        jam = FindObjectOfType<AudienceBehave>();

        LightHut = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        HutZylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        LightHut.name = "LightHut";
        LightHut.transform.localScale = new Vector3(1, 0.01f, 1);
        LightHut.transform.Translate(0, 0, 0);
        LightHut.GetComponent<Renderer>().material.color = new Color(1f, 1f, 0f);

        HutZylinder.name = "HutZylinder";
        HutZylinder.transform.localScale = new Vector3(0.8f, 0.15f, 0.8f);
        HutZylinder.transform.Translate(0, 0.15f, 0);
        HutZylinder.GetComponent<Renderer>().material.color = new Color(1f, 1f, 0f);

        HutZylinder.transform.parent = LightHut.transform;

        LightHut.transform.Translate(6.45f, 0, 3.8f);

        LightHut.GetComponent<CapsuleCollider>().isTrigger = true;
        HutZylinder.GetComponent<CapsuleCollider>().isTrigger = true;

        LightHut.AddComponent<HutTriggerScript>();

        //LightHut.AddComponent<CandleTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(LightHut.GetComponent<Renderer>().material.color);
        if (jam.jam < -20) {
            jamSchwabi = 0;
        }
        else if (jam.jam >= -20 && jam.jam < 20) {
            jamSchwabi = 1;
        }
        else {
            jamSchwabi = 2;
        }

        switch (jamSchwabi) {
            case 0:
                //Debug.Log("CASE=rot");
                StartFadeHut(red);
                //LightHut.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                //HutZylinder.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                break;
            case 1:
                //Debug.Log("CASE=gelb");
                StartFadeHut(yellow);
                //LightHut.GetComponent<Renderer>().material.color = new Color(1, 1, 0);
                //HutZylinder.GetComponent<Renderer>().material.color = new Color(1, 1, 0);
                break;
            case 2:
                //Debug.Log("CASE=grün");
                StartFadeHut(green);
                //LightHut.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                //HutZylinder.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                break;
            default:
                //Debug.Log("Lichtwechsel - Fehler");
                break;
        }
    }
    void StartFadeHut(Color Color) {
        StartCoroutine(FadeHutCore(Color));
    }

    IEnumerator FadeHutCore(Color Color) {
        if (tmp == 1) {
            tmp = 0;
            StopCoroutine(FadeHutCore(Color));
        }
        tmp = 1;
        while (Color != LightHut.GetComponent<Renderer>().material.color) {
            if (Color == red) {
                if (LightHut.GetComponent<Renderer>().material.color.r < 1) {
                    LightHut.GetComponent<Renderer>().material.color = LightHut.GetComponent<Renderer>().material.color + new Color(0.02f, 0, 0, 0);
                    HutZylinder.GetComponent<Renderer>().material.color = HutZylinder.GetComponent<Renderer>().material.color + new Color(0.02f, 0, 0, 0);
                }
                if (LightHut.GetComponent<Renderer>().material.color.g > 0) {
                    LightHut.GetComponent<Renderer>().material.color = LightHut.GetComponent<Renderer>().material.color + new Color(0, -0.02f, 0, 0);
                    HutZylinder.GetComponent<Renderer>().material.color = HutZylinder.GetComponent<Renderer>().material.color + new Color(0, -0.02f, 0, 0);
                }
                //Debug.Log("RICHTUNG=rot");
            }
            else if (Color == yellow) {
                if (LightHut.GetComponent<Renderer>().material.color.r < 1) {
                    LightHut.GetComponent<Renderer>().material.color = LightHut.GetComponent<Renderer>().material.color + new Color(0.02f, 0, 0, 0);
                    HutZylinder.GetComponent<Renderer>().material.color = HutZylinder.GetComponent<Renderer>().material.color + new Color(0.02f, 0, 0, 0);
                }
                if (LightHut.GetComponent<Renderer>().material.color.g < 1) {
                    LightHut.GetComponent<Renderer>().material.color = LightHut.GetComponent<Renderer>().material.color + new Color(0, 0.02f, 0, 0);
                    HutZylinder.GetComponent<Renderer>().material.color = HutZylinder.GetComponent<Renderer>().material.color + new Color(0, 0.02f, 0, 0);
                }
                //Debug.Log("RICHTUNG=gelb");
            }
            else if (Color == green) {
                if (LightHut.GetComponent<Renderer>().material.color.r > 0) {
                    LightHut.GetComponent<Renderer>().material.color = LightHut.GetComponent<Renderer>().material.color + new Color(-0.02f, 0, 0, 0);
                    HutZylinder.GetComponent<Renderer>().material.color = HutZylinder.GetComponent<Renderer>().material.color + new Color(-0.02f, 0, 0, 0);
                }
                if (LightHut.GetComponent<Renderer>().material.color.g < 1) {
                    LightHut.GetComponent<Renderer>().material.color = LightHut.GetComponent<Renderer>().material.color + new Color(0, 0.02f, 0, 0);
                    HutZylinder.GetComponent<Renderer>().material.color = HutZylinder.GetComponent<Renderer>().material.color + new Color(0, 0.02f, 0, 0);
                }
                //Debug.Log("RICHTUNG=grün");
            }
            yield return new WaitForSeconds(1f);
        }
        tmp = 0;
        yield return null;
    }
}
