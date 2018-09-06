﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BardCol : MonoBehaviour {
    GameObject bard;
    taverne tavern;
    float movement, rotation;
    Control control;
    int mode;
    bool tavernInit = false, rotating = false, climbing = false, init = false;

    void Start() {
        control = GameObject.Find("Control").GetComponent<Control>();
        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        StartCoroutine(WaitForTavern());
        mode = control.GetMode();
        StartCoroutine(WaitForInit());
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
        if (init) {
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
            } catch (NullReferenceException) {
                // Do nothing
            }
        }
    }

    void ClimbStairs() {
        if (!climbing) {
            climbing = true;
            Mesh stairMesh = tavern.getTreppe().GetComponent<MeshFilter>().mesh;
            Vector3[] stairVerts = stairMesh.vertices;
            Vector3 firstStep = stairVerts[21];
            firstStep -= new Vector3(-0.06f, 0, (stairVerts[21].z - stairVerts[15].z) / 2);
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
            }
        }
    }

    Rigidbody rb;
    Vector3 stageMid;

    IEnumerator MoveOnStage(Vector3 point, bool firstJump) {
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
                break;
            }
        }

        if (climbing) {
            if (firstJump) {
                rb.isKinematic = true;
                // Get mid-position of stage
                Vector3[] stageMeshVert = tavern.getBuehne().GetComponent<MeshFilter>().mesh.vertices;
                stageMid = new Vector3(stageMeshVert[5].x - ((stageMeshVert[5].x - stageMeshVert[9].x) / 2), stageMeshVert[9].y, stageMeshVert[5].z - ((stageMeshVert[5].z - stageMeshVert[9].z) / 2));
                StartCoroutine(Rotate(new Vector3(point.x + 0.8f, stageMid.y, stageMid.z), false, false));
            } else {
                climbing = false;
                StartCoroutine(Rotate(stageMid, false, false));
            }
        } else {
            StartCoroutine(Rotate(new Vector3(stageMid.x, stageMid.y, stageMid.z - 1), false, true));
        }
    }

    IEnumerator WaitForTavern() {
        while (tavern.getSpawn() == null) {
            yield return new WaitForSeconds(0.1f);
        }
        tavernInit = true;
    }

    IEnumerator WaitForInit() {
        yield return new WaitForSeconds(0.5f);
        init = true;
    }
}