using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {
    private GameObject buehne, bard;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitForStage());
        // stageRef.transform.localScale = new Vector3(2, 10, 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WaitForStage() {
        while ((GameObject.Find("Buehne") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        buehne = GameObject.Find("Buehne");

        while ((GameObject.Find("Bard") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        bard = GameObject.Find("Bard");

        
        buehne.AddComponent<BoxCollider>();
        print(buehne);

        Rigidbody bardRB = bard.AddComponent<Rigidbody>();
        bard.AddComponent<SphereCollider>();
        print(bard);
    }


}
