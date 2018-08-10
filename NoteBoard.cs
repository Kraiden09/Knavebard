using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBoard : MonoBehaviour
{
    // Reference to stage
    private GameObject stageRef, post1, post2, screen, borderLeft, borderRight, scoreText;
    private Mesh screenMesh;
    // Flag for Score Coroutine
    private bool scoreCRrunning;
    // Saves last called Score Coroutine
    public Coroutine lastScoreCR;
    public int bad, good, great;
    public List<GameObject> notes = new List<GameObject>();
    // Random numbers for testing
    System.Random rnd = new System.Random();
    Vector3 despawn;

    // Refreshes the Noteposition every x Seconds
    float refresh = 0.01667f;

    // Move x Units along Screen - Higher number = Faster notes
    float moveAlongScreen = 0.03f;


    //calling NoteReader (joerg)
    NoteReader noteReader;



    // Use this for initialization
    void Start()
    {
        //JOERG REFERENCE
        noteReader = GameObject.Find("NoteReader").GetComponent<NoteReader>();


        StartCoroutine(WaitForStage());
        bad = 0;
        good = 0;
        great = 0;
        scoreCRrunning = false;
        lastScoreCR = null;


    }

    // Update is called once per frame
    void Update()
    {
    }

    void BuildBoard()
    {
        stageRef = GameObject.Find("Tavern").GetComponent<taverne>().getBuehne();

        Mesh stageRefMesh = stageRef.GetComponent<MeshFilter>().mesh;
        Transform stageRefTrans = stageRef.GetComponent<Transform>();

        // Get Length
        /*float stageLenX = stageRefMesh.bounds.size.x * stageRefTrans.localScale.x;
        float stageLenY = stageRefMesh.bounds.size.y * stageRefTrans.localScale.y;
        float stageLenZ = stageRefMesh.bounds.size.z * stageRefTrans.localScale.z;
        Debug.Log(stageLenX + " " + stageLenY + " " + stageLenZ + " ");*/

        BuildPosts(stageRefMesh, stageRefTrans);
        BuildScreen();
    }

    void BuildPosts(Mesh stageRefMesh, Transform stageRefTrans)
    {
        Vector3 post1Pos = new Vector3(0, 0, 0), post2Pos = new Vector3(0, 0, 0);
        for (int i = 0; i < stageRefMesh.vertices.Length; i++)
        {
            if (i == 0)
            {
                post1Pos = stageRefMesh.vertices[0];
                post2Pos = stageRefMesh.vertices[0];
            }
            // Left post - negative x, positive y and z
            if (stageRefMesh.vertices[i].x <= post1Pos.x && stageRefMesh.vertices[i].y >= post1Pos.y && stageRefMesh.vertices[i].z >= post1Pos.z)
            {
                post1Pos = stageRefMesh.vertices[i];
                // Right post - positive x, y and z
            }
            else if (stageRefMesh.vertices[i].x >= post1Pos.x && stageRefMesh.vertices[i].y >= post1Pos.y && stageRefMesh.vertices[i].z >= post1Pos.z)
            {
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
        post1.name = "LeftPost";
        post2.name = "RightPost";
    }

    void BuildScreen()
    {
        screenMesh = new Mesh();
        screen = new GameObject("Screen");
        //screen.transform.position = post2.transform.position;

        screen.AddComponent<MeshFilter>();
        screen.AddComponent<MeshRenderer>();
        screen.GetComponent<MeshRenderer>().receiveShadows = false;
        screen.GetComponent<MeshFilter>().mesh = screenMesh;

        List<Vector3> newVertices = new List<Vector3>();
        float post1XPos = post1.transform.position.x;
        float post1YPos = post1.transform.position.y;
        float post1ZPos = post1.transform.position.z;
        float post2XPos = post2.transform.position.x;
        //float post2YPos = post2.transform.position.y;
        float post2ZPos = post2.transform.position.z;
        float post1XScale = post1.transform.localScale.x;
        float post2XScale = post2.transform.localScale.x;
        // upperLimit and lowerLimit are the same for post1YPos and post2YPos
        float upperLimit = post1YPos + 1.1f;
        float lowerLimit = post1YPos - 0.1f;

        newVertices.Add(new Vector3(post1XPos + post1XScale / 2, upperLimit, post1ZPos - 0.01f));
        newVertices.Add(new Vector3(post1XPos + post1XScale / 2, lowerLimit, post1ZPos - 0.01f));
        newVertices.Add(new Vector3(post2XPos - post2XScale / 2, upperLimit, post2ZPos - 0.01f));
        newVertices.Add(new Vector3(post2XPos - post2XScale / 2, lowerLimit, post2ZPos - 0.01f));

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

        BuildHitArea(upperLimit - lowerLimit, screenMesh.vertices[0]);

        /*GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        test.transform.position = post1.transform.position;
        test.transform.Translate(post1.transform.localScale.x / 2, 1.1f, -0.01f);
        test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        GameObject test2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        test2.transform.position = post1.transform.position;
        test2.transform.Translate(post1.transform.localScale.x / 2, -0.1f, -0.01f);
        test2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);*/
    }

    void BuildHitArea(float length, Vector3 screenPos)
    {
        screenPos.y = screenPos.y - length / 2;
        screenPos.x = screenPos.x + 0.3f;
        // For better visibility
        screenPos.z = screenPos.z + 0.038f;
        borderLeft = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // Rescaling the HitArea according to the size of the screen
        length = length * borderLeft.transform.localScale.y / borderLeft.GetComponent<Renderer>().bounds.size.y;
        borderLeft.name = "BorderLeft";
        borderLeft.transform.position = screenPos;
        borderLeft.transform.localScale = new Vector3(0.1f, length, 0.1f);
        screenPos.x = screenPos.x + 0.8f;
        borderRight = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        borderRight.name = "BorderRight";
        borderRight.transform.position = screenPos;
        borderRight.transform.localScale = new Vector3(0.1f, length, 0.1f);
        Debug.Log(length);
        borderLeft.GetComponent<Renderer>().material.color = Color.black;
        borderRight.GetComponent<Renderer>().material.color = Color.black;
    }

    int RandomNumber()
    {
        return rnd.Next(0, 4);
    }

    // For score calculation
    private float acceptance = 0.2f;

    // Note Rating depending on HitArea
    public void ScorePos()
    {
        Vector3 notePos = notes[0].transform.position;
        float rightBorder = borderRight.transform.position.x;
        float leftBorder = borderLeft.transform.position.x;
        // Checks if Coroutine of ShowScore already running
        ScoreCRrunningCheck();
        if (notePos.x > (rightBorder + acceptance) || notePos.x < (leftBorder - acceptance))
        {
            // Notescore Bad (0)
            lastScoreCR = StartCoroutine(ShowScore(0));
            bad++;
        }
        else if (notePos.x < (rightBorder - acceptance) && notePos.x > (leftBorder + acceptance))
        {
            // Notescore Great (2)
            lastScoreCR = StartCoroutine(ShowScore(2));
            great++;
        }
        else
        {
            // Notescore Good (1)
            lastScoreCR = StartCoroutine(ShowScore(1));
            good++;
        }
        DestroyNote();
    }

    // Starts Coroutine if called from another script
    public void BadScoreExtCall()
    {
        ScoreCRrunningCheck();
        lastScoreCR = StartCoroutine(ShowScore(0));
    }

    // Checks if Coroutine of ShowScore already running - if it does, kill it
    private void ScoreCRrunningCheck()
    {
        if (scoreCRrunning)
        {
            StopCoroutine(lastScoreCR);
            Destroy(scoreText);
            scoreCRrunning = false;
        }
    }

    public void DestroyNote()
    {
        // Only destroy the note, if position is < the position of the right border + acceptance
        if (notes[0].transform.position.x < borderRight.transform.position.x + acceptance)
        {
            Destroy(notes[0]);
            notes.RemoveAt(0);
        }
    }

    IEnumerator ShowScore(int scoreType)
    {
        scoreCRrunning = true;
        int destroyCycle = 0;
        while (destroyCycle < 2)
        {
            if (destroyCycle == 0)
            {
                switch (scoreType)
                {
                    case 0:
                        scoreText = (GameObject)Instantiate(Resources.Load("Prefab/Bad"));
                        scoreText.name = "BadText";
                        break;
                    case 1:
                        scoreText = (GameObject)Instantiate(Resources.Load("Prefab/Good"));
                        scoreText.name = "GoodText";
                        break;
                    case 2:
                        scoreText = (GameObject)Instantiate(Resources.Load("Prefab/Great"));
                        scoreText.name = "GreatText";
                        break;
                    default:
                        scoreText = null;
                        Debug.Log("Error - Not a valid case in \"ShowScore()\"");
                        break;
                }
            }
            else
            {
                Destroy(scoreText);
                scoreCRrunning = false;
            }
            destroyCycle++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator GenerateNotes()
    {
        GameObject note;
        for (int i = 0; i < noteReader.songlength(); i++)
        {
            //switch (songNotes[i])
            switch(noteReader.readNotes(i))
            {
                case 0:
                    note = (GameObject)Instantiate(Resources.Load("Prefab/NoteUp"));
                    note.name = "NoteUp";
                    break;
                case 1:
                    note = (GameObject)Instantiate(Resources.Load("Prefab/NoteRight"));
                    note.name = "NoteRight";
                    break;
                case 2:
                    note = (GameObject)Instantiate(Resources.Load("Prefab/NoteDown"));
                    note.name = "NoteDown";
                    break;
                case 3:
                    note = (GameObject)Instantiate(Resources.Load("Prefab/NoteLeft"));
                    note.name = "NoteLeft";
                    break;
                default:
                    note = (GameObject)Instantiate(Resources.Load("Prefab/NoteUp"));
                    Debug.Log("Error - Not a valid number in \"generateNotes()\"");
                    break;
            }

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
            notes.Add(note);
            //yield return new WaitForSeconds(noteTiming[i]);
            yield return new WaitForSeconds(noteReader.readTime(i));
        }
    }

    // Wait for stage to be initialized
    IEnumerator WaitForStage()
    {
        while ((GameObject.Find("Buehne") == null))
        {
            yield return new WaitForSeconds(0.1f);
        }
        BuildBoard();
        StartCoroutine(GenerateNotes());
        StartCoroutine(MoveNotes());
    }

    IEnumerator MoveNotes()
    {
        bool remove = false;
        while (true)
        {
            foreach (GameObject note in notes)
            {
                note.transform.Translate(-moveAlongScreen, 0, 0);
                if (note.transform.position.x <= despawn.x)
                {
                    remove = true;
                }
            }
            if (remove)
            {
                lastScoreCR = StartCoroutine(ShowScore(0));
                DestroyNote();
                bad++;
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
