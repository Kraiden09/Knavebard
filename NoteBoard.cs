using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBoard : MonoBehaviour {
    // Reference to stage
    private GameObject stageRef, post1, post2, screen, borderLeft, borderRight, scoreText;
    private Mesh screenMesh;
    // Flag for Score Coroutine, FPS Cap and to initialize note boundaries
    private bool scoreCRrunning, fpsToLow, initNoteBound;
    private float noteWidth, noteHeight, noteDepth;
    private int fpsCap;
    private GameObject[] despawningNotes;
    private readonly int noteArrSize = 10;
    // Saves last called Score Coroutine
    public Coroutine lastScoreCR;
    public int bad, good, great;
    public List<GameObject> notes = new List<GameObject>();
    public float deltaTime;

    // Current Position in notes List for FadeOut
    public int curNotePos;

    // Current Position in notes List for FadeIn
    int curNoteEndPos;

    Vector3 despawn;

    // Refreshes the Noteposition every x Seconds
    float refresh = 0.01667f;

    // Move x Units along Screen - Higher number = Faster notes
    float moveAlongScreen = 0.04f;

    readonly float fadeBaseTime = 0.2f;
    readonly float moveAlongScreenBase = 0.03f;
    readonly float fadeBaseInterval = 0.04f;
    readonly float fadeInBaseDelay = 0.3f;

    // Time for the note to fade
    float fadeTime;

    // Time for a step of the fade
    float fadeInterval;

    // Time the note stays at Alpha 0f
    float fadeInDelay;

    // calling NoteReader (joerg)
    NoteReader noteReader;

    // For Colliding (Will be replaced by Marcel's Script)
    taverne tavern;
    GameObject floor, stairs;

    // Use this for initialization
    void Start() {
        //JOERG REFERENCE
        noteReader = GameObject.Find("NoteReader").GetComponent<NoteReader>();

        // Collider
        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        floor = tavern.getBoden();
        stageRef = tavern.getBuehne();
        stairs = tavern.getTreppe();
        floor.AddComponent<BoxCollider>();
        stageRef.AddComponent<BoxCollider>();
        stairs.AddComponent<MeshCollider>();

        despawningNotes = new GameObject[noteArrSize];

        bad = 0;
        good = 0;
        great = 0;
        scoreCRrunning = false;
        fpsToLow = false;
        initNoteBound = true;
        lastScoreCR = null;
        curNotePos = 0;
        curNoteEndPos = 0;
        fpsCap = 60;

        // Correct fading
        fadeTime = fadeBaseTime * moveAlongScreenBase / moveAlongScreen; 
        fadeInterval = fadeBaseInterval * moveAlongScreenBase / moveAlongScreen;
        fadeInDelay = fadeInBaseDelay * moveAlongScreenBase / moveAlongScreen;
        acceptance = baseAcceptance * moveAlongScreenBase / moveAlongScreen;
        scoreTime = baseScoreTime * moveAlongScreenBase / moveAlongScreen;

        StartCoroutine(WaitForStage());
    }

    // Update is called once per frame
    void Update() {
        if(ShowFPS() < 50 && !fpsToLow) {
            fpsToLow = true;
            Debug.Log("FPS to low!");
        }
    }

    void BuildBoard() {
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

    void BuildPosts(Mesh stageRefMesh, Transform stageRefTrans) {
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
        post1.transform.position = new Vector3(post1Pos.x + (post1Mesh.bounds.size.x * post1Trans.localScale.x) / 2, post1Pos.y + (post1Mesh.bounds.size.y * post1Trans.localScale.y) / 2, post1Pos.z + 0.1f);
        post2.transform.localScale = new Vector3(0.5f, 1.2f, 0.5f);
        // Adjust right post position
        post2.transform.position = new Vector3(post2Pos.x - (post2Mesh.bounds.size.x * post1Trans.localScale.x) / 2, post2Pos.y + (post2Mesh.bounds.size.y * post2Trans.localScale.y) / 2, post2Pos.z + 0.1f);
        // + baseAcceptance Units on X to avoid clipping
        despawn = post1.transform.position + new Vector3(baseAcceptance + 0.2f, 0, 0);
        post1.name = "LeftPost";
        post2.name = "RightPost";

        Destroy(post1.GetComponent<CapsuleCollider>());
        Destroy(post2.GetComponent<CapsuleCollider>());
    }

    void BuildScreen() {
        screenMesh = new Mesh();
        screen = new GameObject("Screen");
        //screen.transform.position = post2.transform.position;

        screen.AddComponent<MeshFilter>();
        screen.AddComponent<MeshRenderer>();
        screen.GetComponent<MeshRenderer>().receiveShadows = false;
        screen.GetComponent<MeshFilter>().mesh = screenMesh;

        List<Vector3> newVertices = new List<Vector3>();
        float post1XPos = post1.transform.position.x - 0.1f;
        float post1YPos = post1.transform.position.y;
        float post1ZPos = post1.transform.position.z;
        float post2XPos = post2.transform.position.x + 0.1f;
        //float post2YPos = post2.transform.position.y;
        float post2ZPos = post2.transform.position.z;
        float post1XScale = post1.transform.localScale.x;
        float post2XScale = post2.transform.localScale.x;
        // upperLimit and lowerLimit are the same for post1YPos and post2YPos
        float upperLimit = post1YPos + 1.1f;
        float lowerLimit = post1YPos - 0.1f;

        newVertices.Add(new Vector3(post1XPos + post1XScale / 2, upperLimit, post1ZPos - 0.11f));
        newVertices.Add(new Vector3(post1XPos + post1XScale / 2, lowerLimit, post1ZPos - 0.11f));
        newVertices.Add(new Vector3(post2XPos - post2XScale / 2, upperLimit, post2ZPos - 0.11f));
        newVertices.Add(new Vector3(post2XPos - post2XScale / 2, lowerLimit, post2ZPos - 0.11f));

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

    private float rightBorder, leftBorder;

    void BuildHitArea(float length, Vector3 screenPos) {
        screenPos.y = screenPos.y - length / 2;
        screenPos.x = screenPos.x + 0.6f;
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
        borderLeft.GetComponent<Renderer>().material.color = Color.black;
        borderRight.GetComponent<Renderer>().material.color = Color.black;
        rightBorder = borderRight.transform.position.x;
        leftBorder = borderLeft.transform.position.x;

        Destroy(borderLeft.GetComponent<CapsuleCollider>());
        Destroy(borderRight.GetComponent<CapsuleCollider>());
    }

    /*byte RandomColorVal() {
        return (byte)rnd.Next(0, 255);
    }*/

    // For score calculation
    private readonly float baseAcceptance = 0.20f;
    private float acceptance;

    // Note Rating depending on HitArea
    public void ScorePos() {
        Vector3 notePos = notes[0].transform.position;
        // Checks if Coroutine of ShowScore already running
        ScoreCRrunningCheck();
        if (notePos.x > (rightBorder + acceptance) || notePos.x < (leftBorder - acceptance)) {
            // Notescore Bad (0)
            lastScoreCR = StartCoroutine(ShowScore(0));
            bad++;
        } else if (notePos.x < (rightBorder - acceptance) && notePos.x > (leftBorder + acceptance)) {
            // Notescore Great (2)
            lastScoreCR = StartCoroutine(ShowScore(2));
            great++;
        } else {
            // Notescore Good (1)
            lastScoreCR = StartCoroutine(ShowScore(1));
            good++;
        }
        DestroyNote();
    }

    // Starts Coroutine if called from another script
    public void BadScoreExtCall() {
        ScoreCRrunningCheck();
        lastScoreCR = StartCoroutine(ShowScore(0));
    }

    // Checks if Coroutine of ShowScore already running - if it does, kill it
    private void ScoreCRrunningCheck() {
        if (scoreCRrunning) {
            StopCoroutine(lastScoreCR);
            Destroy(scoreText);
            scoreCRrunning = false;
        }
    }

    public void DestroyNote() {
        // Only destroy the note, if position is < the position of the right border + acceptance
        if (notes[0].transform.position.x < borderRight.transform.position.x + baseAcceptance) {
            //StartCoroutine(WaitTillFade());
            ValidateFadeValues();
            StartCoroutine(FadeOut(curNotePos, fadeInterval, fadeTime, false));
        }
    }

    private float ShowFPS() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        return fps;
    }

    public ParticleSystemRenderMode renderMode = ParticleSystemRenderMode.Mesh;

    private void CreateNoteParticle(Vector3 pos) {
        // Create emitter
        GameObject emitter = new GameObject();
        emitter.name = "Emitter";
        // Set emitter position to note position
        emitter.transform.position = pos;
        // Particle System for further customization
        ParticleSystem ps = emitter.AddComponent<ParticleSystem>();
        // Rotate emitter to generate particles away from screen
        emitter.transform.Rotate(0, 180, 0);
        // Set emission var for further customization
        var emission = ps.emission;
        // Set main var for further (general) customization
        var main = ps.main;
        // Particle lifetime
        float lifetime = 0.5f;
        main.startLifetime = lifetime;
        // Allows particles to be customized
        ParticleSystemRenderer emittedParticles = ps.GetComponent<ParticleSystemRenderer>();
        // Emitted Particles Mesh = Sphere
        emittedParticles.renderMode = ParticleSystemRenderMode.Mesh;
        emittedParticles.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        emittedParticles.material = (Material)Resources.Load("Materials/NoteParticleMat");
        // Color Gradient for fading
        var col = ps.colorOverLifetime;
        col.enabled = true;
        Gradient grad = new Gradient();
        // Random Color for every Note PS
        Color newColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(newColor, 0.0f), new GradientColorKey(newColor, 1.0f) }, 
                     new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        col.color = grad;
        //Color newColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //emittedParticles.material.SetColor("_TintColor", newColor);
        main.startSize = 0.05f;
        emission.enabled = true;
        // Emission Rate
        emission.rateOverTime = 70;

        ps.Play();
        //StartCoroutine(Recolor(ps));
        StartCoroutine(StopPS(emitter, ps, 0.5f, lifetime));
    }

    void ValidateFadeValues() {
        // fadeInterval and fadeTime need to be <= 1.0f (Alpha -> fully visible)
        while (fadeInterval > 1f || fadeTime > 1f) {
            if (fadeInterval > 1f) fadeInterval /= 10;
            if (fadeTime > 1f) fadeTime /= 10;
        }
    }

    public void ExtDropCall() {
        StartCoroutine(DropNote(curNotePos));
    }

    IEnumerator DropNote(int myPos) {
        //Debug.Log(myPos);
        despawningNotes[myPos] = notes[0];
        notes.RemoveAt(0);
        curNoteEndPos--;
        curNotePos = (curNotePos + 1) % noteArrSize;
        //notesDespawning++;
        //StartCoroutine(WaitTillFade());
        Rigidbody noteRB = despawningNotes[myPos].AddComponent<Rigidbody>();
        despawningNotes[myPos].AddComponent<SphereCollider>();
        noteRB.AddForce(new Vector3(-fiMoveAlongScreen, 0, 0) * UnityEngine.Random.Range(1, 100), ForceMode.Impulse);
        noteRB.AddForce(new Vector3(0, 0, -fiMoveAlongScreen) * UnityEngine.Random.Range(1, 50), ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        ValidateFadeValues();
        StartCoroutine(FadeOut(myPos, fadeInterval, fadeTime, true));
    }

    IEnumerator FadeIn(int myPos, float increaseInterval, float time, float delay) {
        //Debug.Log("Fade In: " + myPos);
        if (increaseInterval > time) increaseInterval = time;
        curNoteEndPos++;
        // Fading happens over (time / increase) times, which means to reach 1f it has
        // to increase the value by 1f / (time / increaseInterval).
        float increaseVal = 1f / (time / increaseInterval);
        Renderer[] rendererObjects = notes[myPos].GetComponentsInChildren<Renderer>();
        Color col;
        float currentAlpha = 0f;
        // Set Alpha of note after init to 0f
        foreach (Renderer item in rendererObjects) {
            col = item.material.color;
            col.a = 0f;
            item.material.SetColor("_Color", col);
        }
        while (currentAlpha < 1f) {
            if (delay > 0f) {
                yield return new WaitForSeconds(increaseInterval);
                delay -= increaseInterval;
                continue;
            }
            // Not Alpha > 1f
            if (currentAlpha + increaseInterval > 1f) {
                increaseVal = 1f - currentAlpha;
            }
            foreach (Renderer item in rendererObjects) {
                col = item.material.color;
                col.a += increaseVal;
                item.material.SetColor("_Color", col);
            }
            currentAlpha += increaseVal;
            yield return new WaitForSeconds(increaseInterval);
        }        
    }

    IEnumerator FadeOut(int myPos, float decreaseInterval, float time, bool dropped) {
        // Multiple notes Despawning
        //bool multiple = false;
        if (!dropped) {
            despawningNotes[myPos] = notes[0];
            notes.RemoveAt(0);
            curNoteEndPos--;
            curNotePos = (curNotePos + 1) % noteArrSize;
            //notesDespawning++;
        }
        /*if (notesDespawning > 1) {
            multiple = true;
        }*/
        CreateNoteParticle(despawningNotes[myPos].transform.position);
        if (decreaseInterval > time) decreaseInterval = time;
        // Fading happens over (time / decrease) times, which means to reach 0f it has
        // to decrease the value by 1f / (time / decreaseInterval).
        float decreaseVal = 1f / (time / decreaseInterval);
        Renderer[] rendererObjects = despawningNotes[myPos].GetComponentsInChildren<Renderer>();
        Color col;
        float currentAlpha = 1f;
        while (currentAlpha > 0f) {
            // No negative Alpha
            if (currentAlpha - decreaseVal < 0f) {
                decreaseVal = currentAlpha;
            }
            foreach (Renderer item in rendererObjects) {
                col = item.material.color;
                col.a -= decreaseVal;
                item.material.SetColor("_Color", col);
            }
            currentAlpha -= decreaseVal;
            yield return new WaitForSeconds(decreaseInterval);
        }
        /*if (multiple) {
            Destroy(notes[curNotePos-1]);
            notes.RemoveAt(curNotePos-1);
        } else {
            Destroy(notes[myPos]);
            notes.RemoveAt(myPos);
        }*/
        Destroy(despawningNotes[myPos]);
        despawningNotes[myPos] = null;
        //Debug.Log("Despawned");
        //notesDespawning--;
        //curNotePos--;
        //curNoteEndPos--;
    }

    IEnumerator StopPS(GameObject emitter, ParticleSystem ps, float timeToEmit, float lifetime) {
        yield return new WaitForSeconds(timeToEmit);
        ps.Stop();
        StartCoroutine(DeletePS(emitter, ps, lifetime));
    }

    IEnumerator DeletePS(GameObject emitter, ParticleSystem ps, float lifetime) {
        yield return new WaitForSeconds(lifetime);
        Destroy(emitter);
    }

    // TBC - Every Particle has another color (optional)
    IEnumerator Recolor(ParticleSystem ps) {
        ParticleSystem.Particle[] particles;
        particles = new ParticleSystem.Particle[ps.particleCount];
        int num = ps.GetParticles(particles);
        Debug.Log(num);
        yield return new WaitForSeconds(1f);
    }

    private readonly float baseScoreTime = 0.5f;
    private float scoreTime;

    IEnumerator ShowScore(int scoreType) {
        scoreCRrunning = true;
        int destroyCycle = 0;
        while (destroyCycle < 2) {
            if (destroyCycle == 0) {
                switch (scoreType) {
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
            } else {
                Destroy(scoreText);
                scoreCRrunning = false;
            }
            destroyCycle++;
            //yield return new WaitForSeconds(scoreTime * deltaTime * fpsCap);
            yield return new WaitForSeconds(scoreTime);
        }
    }

    // Position of the clipping "area"
    float noteClippingPos;

    IEnumerator GenerateNotes() {
        GameObject note;

        //timing for 1st note
        yield return new WaitForSeconds(1.5f + 0.98360655737f);

        for (int i = 0; i < noteReader.songlength(); i++) {
            //switch (songNotes[i])
            switch (noteReader.readNotes(i)) {
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


            // Get noteWidth and noteWidth
            if (initNoteBound) {
                Mesh noteBody = note.GetComponentsInChildren<MeshFilter>()[1].mesh;
                Transform noteTrans = note.GetComponent<Transform>();
                noteWidth = noteBody.bounds.size.x * noteTrans.localScale.x;
                noteHeight = noteBody.bounds.size.y * noteTrans.localScale.y;
                noteDepth = noteBody.bounds.size.z * noteTrans.localScale.z;
                initNoteBound = false;
            }

            note.transform.position = post2.transform.position;
            note.transform.Translate(-0.1f, 0.2f, -0.1f);

            // Avoid notes clipping
            if (notes.Count != 0) {
                if (notes[notes.Count - 1].transform.position.y <= note.transform.position.y) {
                    noteClippingPos = notes[notes.Count - 1].transform.position.x + noteWidth;
                    if (note.transform.position.x <= noteClippingPos) {
                        note.transform.Translate(0, noteHeight, -noteDepth / 2);
                    }
                }
            }

            //StartCoroutine(fade(fadeMesh, 2f, true));
            notes.Add(note);

            StartCoroutine(FadeIn(curNoteEndPos, fadeInterval, fadeTime, fadeInDelay));

            //yield return new WaitForSeconds(noteTiming[i]);
            //yield return new WaitForSeconds(noteReader.readTime(i) * Time.deltaTime * fpsCap);
            yield return new WaitForSeconds(noteReader.readTime(i));
        }
    }

    // Wait for stage to be initialized
    IEnumerator WaitForStage() {
        while ((GameObject.Find("Buehne") == null)) {
            yield return new WaitForSeconds(0.1f);
        }
        BuildBoard();
        StartCoroutine(GenerateNotes());
        StartCoroutine(MoveNotes());
    }

    float fiMoveAlongScreen;

    IEnumerator MoveNotes() {
        // FI = Framerate Independent
        fiMoveAlongScreen = moveAlongScreen * Time.deltaTime * fpsCap;
        bool remove = false;
        while (true) {
            foreach (GameObject note in notes) {
                note.transform.Translate(-fiMoveAlongScreen, 0, 0);
                if (note.transform.position.x <= despawn.x) {
                    /*if (i < notesDespawning) {
                        i++;
                        continue;
                    }*/
                    remove = true;
                }
            }
            if (remove) {
                lastScoreCR = StartCoroutine(ShowScore(0));
                //DestroyNote();
                StartCoroutine(DropNote(curNotePos));
                bad++;
                remove = false;
            }
            yield return new WaitForSeconds(refresh);
        }
    }

    public GameObject[] getPosts() {
        GameObject[] posts = new GameObject[2];
        // Left Post
        posts[0] = post1;
        // Right Post
        posts[1] = post2;
        return posts;
    }

    public GameObject getScreen() {
        return screen;
    }

    public List<GameObject> getNotes() {
        return notes;
    }

    public GameObject[] getDespawningNotes() {
        return despawningNotes;
    }
}