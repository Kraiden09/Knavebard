using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardCol : MonoBehaviour, IObserver {
    taverne tavern;
    Control control;
    NoteBoard board;
    Rigidbody rb;

    Vector3 stageMid;

    GameObject[] Tische;

    GameObject bard;
    float movement, rotation;
    int mode;
    bool tavernInit = false, rotating = false, climbing = false, onStage = false;

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
        if (col.gameObject.name != "Boden" && !onStage) rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        // Stairs
        if (col.gameObject.name == "Hand") {
            rb.isKinematic = false;
            Physics.IgnoreCollision(bard.GetComponent<Collider>(), col.collider);
        }
        try {
            if (col.gameObject.name == "Quad") {
                rb.isKinematic = false;
                if (tavernInit) {
                    // Only in Exploration Mode
                    if (mode == 0) {
                        control.ModeChange();
                        ClimbStairs();
                    }
                }
            }
            if (col.gameObject.name == "Buehne" && !climbing && !onStage) {
                CollisionProt();
            }
            if (col.gameObject.name == "Boden") {
                if (onStage) onStage = false;
                control.Landed();
            }


            // By Schwabi ab hier!!!
            if (col.gameObject.name == "wand1") {
                CollisionProt();
            }
            if (col.gameObject.name == "wand2") {
                CollisionProt();
            }
            if (col.gameObject.name == "wand3") {
                CollisionProt();
            }
            if (col.gameObject.name == "wand4") {
                CollisionProt();
            }
            if (col.gameObject.name == "Bar") {
                CollisionProt();
            }

            for (int i = 0; i < Tische.Length; i++) {
                if (col.gameObject.name == ("Tisch" + i)) {
                    CollisionProt();
                }
            }
            // By Schwabi bis hier!!!


        } catch (NullReferenceException) {
            // Do nothing
        }
    }

    private void OnCollisionExit(UnityEngine.Collision col) {
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
    }

    void CollisionProt() {
        Rigidbody rb = bard.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        bard.transform.Translate(-movement * control.GetButtonPressed() * 2, 0, 0);
        rb.isKinematic = false;
        control.SetColProt(true);
        rb.velocity = Vector3.zero;
    }

    void ClimbStairs() {
        board.CreateGuitar();
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

    public bool GetRotating() {
        return rotating;
    }

    public void MoveTo(Vector3 whereToStand, Vector3 lookAt, IMinigame mg) {
        mg.InAction();
        scriptInvolvedInMoving = mg;
        stageMovement = false;
        moveToOtherPoint = true;
        lookAtPoint = lookAt;
        StartCoroutine(Rotate(whereToStand, true, true));
    }

    IMinigame scriptInvolvedInMoving;
    bool stageMovement = true, moveToOtherPoint = false;
    Vector3 lookAtPoint;

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
            } else if (stageMovement) {
                climbing = false;
                control.ModeChange();
                board.ShowGuitar(bard.transform.position);
            } else if (moveToOtherPoint) {
                StartCoroutine(MoveToCR(point, lookAtPoint));
                moveToOtherPoint = false;
            } else {
                if (scriptInvolvedInMoving != null) {
                    scriptInvolvedInMoving.InAction();
                    scriptInvolvedInMoving = null;
                }
                stageMovement = true;
            }
            rotating = false;
        }
    }

    IEnumerator MoveToCR(Vector3 point, Vector3 lookTo) {
        int distanceCounter = 0;
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
        if (distanceCounter >= 2) control.MoveBard("backward");
        StartCoroutine(Rotate(lookTo, false, true));
    }

    IEnumerator MoveOnStage(Vector3 point, bool firstJump) {
        int distanceCounter = 0;
        rotating = false;
        if (climbing) control.JumpExt();
        rb.isKinematic = false;
        float startYBardPos = bard.transform.position.y;
        Vector3 bardPos = bard.transform.position;
        float prevDis;
        float distance = Vector3.Distance(point, bardPos);
        onStage = true;
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
        if (distanceCounter >= 2) control.MoveBard("backward");
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