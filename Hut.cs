using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : MonoBehaviour {
    GameObject LightHut, HutZylinder;
    private AudienceBehave jam;

    public int jamSchwabi;

    // Use this for initialization
    void Start () {
        jam = FindObjectOfType<AudienceBehave>();

        LightHut = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        HutZylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        LightHut.name = "LightHut";
        LightHut.transform.localScale = new Vector3(1, 0.01f, 1);
        LightHut.transform.Translate(0, 0, 0);
        LightHut.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);

        HutZylinder.name = "HutZylinder";
        HutZylinder.transform.localScale = new Vector3(0.8f, 0.15f, 0.8f);
        HutZylinder.transform.Translate(0, 0.15f, 0);
        HutZylinder.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);

        HutZylinder.transform.parent = LightHut.transform;

        LightHut.transform.Translate(6.45f, 0, 3.8f);

        LightHut.GetComponent<CapsuleCollider>().isTrigger = true;
        HutZylinder.GetComponent<CapsuleCollider>().isTrigger = true;

        LightHut.AddComponent<HutTriggerScript>();

        //LightHut.AddComponent<CandleTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
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
                LightHut.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                HutZylinder.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                break;
            case 1:
                //Debug.Log("CASE=gelb");
                LightHut.GetComponent<Renderer>().material.color = new Color(1, 1, 0);
                HutZylinder.GetComponent<Renderer>().material.color = new Color(1, 1, 0);
                break;
            case 2:
                //Debug.Log("CASE=grün");
                LightHut.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                HutZylinder.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                break;
            default:
                //Debug.Log("Lichtwechsel - Fehler");
                break;
        }
    }
}
