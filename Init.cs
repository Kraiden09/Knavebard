using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : Subject, IObserver {
    GameObject bard;
    GameObject[] hands;
    CharacterJoint[] joints;
    Vector3[] distanceVector;
    GameObject hand;

    Collision col;

    public void UpdateObserver(Subject subject) {
        if (subject is Collision) {
            CreateHands();
        }
    }

    // Use this for initialization
    void Start () {
        col = GameObject.Find("Collision").GetComponent<Collision>();
        col.Subscribe(this);

        bard = (GameObject)Instantiate(Resources.Load("Prefab/Character"));
        hands = new GameObject[2];
        joints = new CharacterJoint[2];
        distanceVector = new Vector3[2];
        bard.name = "Bard";
        NotifyAll();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake() {
        Application.targetFrameRate = 60;
    }

    /*public void UpdateJoint() {
        StartCoroutine(CreateHands());
    }*/

    public GameObject getBard() {
        return bard;
    }

    private void CreateHands() {
        float handSize = 0.3f;
        for (int i = 0; i < 2; i++) {
            hand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hand.GetComponent<MeshRenderer>().material = (Material)Instantiate(Resources.Load("Materials/Body"));
            hands[i] = hand;

            if (i == 0) hands[i].name = "RightHandBard";
            else hands[i].name = "LeftHandBard";

            hands[i].transform.localScale = new Vector3(handSize, handSize, handSize);

            // Generate Joints for swinging hands
            joints[i] = hands[i].AddComponent<CharacterJoint>();
            joints[i].connectedBody = bard.GetComponent<Rigidbody>();
            joints[i].autoConfigureConnectedAnchor = false;
            // Needs to be 0 else bard will be lifted up
            joints[i].connectedMassScale = 0;
            joints[i].anchor = new Vector3(2f * Mathf.Pow(-1, i), 0.2f, -2f * Mathf.Pow(-1, i));
            joints[i].connectedAnchor = new Vector3(0 * Mathf.Pow(-1, i), 0.5f, 0 * Mathf.Pow(-1, i));
            joints[i].axis = new Vector3(1 * Mathf.Pow(-1, i), 0, -1 * Mathf.Pow(-1, i));
            SoftJointLimit sjl = new SoftJointLimit {
                limit = 50
            };
            joints[i].swing1Limit = sjl;
        }
    }

    /*IEnumerator CreateHands() {
        GameObject hand;
        while (bard.GetComponent<Rigidbody>() == null) {
            yield return new WaitForSeconds(0.1f);
        }
        float handSize = 0.3f;
        for (int i = 0; i < 2; i++) {
            hand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hand.GetComponent<MeshRenderer>().material = (Material)Instantiate(Resources.Load("Materials/Body"));
            hands[i] = hand;

            if (i == 0) hands[i].name = "RightHandBard";
            else hands[i].name = "LeftHandBard";

            hands[i].transform.localScale = new Vector3(handSize, handSize, handSize);

            // Generate Joints for swinging hands
            joints[i] = hands[i].AddComponent<CharacterJoint>();
            joints[i].connectedBody = bard.GetComponent<Rigidbody>();
            joints[i].autoConfigureConnectedAnchor = false;
            // Needs to be 0 else bard will be lifted up
            joints[i].connectedMassScale = 0;
            joints[i].anchor = new Vector3(2f * Mathf.Pow(-1, i), 0.2f, -2f * Mathf.Pow(-1, i));
            joints[i].connectedAnchor = new Vector3(0 * Mathf.Pow(-1, i), 0.5f, 0 * Mathf.Pow(-1, i));
            joints[i].axis = new Vector3(1 * Mathf.Pow(-1, i), 0, -1 * Mathf.Pow(-1, i));
            SoftJointLimit sjl = new SoftJointLimit {
                limit = 50
            };
            joints[i].swing1Limit = sjl;
        }
    }*/

}
