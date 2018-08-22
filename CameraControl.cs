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

    //Bewegungsgeschwindigkeit
    float camSpeed;
    float borderx, bordery, borderz;
    bool director;
    bool waiting;
    int random;
    int ranDirection;

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



	// Use this for initialization
	void Start () {
        //indirekte Grenzen durch urspruengliche Kameraposition
        borderx = transform.position.x;
        bordery = transform.position.y;
        borderz = transform.position.z;

        camSpeed = 2;

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
        leftrear.transform.Translate(-4.5f, 4.5f, -8.3f);
        rightrear = new GameObject("RightRearPos");
        rightrear.transform.Translate(4.5f, 4.5f, -8.3f);


        Positions = new GameObject[]{ standard, front, leftfront, rightfront,rightrear,leftrear };

        /*
        //References (not necessary)
        noteBoard = GameObject.Find("NoteBoard").GetComponent<NoteBoard>();
        screen = noteBoard.getScreen();
        */
        
        //standard fixpoint for bard-mode (maybe later changed for free-roam)
        fix = new Vector3(0, 2.7f, 6.15f);
        CamMoveTo(standard);
    }

    // Update is called once per frame
    void Update () {
        //switch
        if (Input.GetKeyDown("1"))
        {
            director = !director;
        }




        //AUTO-MODE
        if (director)
        {

            //moving camera from pos to pos
            //waiting in between changes
            if (!waiting)
            {
                StartCoroutine(sleeper());
                random = UnityEngine.Random.Range(0, Positions.Length);
            }
            
            //moving
            if (waiting)
            {
                CamMoveTo(Positions[random]);
            }
            if (Positions[random].transform.position == transform.position)
            {
                //slowly moving in random direction
                //int ranDirection used (and possible rotation)
                CameraMoveSC();
            }
            
        }


        //MANUAL-MODE
        if (!director)
        {
            //w-a-s-d
            if (Input.GetKey(KeyCode.W))
            {
                //Grenze
                if (transform.position.y - bordery < 2)
                {
                    transform.Translate(new Vector3(0, camSpeed * Time.deltaTime, 0));
                    transform.LookAt(fix);
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (transform.position.x - borderx > -4)
                {
                    transform.Translate(new Vector3(-camSpeed * Time.deltaTime, 0, 0));
                    transform.LookAt(fix);
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (transform.position.y - bordery > -0.5)
                {
                    transform.Translate(new Vector3(0, -camSpeed * Time.deltaTime, 0));
                    transform.LookAt(fix);
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (transform.position.x - borderx < 4)
                {
                    transform.Translate(new Vector3(camSpeed * Time.deltaTime, 0, 0));
                    transform.LookAt(fix);
                }
            }

            //Zoom + Zoom out
            if (Input.GetKey(KeyCode.PageUp))
            {
                if (transform.position.z - borderz < 5)
                {
                    transform.Translate(new Vector3(0, 0, camSpeed * Time.deltaTime));
                }
            }

            if (Input.GetKey(KeyCode.PageDown))
            {
                if (transform.position.z - borderz > -1)
                {
                    transform.Translate(new Vector3(0, 0, -camSpeed * Time.deltaTime));
                }
            }

            
            //manual change of Camera-Position
            if (Input.GetKeyDown("q"))
            {
                
            }

            if (Input.GetKeyDown("e"))
            {

            }
        }
    }

    
    public struct CamPos
    {
        // Camera-Position
        public float x;
        public float y;
        public float z;
        public float rotX;
        public float rotY;

        public CamPos(float x, float y, float z, float rotX, float rotY)
        {
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
    public void CameraMoveSC()
    {
        switch (UnityEngine.Random.Range(2, 4))
        {
            /*
            //UP
            case 0:
                break;

            //Down
            case 1:
                break;
                */
            //Left
            case 2:
                break;

            //Right
            case 3:
                break;

            default:
                break;
        }
        transform.LookAt(fix);
    }

    //move between set positions
    public void CamMoveTo(GameObject pos)
    {
        float step = camSpeed * Time.deltaTime;
        //Vector3 target = pos.transform.position;
        Vector3 targetDir = pos.transform.position - transform.position;


        //moving itself towards pos
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(pos.transform.position.x, pos.transform.position.y,pos.transform.position.z), camSpeed * 2 * Time.deltaTime);


        //IMPLEMENT ROTATION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //Vector3 rotat = Vector3.RotateTowards(transform.forward, targetDir, step, 1.0f);    //(pos.rotX, pos.rotY, 0);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(pos.rotX,pos.rotY,0,0), 0.1f);

        transform.LookAt(fix);
    }


    // just there for waiting
    IEnumerator sleeper()
    {
        waiting = true;

        //short time for testing
        yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));
        print("waited");
        waiting = false;
    }

}
