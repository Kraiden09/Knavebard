using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardCol : MonoBehaviour, IObserver {
    taverne tavern;
    Control control;
    NoteBoard board;

    GameObject[] Tische;

    GameObject bard;
    float movement, rotation;
    int mode;
    bool tavernInit = false, rotating = false, climbing = false;

    public void UpdateObserver(Subject subject) {
        if (subject is taverne) {
            tavernInit = true;

            Tische = GameObject.Find("Tavern").GetComponent<taverne>().getTische();
        }
    }

    void Start() {
        control = GameObject.Find("Control").GetComponent<Control>();

        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        tavern.Subscribe(this);

        board = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();

        //StartCoroutine(WaitForTavern());
        mode = control.GetMode();
    }

    void Update() {

    }

    public void AddColHandler(GameObject chara, float move, float rotate) {
        bard = chara;
        movement = move;
        rotation = rotate;
    }

    public void SetBardRB() {
        rb = bard.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(UnityEngine.Collision col) {
        // Stairs
        if (col.gameObject.name == "Hand") {
            Physics.IgnoreCollision(bard.GetComponent<Collider>(), col.collider);
        }
        try {
            if (col.gameObject.name == "Quad") {
                if (tavernInit) {
                    // Only in Exploration Mode
                    if (mode == 0) {
                        control.ModeChange();
                        ClimbStairs();
                    }
                }
            }
            if (col.gameObject.name == "Buehne" && !climbing) {
                bard.transform.Translate(-movement, 0, 0);
                control.SetColProt(true);
            }
            if (col.gameObject.name == "Boden") {
                control.Landed();
            }


            // By Schwabi ab hier!!!
            if (col.gameObject.name == "wand1") {
                bard.transform.Translate(-movement, 0, 0);
                control.SetColProt(true);
            }
            if (col.gameObject.name == "wand2") {
                bard.transform.Translate(-movement, 0, 0);
                control.SetColProt(true);
            }
            if (col.gameObject.name == "wand3") {
                bard.transform.Translate(-movement, 0, 0);
                control.SetColProt(true);
            }
            if (col.gameObject.name == "wand4") {
                bard.transform.Translate(-movement, 0, 0);
                control.SetColProt(true);
            }
            if (col.gameObject.name == "Bar") {
                bard.transform.Translate(-movement, 0, 0);
                control.SetColProt(true);
            }
            
            for (int i = 0; i < Tische.Length; i++) {
                if (col.gameObject.name == ("Tisch"+i)) {
                    bard.transform.Translate(-movement, 0, 0);
                    control.SetColProt(true);
                }
            }
            // By Schwabi bis hier!!!


        } catch (NullReferenceException) {
            // Do nothing
        }
    }

    void ClimbStairs() {
        if (!climbing) {
            climbing = true;
            GameObject stairs = tavern.getTreppe();
            Mesh stairMesh = tavern.getTreppe().GetComponent<MeshFilter>().mesh;
            Vector3[] stairVerts = stairMesh.vertices;
            Vector3 firstStep = stairs.transform.position;
            //firstStep -= new Vector3(-0.06f, 0, (stairVerts[21].z - stairVerts[15].z) / 2);
            firstStep += stairVerts[29];
            firstStep -= new Vector3(0, 0, (stairVerts[29].z - stairVerts[31].z) / 2);
            /*GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            temp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            temp.transform.position = firstStep;
            */

            // Get Rotation of LookAt and Rotate bard every 0.x seconds
            // Then jump on firstStep
            StartCoroutine(Rotate(firstStep, true, false));
        }
    }

    public void SetMode(int newMode) {
        mode = newMode;
    }

    IEnumerator Rotate(Vector3 point, bool firstRotation, bool stop) {
        /*GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        temp.transform.position = point;
        temp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);*/
        if (!rotating) {
            rotating = true;
            point.y = bard.transform.position.y;
            Vector3 targetDir = point - bard.transform.position;
            Quaternion rotationQuat = Quaternion.Euler(0, 90, 0);
            Vector3 lineOfSight = rotationQuat * bard.transform.forward;

            // Choose the rotation-direction
            String direction;
            if (Vector3.Angle(targetDir, lineOfSight) > Vector3.Angle(targetDir, Quaternion.Euler(0, 1, 0) * lineOfSight)) {
                direction = "right";
            } else {
                direction = "left";
            }

            // Rotate bard towards point
            while (Vector3.Angle(targetDir, lineOfSight) > rotation) {
                control.RotateBard(direction);
                lineOfSight = rotationQuat * bard.transform.forward;
                yield return new WaitForSeconds(0.01f);
            }
            // Last rotation for angle = 0
            control.RotateBard(direction, Vector3.Angle(targetDir, lineOfSight));
            lineOfSight = rotationQuat * bard.transform.forward;
            if (!stop) {
                StartCoroutine(MoveOnStage(point, firstRotation));
            } else {
                rotating = false;
                climbing = false;
                control.ModeChange();
                board.ShowGuitar(bard.transform.position);
            }
        }
    }

    Rigidbody rb;
    Vector3 stageMid;

    IEnumerator MoveOnStage(Vector3 point, bool firstJump) {
        int distanceCounter = 0;
        rotating = false;
        if (climbing) control.JumpExt();
        rb.isKinematic = false;
        float startYBardPos = bard.transform.position.y;
        Vector3 bardPos = bard.transform.position;
        float prevDis;
        float distance = Vector3.Distance(point, bardPos);
        while (distance > movement) {
            prevDis = distance;
            control.MoveBard("forward");
            distance = Vector3.Distance(point, bardPos);
            bardPos = new Vector3(bard.transform.position.x, startYBardPos, bard.transform.position.z);
            yield return new WaitForSeconds(0.01f);
            if (prevDis < distance) {
                distanceCounter++;
                if (distanceCounter >= 2) break;
            }
        }

        if (climbing) {
            if (firstJump) {
                rb.isKinematic = true;
                // Get mid-position of stage
                //Vector3[] stageMeshVert = tavern.getBuehne().GetComponent<MeshFilter>().mesh.vertices;
                GameObject stage = tavern.getBuehne();
                //stageMid = new Vector3(stageMeshVert[5].x - ((stageMeshVert[5].x - stageMeshVert[9].x) / 2), stageMeshVert[9].y, stageMeshVert[5].z - ((stageMeshVert[5].z - stageMeshVert[9].z) / 2));
                stageMid = stage.transform.position;
                StartCoroutine(Rotate(new Vector3(point.x + 0.8f, stageMid.y, stageMid.z), false, false));
            } else {
                climbing = false;
                StartCoroutine(Rotate(stageMid, false, false));
            }
        } else {
            StartCoroutine(Rotate(new Vector3(stageMid.x, stageMid.y, stageMid.z - 1), false, true));
        }
    }

    /*IEnumerator WaitForTavern() {
        while (tavern.getSpawn() == null) {
            yield return new WaitForSeconds(0.1f);
        }
        tavernInit = true;
    }*/
}