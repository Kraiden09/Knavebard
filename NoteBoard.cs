using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBoard : MonoBehaviour {
    // Reference to stage
    private GameObject stageRef, post1, post2, screen;
    private Mesh screenMesh;
    public List<GameObject> notes;
    Vector3 despawn;

    // Refreshes the Noteposition every x Seconds
    float refresh = 0.01f;

    // Move x Units along Screen - Higher number = Faster notes
    float moveAlongScreen = 0.03f;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitForStage());
        StartCoroutine(generateNotes());
    }
	
	// Update is called once per frame
	void Update () {
	}

    void buildBoard() {
        stageRef = GameObject.Find("Tavern").GetComponent<taverne>().getBuehne();

        Mesh stageRefMesh = stageRef.GetComponent<MeshFilter>().mesh;
        Transform stageRefTrans = stageRef.GetComponent<Transform>();

        // Get Length
        /*float stageLenX = stageRefMesh.bounds.size.x * stageRefTrans.localScale.x;
        float stageLenY = stageRefMesh.bounds.size.y * stageRefTrans.localScale.y;
        float stageLenZ = stageRefMesh.bounds.size.z * stageRefTrans.localScale.z;
        Debug.Log(stageLenX + " " + stageLenY + " " + stageLenZ + " ");*/

        buildPosts(stageRefMesh, stageRefTrans);
        buildScreen();
    }

    void buildPosts(Mesh stageRefMesh, Transform stageRefTrans) {
        Vector3 post1Pos = new Vector3(0, 0, 0), post2Pos = new Vector3(0, 0, 0);
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
        post1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // Mesh for calculating the height of post
        Mesh post1Mesh = post1.GetComponent<MeshFilter>().mesh;
        // For saving the current scaling
        Transform post1Trans = post1.GetComponent<Transform>();
        // Right post
        post2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Mesh post2Mesh = post2.GetComponent<MeshFilter>().mesh;
        Transform post2Trans = post2.GetComponent<Transform>();

        post1.transform.localScale = new Vector3(0.5f, 1.2f, 0.5f);
        // Adjust left post position
        post1.transform.position = new Vector3(post1Pos.x + (post1Mesh.bounds.size.x * post1Trans.localScale.x) / 2, post1Pos.y + (post1Mesh.bounds.size.y * post1Trans.localScale.y) / 2, post1Pos.z);
        post2.transform.localScale = new Vector3(0.5f, 1.2f, 0.5f);
        // Adjust right post position
        post2.transform.position = new Vector3(post2Pos.x - (post2Mesh.bounds.size.x * post1Trans.localScale.x) / 2, post2Pos.y + (post2Mesh.bounds.size.y * post2Trans.localScale.y) / 2, post2Pos.z);
        // + 0.1 Units on X to avoid clipping
        despawn = post1.transform.position + new Vector3(0.1f, 0, 0);
    }

    void buildScreen() {
        screenMesh = new Mesh();
        screen = new GameObject("Screen");
        //screen.transform.position = post2.transform.position;

        screen.AddComponent<MeshFilter>();
        screen.AddComponent<MeshRenderer>();
        screen.GetComponent<MeshRenderer>().receiveShadows = false;
        screen.GetComponent<MeshFilter>().mesh = screenMesh;

        List<Vector3> newVertices = new List<Vector3>();

        newVertices.Add(new Vector3(post1.transform.position.x + post1.transform.localScale.x / 2, post1.transform.position.y + 1.1f, post1.transform.position.z - 0.01f));
        newVertices.Add(new Vector3(post1.transform.position.x + post1.transform.localScale.x / 2, post1.transform.position.y - 0.1f, post1.transform.position.z - 0.01f));
        newVertices.Add(new Vector3(post2.transform.position.x - post2.transform.localScale.x / 2, post2.transform.position.y + 1.1f, post2.transform.position.z - 0.01f));
        newVertices.Add(new Vector3(post2.transform.position.x - post2.transform.localScale.x / 2, post2.transform.position.y - 0.1f, post2.transform.position.z - 0.01f));

        screenMesh.vertices = newVertices.ToArray();

        List<int> faces = new List<int>();

        faces.Add(0);
        faces.Add(2);
        faces.Add(1);

        faces.Add(1);
        faces.Add(2);
        faces.Add(3);

        screenMesh.triangles = faces.ToArray();

        screen.GetComponent<Renderer>().material.color = Color.white;
    
        /*GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        test.transform.position = post1.transform.position;
        test.transform.Translate(post1.transform.localScale.x / 2, 1.1f, -0.01f);
        test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        GameObject test2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        test2.transform.position = post1.transform.position;
        test2.transform.Translate(post1.transform.localScale.x / 2, -0.1f, -0.01f);
        test2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);*/
    }

    IEnumerator generateNotes() {
        while (true) {
            GameObject note = (GameObject)Instantiate(Resources.Load("Prefab/NoteUp"));
            /*note.AddComponent<MeshRenderer>();
            Material[] noteMaterials;
            noteMaterials = note.GetComponentInChildren<MeshRenderer>().materials;
            Debug.Log(noteMaterials[0]);

            MeshRenderer fadeMesh = note.GetComponent<MeshRenderer>();
            fadeMesh.enabled = true;

            for (int i = 0; i < noteMaterials.Length; ++i) {
                //ENABLE FADE Mode on the material if not done already
                noteMaterials[i].SetFloat("_Mode", 2);
                noteMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                noteMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                noteMaterials[i].SetInt("_ZWrite", 0);
                noteMaterials[i].DisableKeyword("_ALPHATEST_ON");
                noteMaterials[i].EnableKeyword("_ALPHABLEND_ON");
                noteMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                noteMaterials[i].renderQueue = 3000;
                Debug.Log(noteMaterials[i].GetFloat("_Mode"));

                Color meshColor = noteMaterials[i].color;
                //noteMaterials[i].color = new Color(meshColor.r, meshColor.g, meshColor.b, 0f);
                meshColor.a = 0f;
                noteMaterials[i].color = meshColor;
                Debug.Log(noteMaterials[i].name);
            }*/
            
            note.transform.position = post2.transform.position;
            note.transform.Translate(-0.1f, 0.2f, 0);
            //StartCoroutine(fade(fadeMesh, 2f, true));
            notes.Add(note);
            yield return new WaitForSeconds(1);
        }
    }

    // Wait for stage to be initialized
    IEnumerator WaitForStage() {
        while ((GameObject.Find("Buehne") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        buildBoard();
        StartCoroutine(moveNotes());
    }

    IEnumerator moveNotes() {
        bool remove = false;
        while (true) {
            foreach (GameObject note in notes) {
                note.transform.Translate(-moveAlongScreen, 0, 0);
                if(note.transform.position.x <= despawn.x) {
                    remove = true;
                }
            }
            if (remove) {
                Destroy(notes[0]);
                notes.RemoveAt(0);
                remove = false;
            }
            yield return new WaitForSeconds(refresh);
        }
    }

    /*
    // Based on https://stackoverflow.com/questions/44933517/fading-in-out-gameobject
    IEnumerator fade(MeshRenderer fadingNote, float duration, bool fadeInOut) {
        float start, end;

        // fadeInOut true = fadeIn, fadeInOut false = fadeOut
        if (fadeInOut) {
            start = 0;
            end = 1;
        } else {
            start = 1;
            end = 0;
        }

        //ENABLE FADE Mode on the material if not done already
        fadingNote.material.SetFloat("_Mode", 2);
        fadingNote.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        fadingNote.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        fadingNote.material.SetInt("_ZWrite", 0);
        fadingNote.material.DisableKeyword("_ALPHATEST_ON");
        fadingNote.material.EnableKeyword("_ALPHABLEND_ON");
        fadingNote.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        fadingNote.material.renderQueue = 3000;

        float counter = 0;

        // Get current color
        Color spriteColor = fadingNote.material.color;

        while (counter < duration) {
            counter += Time.deltaTime;

            //Fade from start to end
            float alpha = Mathf.Lerp(start, end, counter / duration);

            //Change alpha only
            fadingNote.material.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);

            //Wait for a frame
            yield return null;
        }
    }*/
}
