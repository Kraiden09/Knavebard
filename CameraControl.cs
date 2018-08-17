using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* simpelste Bewegung der Kamera innerhalb simpler Grenzen
 * bisher: oben, unten, links, rechts, zoom, zoomout
 * 
 */
public class CameraControl : MonoBehaviour {


    //Bewegungsgeschwindigkeit
    float camSpeed;
    float borderx, bordery, borderz;
    bool director;
    bool waiting;
    int random;
    int ranDirection;

    /*
    // used structs
    CamPos[] Positions;
    CamPos standard;
    CamPos front;
    CamPos left;
    CamPos right;
    */

    GameObject standard;
    GameObject front;
    GameObject left;

    GameObject[] Positions;

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
        standard.transform.Translate(0, 1, -8);
        front = new GameObject("RightPos");
        front.transform.Translate(0, 1.8f, -2.5f);


        Positions = new GameObject[]{ standard, front };
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
            //slowly moving in random direction
            //int ranDirection used (and possible rotation)




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
            
            
        
        }


        //MANUAL-MODE
        if (!director)
        {
            //w-a-s-d
            if (Input.GetKey(KeyCode.W))
            {
                //Grenze
                if (transform.position.y - bordery < 1)
                {
                    transform.Translate(new Vector3(0, camSpeed * Time.deltaTime, 0));
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (transform.position.x - borderx > -1)
                {
                    transform.Translate(new Vector3(-camSpeed * Time.deltaTime, 0, 0));
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (transform.position.y - bordery > -1)
                {
                    transform.Translate(new Vector3(0, -camSpeed * Time.deltaTime, 0));
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (transform.position.x - borderx < 1)
                {
                    transform.Translate(new Vector3(camSpeed * Time.deltaTime, 0, 0));
                }
            }

            //Zoom + Zoom out
            if (Input.GetKey(KeyCode.PageUp))
            {
                if (transform.position.z - borderz < 1)
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

    /*
    //moving Camera as Director
    public void CameraMoveAnchor(Vector3 pos)
    {
        transform.Translate(new Vector3(pos.x, pos.y, pos.z));
    }
    */

    public void CamMoveTo(GameObject pos)
    {
        float step = camSpeed * Time.deltaTime;
        //Vector3 target = pos.transform.position;
        Vector3 targetDir = pos.transform.position - transform.position;


        //moving itself towards pos
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(pos.transform.position.x, pos.transform.position.y,pos.transform.position.z), camSpeed * 2 * Time.deltaTime);


        Vector3 rotat = Vector3.RotateTowards(transform.forward, targetDir, step, 1.0f);    //(pos.rotX, pos.rotY, 0);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(pos.rotX,pos.rotY,0,0), 0.1f);


    }

    // just there for waiting
    IEnumerator sleeper()
    {
        waiting = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));
        print("waited");
        waiting = false;
    }

}
