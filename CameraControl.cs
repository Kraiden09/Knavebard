using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* simpelste Bewegung der Kamera innerhalb simpler Grenzen
 * bisher: oben, unten, links, rechts, zoom, zoomout
 * 
 */
public class CameraControl : MonoBehaviour {
    //Camera cam;


    //Bewegungsgeschwindigkeit
    public float camSpeed;

    float borderx, bordery, borderz;

	// Use this for initialization
	void Start () {
        //  cam = Camera.main;
        //indirekte Grenzen durch urspruengliche Kameraposition
        borderx = transform.position.x;
        bordery = transform.position.y;
        borderz = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {

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


    }
}
