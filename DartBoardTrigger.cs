using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartBoardTrigger : MonoBehaviour, IObserver, IMinigame {
    Control control;

    GameObject interaction;

    Vector3 startPoint;

    bool isPlaying = false, moving = false, controlInit = false;

    public Text text;

    public void UpdateObserver(Subject subject) {
        if (subject is Control) controlInit = true;
    }

    // Use this for initialization
    void Start () {
        control = GameObject.Find("Control").GetComponent<Control>();
        control.Subscribe(this);

        interaction = GameObject.Find("InteractionText");
        text = interaction.GetComponent<Text>();

        startPoint = transform.position + new Vector3(0, 0, -((GetComponent<BoxCollider>().size.z) / 4));

        //GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //temp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //temp.transform.position = startPoint;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShowText() {
        if (!isPlaying) {
            text.text = "Press \"Return\" to start Playing.";
        } else {
            text.text = "Press \"Return\" to throw Dart or \"Esc\" to stop.";
        }
    }

    void MoveToStartPoint() {
        BardCol bc = GameObject.Find("Bard").GetComponent<BardCol>();
        bc.MoveTo(startPoint, transform.parent.position, this);
    }

    public void InAction() {
        moving = !moving;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Bard") {
            ShowText();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.Return) && !isPlaying) {
            control.SetMinigameMode();
            isPlaying = true;
            ShowText();
            MoveToStartPoint();
        } else if (Input.GetKeyDown(KeyCode.Escape) && isPlaying && !moving) {
            control.SetMinigameMode();
            isPlaying = false;
            ShowText();
        }
    }

    private void OnTriggerExit(Collider other) {
        text.text = "";
    }
}
