using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : Subject, IObserver {
    private GameObject buehne, bard, bar, boden, wand1, wand2, wand3, wand4, quad, screen, leftpost, rightpost, RightHandBard, LeftHandBard;
    Control control;
    taverne taverne;
    Init init;
    NoteBoard noteBoard;
    int allesInit = 0;

    public void UpdateObserver(Subject subject) {
        if (subject is taverne) {
            WaitForStage();
            allesInit++;
        } else if (subject is Init) {
            WaitForBard();
            allesInit++;
        } else if (subject is NoteBoard) {
            WaitForNoteBoard();
            allesInit++;
        } else if (subject is Control) {
            WaitForBardHands();
        }
        if (allesInit >= 3) {
            NotifyAll();
        }
    }

    // Use this for initialization
    void Start() {
        //StartCoroutine(WaitForStage());
        taverne = GameObject.Find("Tavern").GetComponent<taverne>();
        taverne.Subscribe(this);

        init = GameObject.Find("Init").GetComponent<Init>();
        init.Subscribe(this);

        noteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        noteBoard.Subscribe(this);

        control = GameObject.Find("Control").GetComponent<Control>();
        control.Subscribe(this);
    }

    // Update is called once per frame
    void Update() {

    }

    void WaitForStage() {
        buehne = GameObject.Find("Buehne");
        bar = GameObject.Find("Bar");
        wand1 = GameObject.Find("wand1");
        wand2 = GameObject.Find("wand2");
        wand3 = GameObject.Find("wand3");
        wand4 = GameObject.Find("wand4");
        quad = GameObject.Find("Quad");
        boden = GameObject.Find("Boden");

        GameObject[] Tische = GameObject.Find("Tavern").GetComponent<taverne>().getTische();
        for (int i = 0; i < Tische.Length; i++) {
            Tische[i].AddComponent<BoxCollider>();
        }

        GameObject[] Hocker = GameObject.Find("Tavern").GetComponent<taverne>().getHocker();
        for (int i = 0; i < Hocker.Length; i++) {
            Hocker[i].AddComponent<BoxCollider>();
        }

        buehne.AddComponent<BoxCollider>();
        bar.AddComponent<MeshCollider>();
        wand1.AddComponent<BoxCollider>();
        wand2.AddComponent<BoxCollider>();
        wand3.AddComponent<BoxCollider>();
        wand4.AddComponent<BoxCollider>();
        quad.AddComponent<MeshCollider>();
        boden.AddComponent<BoxCollider>();
    }

    void WaitForBard() {
        bard = GameObject.Find("Bard");

        Collider[] bardCols = bard.GetComponentsInChildren<Collider>();
        for (int i = 0; i < bardCols.Length; i++) {
            Destroy(bardCols[i]);
        }
        bard.AddComponent<Rigidbody>();
        bard.AddComponent<CapsuleCollider>();
    }

    void WaitForNoteBoard() {
        leftpost = GameObject.Find("LeftPost");
        rightpost = GameObject.Find("RightPost");
        screen = GameObject.Find("Screen");

        leftpost.AddComponent<MeshCollider>();
        rightpost.AddComponent<MeshCollider>();
        screen.AddComponent<BoxCollider>();
    }

    void WaitForBardHands() {
        RightHandBard = GameObject.Find("RightHandBard");
        LeftHandBard = GameObject.Find("LeftHandBard");

        Collider RightHandBardCol = RightHandBard.GetComponent<Collider>();
        Destroy(RightHandBardCol);
        Collider LeftHandBardCol = LeftHandBard.GetComponent<Collider>();
        Destroy(LeftHandBardCol);
    }

    /*
    IEnumerator WaitForStage() {
        while ((GameObject.Find("Buehne") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        buehne = GameObject.Find("Buehne");
        while ((GameObject.Find("Bard") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        bard = GameObject.Find("Bard");
        while ((GameObject.Find("Bar") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        bar = GameObject.Find("Bar");
        while ((GameObject.Find("wand1") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        wand1 = GameObject.Find("wand1");
        while ((GameObject.Find("wand2") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        wand2 = GameObject.Find("wand2");
        while ((GameObject.Find("wand3") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        wand3 = GameObject.Find("wand3");
        while ((GameObject.Find("wand4") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        wand4 = GameObject.Find("wand4");
        while ((GameObject.Find("Quad") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        quad = GameObject.Find("Quad");
        while ((GameObject.Find("Screen") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        screen = GameObject.Find("Screen");
        while ((GameObject.Find("LeftPost") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        leftpost = GameObject.Find("LeftPost");
        while ((GameObject.Find("RightPost") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        rightpost = GameObject.Find("RightPost");

        GameObject[] Tische = GameObject.Find("Tavern").GetComponent<taverne>().getTische();
        for (int i = 0; i < Tische.Length; i++) {
            Tische[i].AddComponent<BoxCollider>();
        }

        GameObject[] Hocker = GameObject.Find("Tavern").GetComponent<taverne>().getHocker();
        for (int i = 0; i < Hocker.Length; i++) {
            Hocker[i].AddComponent<BoxCollider>();
        }


        //buehne.AddComponent<BoxCollider>();
        bar.AddComponent<MeshCollider>();
        wand1.AddComponent<BoxCollider>();
        wand2.AddComponent<BoxCollider>();
        wand3.AddComponent<BoxCollider>();
        wand4.AddComponent<BoxCollider>();
        quad.AddComponent<MeshCollider>();
        screen.AddComponent<BoxCollider>();
        //leftpost.AddComponent<MeshCollider>();
        //rightpost.AddComponent<MeshCollider>();

        Debug.Log("CapsuleCol deleted");
        Collider[] bardCols = bard.GetComponentsInChildren<Collider>();
        for (int i = 0; i < bardCols.Length; i++) {
            Destroy(bardCols[i]);
        }
        bard.AddComponent<Rigidbody>();
        bard.AddComponent<BoxCollider>();
    }
    */

    public void SetBardRB() {
        bard.AddComponent<Rigidbody>();
    }
}