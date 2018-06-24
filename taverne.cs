using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class taverne : MonoBehaviour {

    // Use this for initialization

    //Deklaration der Bereiche auf der Plane Boden
    GameObject boden;
    GameObject barbereich;
    GameObject tischbereich;
    GameObject buehnenbereich;

    //Deklaration der Objekte in den Bereichen
    GameObject buehne;
    GameObject tisch;
    GameObject bar;
    GameObject stuhl;
    GameObject fass;
    GameObject hocker1;
    GameObject hocker2;
    GameObject hocker3;
    GameObject hocker4;
    GameObject hocker5;
    GameObject hocker6;

    //Deklaration der Meshes und Hilfslisten für Bereiche
    Mesh barbermesh;
    List<Vector3> barberVert;
    List<int> barberTri;

    Mesh buebermesh;
    List<Vector3> bueberVert;
    List<int> bueberTri;

    Mesh tibermesh;
    List<Vector3> tiberVert;
    List<int> tiberTri;

    //Deklaration der Meshes und Hilfslisten für Objekte
    Mesh barmesh;
    List<Vector3> barVert;
    List<int> barTri;

    Mesh homesh;
    List<Vector3> hoVert;
    List<int> hoTri;

    Mesh bumesh;
    List<Vector3> buVert;
    List<int> buTri;

    //hockermethode
    void hockermesh(float xpos, float ypos, float zpos, GameObject hocker) {

        hoVert = new List<Vector3>();
        hoTri = new List<int>();

        hoVert.Add(new Vector3(xpos + 0.2f, ypos, zpos - 0.2f));
        hoVert.Add(new Vector3(xpos + 0.2f, ypos + 0.4f, zpos - 0.2f));
        hoVert.Add(new Vector3(xpos + 0.2f, ypos, zpos + 0.2f));
        hoVert.Add(new Vector3(xpos + 0.2f, ypos + 0.4f, zpos + 0.2f));

        hoVert.Add(new Vector3(xpos - 0.2f, ypos, zpos + 0.2f));
        hoVert.Add(new Vector3(xpos - 0.2f, ypos + 0.4f, zpos + 0.2f));

        hoVert.Add(new Vector3(xpos - 0.2f, ypos, zpos - 0.2f));
        hoVert.Add(new Vector3(xpos - 0.2f, ypos + 0.4f, zpos - 0.2f));

        hoVert.Add(new Vector3(xpos + 0.2f, ypos, zpos - 0.2f));
        hoVert.Add(new Vector3(xpos + 0.2f, ypos + 0.4f, zpos - 0.2f));

        hoVert.Add(new Vector3(xpos - 0.2f, ypos + 0.4f, zpos - 0.2f));
        hoVert.Add(new Vector3(xpos - 0.2f, ypos + 0.4f, zpos + 0.2f));
        hoVert.Add(new Vector3(xpos + 0.2f, ypos + 0.4f, zpos - 0.2f));
        hoVert.Add(new Vector3(xpos + 0.2f, ypos + 0.4f, zpos + 0.2f));

        homesh = new Mesh();
        homesh.vertices = hoVert.ToArray();

        hocker.GetComponent<MeshFilter>().mesh = homesh;

        for (int i = 0; i < homesh.vertices.Length - 2; i++)
        {
            if (i % 2 == 0)
            {
                hoTri.Add(i);
                hoTri.Add(i + 1);
                hoTri.Add(i + 2);
            }

            if (i % 2 == 1)
            {
                hoTri.Add(i + 1);
                hoTri.Add(i);
                hoTri.Add(i + 2);
            }
        }

        homesh.triangles = hoTri.ToArray();

        hocker.GetComponent<Renderer>().material.color = new Color(0.0f,1.0f,1.0f);
    }

void Start () {

        //Instanzierung der Bereiche auf der Plane Boden
        boden = GameObject.CreatePrimitive(PrimitiveType.Plane);
        barbereich = GameObject.CreatePrimitive(PrimitiveType.Quad);
        buehnenbereich = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tischbereich = GameObject.CreatePrimitive(PrimitiveType.Quad);

        //Instanzierung der Objekte in den Bereichen
        buehne = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bar = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker3 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker4 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker5 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker6 = GameObject.CreatePrimitive(PrimitiveType.Quad);

        //Instanzierung der Hilfslisten der Bereiche
        barberVert = new List<Vector3>();
        barberTri = new List<int>();
        bueberVert = new List<Vector3>();
        bueberTri = new List<int>();
        tiberVert = new List<Vector3>();
        tiberTri = new List<int>();

        //Instanzierung der Hilfslisten der Objekte
        barVert = new List<Vector3>();
        barTri = new List<int>();

        buVert = new List<Vector3>();
        buTri = new List<int>();

        //Bereiche der Taverne einteilen

        //Barbereich
        barberVert.Add(new Vector3(-5, 0.1f, -5f));
        barberVert.Add(new Vector3(-5, 0.1f, 0));
        barberVert.Add(new Vector3(-2, 0.1f, -5f));
        barberVert.Add(new Vector3(-2, 0.1f, 0));

        barbermesh = new Mesh();
        barbermesh.vertices = barberVert.ToArray();
        barbereich.GetComponent<MeshFilter>().mesh = barbermesh;

        barberTri.Add(0);
        barberTri.Add(1);
        barberTri.Add(2);

        barberTri.Add(2);
        barberTri.Add(1);
        barberTri.Add(3);

        barbermesh.triangles = barberTri.ToArray();

        //Buehnenbereich
        bueberVert.Add(new Vector3(-3, 0.1f, 2));
        bueberVert.Add(new Vector3(-3, 0.1f, 5));
        bueberVert.Add(new Vector3(+3, 0.1f, 2));
        bueberVert.Add(new Vector3(+3, 0.1f, 5));

        buebermesh = new Mesh();
        buebermesh.vertices = bueberVert.ToArray();

        buehnenbereich.GetComponent<MeshFilter>().mesh = buebermesh;

        bueberTri.Add(0);
        bueberTri.Add(1);
        bueberTri.Add(2);

        bueberTri.Add(2);
        bueberTri.Add(1);
        bueberTri.Add(3);

        buebermesh.triangles = bueberTri.ToArray();

        //Tischbereich
        tiberVert.Add(new Vector3( -1, 0.1f, -4.5f));
        tiberVert.Add(new Vector3( -1, 0.1f, 1));
        tiberVert.Add(new Vector3( 4, 0.1f, -4.5f));
        tiberVert.Add(new Vector3( 4, 0.1f, 1));

        tibermesh = new Mesh();
        tibermesh.vertices = tiberVert.ToArray();

        tischbereich.GetComponent<MeshFilter>().mesh = tibermesh;

        tiberTri.Add(0);
        tiberTri.Add(1);
        tiberTri.Add(2);

        tiberTri.Add(2);
        tiberTri.Add(1);
        tiberTri.Add(3);

        tibermesh.triangles = tiberTri.ToArray();

        //Farben der einzelnen Bereiche
        barbereich.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
        buehnenbereich.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
        tischbereich.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f);


        //Objekte verschieben/skalieren/rotieren

        //buehne


        buVert.Add(new Vector3( -2.5f, 0.1f, 2.5f));
        buVert.Add(new Vector3( -2.5f, 0.6f, 2.5f));
        buVert.Add(new Vector3( 2.5f, 0.1f, 2.5f));
        buVert.Add(new Vector3( 2.5f, 0.6f, 2.5f));

        buVert.Add(new Vector3( 2.5f, 0.1f, 5));
        buVert.Add(new Vector3( 2.5f, 0.6f, 5));

        buVert.Add(new Vector3(-2.5f, 0.1f, 5));
        buVert.Add(new Vector3(-2.5f, 0.6f, 5));

        buVert.Add(new Vector3(-2.5f, 0.1f, 2.5f));
        buVert.Add(new Vector3(-2.5f, 0.6f, 2.5f));

        buVert.Add(new Vector3(-2.5f, 0.6f, 2.5f));
        buVert.Add(new Vector3(-2.5f, 0.6f, 5));
        buVert.Add(new Vector3( 2.5f, 0.6f, 2.5f));
        buVert.Add(new Vector3( 2.5f, 0.6f, 5));

        bumesh = new Mesh();
        bumesh.vertices = buVert.ToArray();

        buehne.GetComponent<MeshFilter>().mesh = bumesh;

        int cnt = 0;

        for (int i = 0; i < bumesh.vertices.Length - 2; i++)
        {
            if (i % 2 == 0)
            {
                buTri.Add(i);
                buTri.Add(i + 1);
                buTri.Add(i + 2);
            }

            if (i % 2 == 1)
            {
                buTri.Add(i + 1);
                buTri.Add(i);
                buTri.Add(i + 2);
            }

            if (i > bumesh.vertices.Length - 9)
            {

                cnt++;

                if (cnt % 2 == 0)
                {
                    i = i + 2;
                }

            }
        }

        bumesh.triangles = buTri.ToArray();

        //Barbereichobjekte

        //Bar
        barVert.Add(new Vector3( -4, 0.1f, -5));
        barVert.Add(new Vector3( -4, 0.6f, -5));
        barVert.Add(new Vector3( -3, 0.1f, -5));
        barVert.Add(new Vector3( -3, 0.6f, -5));

        barVert.Add(new Vector3(-3, 0.1f, 0));
        barVert.Add(new Vector3(-3, 0.6f, 0));

        barVert.Add(new Vector3(-5, 0.1f, 0));
        barVert.Add(new Vector3(-5, 0.6f, 0));

        barVert.Add(new Vector3(-5, 0.1f, -1));
        barVert.Add(new Vector3(-5, 0.6f, -1));

        barVert.Add(new Vector3(-4, 0.1f, -1));
        barVert.Add(new Vector3(-4, 0.6f, -1));

        barVert.Add(new Vector3(-4, 0.1f, -5));
        barVert.Add(new Vector3(-4, 0.6f, -5));

        barVert.Add(new Vector3(-4, 0.6f, -5));
        barVert.Add(new Vector3(-4, 0.6f, -1));
        barVert.Add(new Vector3(-3, 0.6f, -5));
        barVert.Add(new Vector3(-3, 0.6f, -1));
        barVert.Add(new Vector3(-5, 0.6f, -1));
        barVert.Add(new Vector3(-5, 0.6f, 0));
        barVert.Add(new Vector3(-3, 0.6f, -1));
        barVert.Add(new Vector3(-3, 0.6f, 0));

        barmesh = new Mesh();
        barmesh.vertices = barVert.ToArray();

        bar.GetComponent<MeshFilter>().mesh = barmesh;

        cnt = 0;

        for (int i = 0; i < barmesh.vertices.Length - 2; i++)
        {
            if (i % 2 == 0)
            {
                barTri.Add(i);
                barTri.Add(i + 1);
                barTri.Add(i + 2);
            }

            if (i % 2 == 1)
            {
                barTri.Add(i + 1);
                barTri.Add(i);
                barTri.Add(i + 2);
            }

            if (i > barmesh.vertices.Length - 13) {

                cnt++;

                if (cnt % 2 == 0) {
                    i = i + 2;
                }
           
            }
        }

        barmesh.triangles = barTri.ToArray();

        //Hocker
        hockermesh(-2.4f, 0.1f, -4.5f, hocker1);
        hockermesh(-2.4f, 0.1f, -3.7f, hocker2);
        hockermesh(-2.4f, 0.1f, -2.9f, hocker3);
        hockermesh(-2.4f, 0.1f, -2.1f, hocker4);
        hockermesh(-2.4f, 0.1f, -1.3f, hocker5);
        hockermesh(-2.4f, 0.1f, -0.5f, hocker6);

        //Farben der Objekte
        buehne.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
        bar.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
        boden.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);

        Debug.Log(bumesh.triangles.Length);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
