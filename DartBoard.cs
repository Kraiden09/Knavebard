using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DartBoard : MonoBehaviour, IObserver {
    NoteBoard nb;
    GameObject dartBoard, triggerArea;
    BoxCollider triggerCol;

    public void UpdateObserver(Subject subject) {
        if (subject is NoteBoard) CreateDartBoard();
    }

    // Use this for initialization
    void Start() {
        nb = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        nb.Subscribe(this);
    }

    // Update is called once per frame
    void Update() {

    }

    void CreateDartBoard() {
        dartBoard = (GameObject)Instantiate(Resources.Load("Prefab/DartBoard"));
        dartBoard.name = "DartBoard";
        dartBoard.transform.position = nb.getPosts()[1].transform.position;
        dartBoard.transform.Translate(new Vector3(1.3f, 0, 0));
        Destroy(dartBoard.GetComponent<CapsuleCollider>());
        dartBoard.AddComponent<MeshCollider>();
        dartBoard.AddComponent<DartBoardCol>();
        CreateTriggerArea();
    }

    void CreateTriggerArea() {
        float lengthOfArea = 4f;
        triggerArea = new GameObject {
            name = "DartBoardTrigger"
        };
        triggerArea.transform.position = dartBoard.transform.position;
        triggerCol = triggerArea.AddComponent<BoxCollider>();
        triggerCol.isTrigger = true;
        triggerCol.size = new Vector3(1, 1, lengthOfArea);
        triggerArea.transform.Translate(new Vector3(0, -1.2f, -(lengthOfArea / 2)));
        triggerArea.transform.parent = dartBoard.transform;
        triggerArea.AddComponent<DartBoardTrigger>();
    }
}