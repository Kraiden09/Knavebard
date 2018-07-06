using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBoard : MonoBehaviour {
    // Reference to stage
    private GameObject stageRef;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitForStage());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Wait for stage to be initialized
    IEnumerator WaitForStage() {
        while((GameObject.Find("Buehne") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        buildBoard();
    }

    void buildBoard() {
        stageRef = GameObject.Find("Tavern").GetComponent<taverne>().getBuehne();

        Mesh stageRefMesh = stageRef.GetComponent<MeshFilter>().mesh;
        Transform stageRefTrans = stageRef.GetComponent<Transform>();

        // Get Length
        float stageLenX = stageRefMesh.bounds.size.x * stageRefTrans.localScale.x;
        float stageLenY = stageRefMesh.bounds.size.y * stageRefTrans.localScale.y;
        float stageLenZ = stageRefMesh.bounds.size.z * stageRefTrans.localScale.z;
        Debug.Log(stageLenX + " " + stageLenY + " " + stageLenZ + " ");

        Vector3 post1Pos = new Vector3(0,0,0), post2Pos = new Vector3(0, 0, 0);
        for (int i = 0; i < stageRefMesh.vertices.Length; i++) {
            if (i == 0) {
                post1Pos = stageRefMesh.vertices[0];
                post2Pos = stageRefMesh.vertices[0];
            }
            // Left post - negative x, positive y and z
            if (stageRefMesh.vertices[i].x <= post1Pos.x && stageRefMesh.vertices[i].y >= post1Pos.y && stageRefMesh.vertices[i].z >= post1Pos.z) {
                post1Pos = stageRefMesh.vertices[i];
            // Right post - positive x, y and z
            } else if (stageRefMesh.vertices[i].x >= post1Pos.x && stageRefMesh.vertices[i].y >= post1Pos.y && stageRefMesh.vertices[i].z >= post1Pos.z) {
                post2Pos = stageRefMesh.vertices[i];
            }
        }

        // Left post for holding the board
        GameObject post1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // Mesh for calculating the height of post
        Mesh post1Mesh = post1.GetComponent<MeshFilter>().mesh;
        // For saving the current scaling
        Transform post1Trans = post1.GetComponent<Transform>();
        // Right post
        GameObject post2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Mesh post2Mesh = post2.GetComponent<MeshFilter>().mesh;
        Transform post2Trans = post2.GetComponent<Transform>();

        post1.transform.localScale = new Vector3(0.5f, 1, 0.5f);
        // Adjust left post position
        post1.transform.position = new Vector3(post1Pos.x + (post1Mesh.bounds.size.x * post1Trans.localScale.x)/2, post1Pos.y + (post1Mesh.bounds.size.y * post1Trans.localScale.y)/2, post1Pos.z);
        post2.transform.localScale = new Vector3(0.5f, 1, 0.5f);
        // Adjust right post position
        post2.transform.position = new Vector3(post2Pos.x - (post2Mesh.bounds.size.x * post1Trans.localScale.x)/2, post2Pos.y + (post2Mesh.bounds.size.y * post2Trans.localScale.y)/2, post2Pos.z);
    }
}
