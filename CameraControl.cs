using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* simpelste Bewegung der Kamera innerhalb simpler Grenzen
 * bisher: oben, unten, links, rechts, zoom, zoomout
 * 
 * MANUAL: 1st: add Script to MainCamera Object in Unity
 * 
 * Switching Modes: Press "1"
 * 
 * During Manual-Mode:
 * "WASD" for common directions
 * "PageUp" and "PageDown" for zooming in and out
 * "Q" and "E" for switching between preset Camerapositions
 * 
 * 
 */
public class CameraControl : MonoBehaviour {
    //references to noteboard
    NoteBoard noteBoard;
    GameObject screen;

    //deactivate Canvas
    CandleInteraction Candle;

    //Bewegungsgeschwindigkeit
    float camSpeed;
    float borderx, bordery, borderz;
    public bool director;
    bool waiting;
    int random, randomWays, lastPos;

    //cullingMask of camera for reactivation
    int mask;

    //position in CamPos-Array for manual control
    int current;
    bool prev, next;

    //camera reached target
    bool there;


    //Fixpunkt
    Vector3 fix;

    /*
    // used structs
    CamPos[] Positions;
    CamPos standard;
    CamPos front;
    CamPos left;
    CamPos right;
    */

    //Camerapositions
    GameObject standard;
    GameObject front;
    GameObject leftfront;
    GameObject rightfront;

    GameObject[] Positions;

    //future positions
    GameObject roof; //maybekn
    GameObject leftrear;
    GameObject rightrear;


    //NORMAL CAMERA POSITION IN UNITY: 0, 2.8f, -7
    // Use this for initialization
    void Start() {
        //Blackscreen on startup
        mask = GetComponent<Camera>().cullingMask;
        GetComponent<Camera>().cullingMask = 0;
        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;

        Candle = GameObject.Find("CandleInteraction").GetComponent<CandleInteraction>();


        //indirekte Grenzen durch urspruengliche Kameraposition
        borderx = transform.position.x;
        bordery = transform.position.y;
        borderz = transform.position.z + 4;
        camSpeed = 1.5f;
        current = 0;
        prev = false;
        next = false;

        //"Flags"
        director = false;
        waiting = false;

        /*
        //Structs
        standard = new CamPos(0, 1, -8, 0, 0);
        front = new CamPos(0, 1.8f, -2.5f,0,0);
        left = new CamPos(-4.4f, 2f, -0.5f, 0, 25);

        //Order to switch between Positions
        Positions = new CamPos[] { standard,front, left};
        */


        standard = new GameObject("StandardPos");
        standard.transform.Translate(0, 2.7f, -8);
        front = new GameObject("FrontPos");
        front.transform.Translate(0, 1.8f, -2.5f);
        leftfront = new GameObject("LeftFrontPos");
        leftfront.transform.Translate(-4.5f, 2, -0.05f);
        rightfront = new GameObject("RightFrontPos");
        rightfront.transform.Translate(4.5f, 2, -0.05f);
        leftrear = new GameObject("LeftRearPos");
        leftrear.transform.Translate(-4.5f, 4, -7);
        rightrear = new GameObject("RightRearPos");
        rightrear.transform.Translate(4.5f, 4, -7);


        Positions = new GameObject[] { standard, front, leftfront, rightfront, rightrear, leftrear };

        /*
        //References (not necessary)
        noteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        screen = noteBoard.getScreen();
        */

        //standard fixpoint for bard-mode (maybe later changed for free-roam)
        fix = new Vector3(0, 2.7f, 6.15f);
        //transform.Translate(0,2.7f,-8);

        //activate camera after startup
        StartCoroutine(WaitForIt());

    }

    // Update is called once per frame
    void Update() {
        
        //switch
        if (Input.GetKeyDown("1")) {
            director = !director;
        }
        

        //DIRECTOR-MODE
        if (director) {
            //moving camera from pos to pos
            //waiting in between changes
            if (!waiting) {
                StartCoroutine(sleeper());
                while (lastPos == random) {
                    random = UnityEngine.Random.Range(0, Positions.Length);
                }
            }

            //moving
            if (waiting && !there) {
                CamMoveTo(Positions[random]);

                if (Positions[random].transform.position == transform.position) {
                    there = true;

                    //lastPos to eliminate returning to the last position
                    lastPos = random;
                }
            }
            if (there) {
                //slowly moving in random direction
                //int randomWays used
                CameraMoveSC();
            }

        }


        //MANUAL-MODE
        if (!director) {
            //w-a-s-d
            if (Input.GetKey(KeyCode.W)) {
                //Grenze
                if (transform.position.y - bordery < 1.5f) {
                    transform.Translate(new Vector3(0, camSpeed * Time.deltaTime, 0));
                    transform.LookAt(fix);
                }
            }

            if (Input.GetKey(KeyCode.A)) {
                if (transform.position.x - borderx > -6) {
                    transform.Translate(new Vector3(-camSpeed * Time.deltaTime, 0, 0));
                    transform.LookAt(fix);
                }
            }

            if (Input.GetKey(KeyCode.S)) {
                if (transform.position.y - bordery > -0.5f) {
                    transform.Translate(new Vector3(0, -camSpeed * Time.deltaTime, 0));
                    transform.LookAt(fix);
                }
            }

            if (Input.GetKey(KeyCode.D)) {
                if (transform.position.x - borderx < 6) {
                    transform.Translate(new Vector3(camSpeed * Time.deltaTime, 0, 0));
                    transform.LookAt(fix);
                }
            }

            //Zoom + Zoom out       OR MAYBE NOT BECAUSE OF UNFORSEEN STUFF
            /*
            if (Input.GetKey(KeyCode.PageUp)) {
                if (transform.position.z - borderz < 5) {
                    transform.Translate(new Vector3(0, 0, camSpeed * Time.deltaTime));
                }
            }

            if (Input.GetKey(KeyCode.PageDown)) {
                if (transform.position.z - borderz > -1) {
                    transform.Translate(new Vector3(0, 0, -camSpeed * Time.deltaTime));
                }
            }
            */

            //manual change of Camera-Position
            if (Input.GetKeyDown("q")) {
                current--;
                prev = true;
                next = false;
            }

            if (Input.GetKeyDown("e")) {
                current++;
                next = true;
                prev = false;
            }

            if (prev) {
                //CamMoveTo(Positions[(Positions.Length - 1) - Mathf.Abs(current % (Positions.Length - 1))]);
                CamMoveTo(Positions[Mathf.Abs(current % (Positions.Length))]);

                if (Positions[Mathf.Abs(current % (Positions.Length))].transform.position == transform.position) {
                    prev = false;
                }

            }
            if (next) {
                CamMoveTo(Positions[Mathf.Abs(current % (Positions.Length))]);
                //CamMoveTo(Positions[(Positions.Length - 1) - Mathf.Abs(current % (Positions.Length - 1))]);

                if (Positions[Mathf.Abs(current % (Positions.Length))].transform.position == transform.position) {
                    next = false;
                }
            }

        }

    }


    public struct CamPos {
        // Camera-Position
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;

        public CamPos(float x, float y, float z, float rotX, float rotY) {
            //position
            this.x = x;
            this.y = y;
            this.z = z;

            //rotation
            this.rotX = rotX;
            this.rotY = rotY;
        }

    }


    //moving Camera as Director
    //move cam in random direction (up, down,left, right)
    //maybe
    public void CameraMoveSC() {
        switch (//UnityEngine.Random.Range(2, 4))
            randomWays) {
            //UP
            case 0:
                if (transform.position.y - bordery < 1.5f) {
                    transform.Translate(new Vector3(0, (camSpeed / 2) * Time.deltaTime, 0));
                    transform.LookAt(fix);
                }
                break;

            //Down
            case 1:
                if (transform.position.y - bordery > -0.5f) {
                    transform.Translate(new Vector3(0, (-camSpeed / 2) * Time.deltaTime, 0));
                    transform.LookAt(fix);
                }
                break;

            //Left
            case 2:
                if (transform.position.x - borderx > -6) {
                    transform.Translate(new Vector3((-camSpeed / 2) * Time.deltaTime, 0, 0));
                    transform.LookAt(fix);
                }
                break;

            //Right
            case 3:
                if (transform.position.x - borderx < 6) {
                    transform.Translate(new Vector3((camSpeed / 2) * Time.deltaTime, 0, 0));
                    transform.LookAt(fix);
                }
                break;

            //UP-right
            case 4:
                if (transform.position.y - bordery < 1.5f) {
                    transform.Translate(new Vector3(0, (camSpeed / 4) * Time.deltaTime, 0));
                }

                if (transform.position.x - borderx < 6) {
                    transform.Translate(new Vector3((camSpeed / 4) * Time.deltaTime, 0, 0));
                }

                transform.LookAt(fix);
                break;

            //Down-right
            case 5:
                if (transform.position.y - bordery > -0.5f) {
                    transform.Translate(new Vector3(0, (-camSpeed / 4) * Time.deltaTime, 0));
                }

                if (transform.position.x - borderx < 6) {
                    transform.Translate(new Vector3((camSpeed / 4) * Time.deltaTime, 0, 0));
                }

                transform.LookAt(fix);
                break;

            //Donw-left
            case 6:
                if (transform.position.y - bordery > -0.5f) {
                    transform.Translate(new Vector3(0, (-camSpeed / 4) * Time.deltaTime, 0));
                }
                if (transform.position.x - borderx > -6) {
                    transform.Translate(new Vector3((-camSpeed / 4) * Time.deltaTime, 0, 0));
                }
                transform.LookAt(fix);
                break;

            //Up-left
            case 7:
                if (transform.position.y - bordery < 1.5f) {
                    transform.Translate(new Vector3(0, (camSpeed / 4) * Time.deltaTime, 0));
                }
                if (transform.position.x - borderx > -6) {
                    transform.Translate(new Vector3((-camSpeed / 4) * Time.deltaTime, 0, 0));
                }
                transform.LookAt(fix);
                break;

            default:
                break;
        }
        transform.LookAt(fix);
    }

    //move between set positions
    public void CamMoveTo(GameObject pos) {
        float step = camSpeed * Time.deltaTime;
        //Vector3 target = pos.transform.position;
        Vector3 targetDir = pos.transform.position - transform.position;


        //moving itself towards pos
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.transform.position.x, pos.transform.position.y, pos.transform.position.z), camSpeed * 2 * Time.deltaTime);


        //IMPLEMENT ROTATION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Vector3 rotat = Vector3.RotateTowards(transform.forward, targetDir, step, 1.0f);    //(pos.rotX, pos.rotY, 0);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(pos.rotX,pos.rotY,0,0), 0.1f);

        transform.LookAt(fix);
    }

    public void ChangeMode()
    {
        director = !director;
    }

    // just there for waiting
    IEnumerator sleeper() {
        waiting = true;
        //randomization for CamMoveSC
        randomWays = UnityEngine.Random.Range(0, 4);

        //short time for testing
        yield return new WaitForSeconds(UnityEngine.Random.Range(5, 7));
        print("waited");
        waiting = false;
        there = false;
    }

    IEnumerator WaitForIt() {
        Candle.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Candle.enabled = true;
        GetComponent<Camera>().cullingMask = mask;
    }
}