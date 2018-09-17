using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class taverne : Subject {

    // Use this for initialization
    int cnt = 0;
    float konfig;
    float lastkonf = 0;
    //Hilfsvariablen für Randomize
    int hockerzahl = 0;
    int tischzahl = 0;
    int maxtisch = 4;
    int maxhocker = 28;
    string hock = "Hocker";
    string tis = "Tisch";
    string nr;
    string tinr;
    GameObject h;
    //Deklaration der Bereiche auf der Plane Boden
    GameObject boden;
    /*GameObject barbereich;
    GameObject tischbereich;
    GameObject buehnenbereich;
    GameObject kamerabereich;*/

    //Deklaration der Objekte in den Bereichen
    GameObject buehne;
    GameObject tisch1;
    GameObject tisch2;
    GameObject tisch3;
    GameObject tisch4;
    GameObject bar;
    GameObject stuhl;
    GameObject fass;
    GameObject hocker1;
    GameObject hocker2;
    GameObject hocker3;
    GameObject hocker4;
    GameObject hocker5;
    GameObject hocker6;
    GameObject hocker7;
    GameObject hocker8;
    GameObject hocker9;
    GameObject hocker10;
    GameObject hocker11;
    GameObject hocker12;
    GameObject hocker13;
    GameObject hocker14;
    GameObject hocker15;
    GameObject hocker16;
    GameObject hocker17;
    GameObject hocker18;
    GameObject hocker19;
    GameObject hocker20;
    GameObject hocker21;
    GameObject hocker22;
    GameObject hocker23;
    GameObject hocker24;
    GameObject hocker25;
    GameObject hocker26;
    GameObject hocker27;
    GameObject hocker28;
    GameObject treppe;
    GameObject wand1;
    GameObject wand2;
    GameObject wand3;
    GameObject wand4;
    GameObject decke;
    GameObject spawn;

    //Deklaration der Meshes und Hilfslisten für Bereiche
    Mesh barbermesh;
    List<Vector3> barberVert;
    List<int> barberTri;

    Mesh bermesh;
    List<Vector3> berVert;
    List<int> berTri;

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

    Mesh timesh;
    List<Vector3> tiVert;
    List<int> tiTri;

    Mesh tremesh;
    List<Vector3> treVert;
    List<int> treTri;

    //waagerechte tischmethode
    void tischmeshwaage(float xpos, float ypos, float zpos, GameObject tisch, GameObject hocker1, GameObject hocker2, GameObject hocker3, GameObject hocker4) {

        tisch.transform.position = new Vector3(xpos, ypos, zpos);
        tiVert = new List<Vector3>();
        tiTri = new List<int>();

        tiVert.Add(new Vector3(+0.8f, 0, -0.4f));
        tiVert.Add(new Vector3(+0.8f, +0.6f, -0.4f));
        tiVert.Add(new Vector3(+0.8f, 0, +0.4f));
        tiVert.Add(new Vector3(+0.8f, +0.6f, +0.4f));

        tiVert.Add(new Vector3(+0.8f, 0, +0.4f));
        tiVert.Add(new Vector3(+0.8f, +0.6f, +0.4f));
        tiVert.Add(new Vector3(-0.8f, 0, +0.4f));
        tiVert.Add(new Vector3(-0.8f, +0.6f, +0.4f));

        tiVert.Add(new Vector3(-0.8f, 0, +0.4f));
        tiVert.Add(new Vector3(-0.8f, +0.6f, +0.4f));
        tiVert.Add(new Vector3(-0.8f, 0, -0.4f));
        tiVert.Add(new Vector3(-0.8f, +0.6f, -0.4f));

        tiVert.Add(new Vector3(-0.8f, 0, -0.4f));
        tiVert.Add(new Vector3(-0.8f, +0.6f, -0.4f));
        tiVert.Add(new Vector3(+0.8f, 0, -0.4f));
        tiVert.Add(new Vector3(+0.8f, +0.6f, -0.4f));

        tiVert.Add(new Vector3(-0.8f, +0.6f, -0.4f));
        tiVert.Add(new Vector3(-0.8f, +0.6f, +0.4f));
        tiVert.Add(new Vector3(+0.8f, +0.6f, -0.4f));
        tiVert.Add(new Vector3(+0.8f, +0.6f, +0.4f));



        timesh = new Mesh();
        timesh.vertices = tiVert.ToArray();

        tisch.GetComponent<MeshFilter>().mesh = timesh;

        int durchlauf = 0;

        for (int i = 0; i < timesh.vertices.Length - 2; i++) {

            if (durchlauf % 2 == 0) {
                tiTri.Add(i);
                tiTri.Add(i + 1);
                tiTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                tiTri.Add(i + 1);
                tiTri.Add(i);
                tiTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }


            durchlauf++;
        }

        timesh.triangles = tiTri.ToArray();

        tisch.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);

        //Hocker werden an Tisch angesetzt...Austausch mit Stuehlen?
        hockermesh(xpos - 0.4f, ypos, zpos + 0.8f, hocker1);
        hockermesh(xpos + 0.4f, ypos, zpos + 0.8f, hocker2);
        hockermesh(xpos - 0.4f, ypos, zpos - 0.8f, hocker3);
        hocker3.transform.Rotate(0, 180, 0);
        hockermesh(xpos + 0.4f, ypos, zpos - 0.8f, hocker4);
        hocker4.transform.Rotate(0, 180, 0);

        hockerzahl = hockerzahl + 4;
        tischzahl = tischzahl + 1;

        createUVHocker(tisch);
    }

    //senkrechte Tischmethode
    void tischmeshsenk(float xpos, float ypos, float zpos, GameObject tisch, GameObject hocker1, GameObject hocker2, GameObject hocker3, GameObject hocker4) {
        tisch.transform.position = new Vector3(xpos, ypos, zpos);
        tiVert = new List<Vector3>();
        tiTri = new List<int>();



        tiVert.Add(new Vector3(+0.4f, 0, -0.8f));
        tiVert.Add(new Vector3(+0.4f, +0.6f, -0.8f));
        tiVert.Add(new Vector3(+0.4f, 0, +0.8f));
        tiVert.Add(new Vector3(+0.4f, +0.6f, +0.8f));

        tiVert.Add(new Vector3(+0.4f, 0, +0.8f));
        tiVert.Add(new Vector3(+0.4f, +0.6f, +0.8f));
        tiVert.Add(new Vector3(-0.4f, 0, +0.8f));
        tiVert.Add(new Vector3(-0.4f, +0.6f, +0.8f));

        tiVert.Add(new Vector3(-0.4f, 0, +0.8f));
        tiVert.Add(new Vector3(-0.4f, +0.6f, +0.8f));
        tiVert.Add(new Vector3(-0.4f, 0, -0.8f));
        tiVert.Add(new Vector3(-0.4f, +0.6f, -0.8f));

        tiVert.Add(new Vector3(-0.4f, 0, -0.8f));
        tiVert.Add(new Vector3(-0.4f, +0.6f, -0.8f));
        tiVert.Add(new Vector3(+0.4f, 0, -0.8f));
        tiVert.Add(new Vector3(+0.4f, +0.6f, -0.8f));

        tiVert.Add(new Vector3(-0.4f, +0.6f, -0.8f));
        tiVert.Add(new Vector3(-0.4f, +0.6f, +0.8f));
        tiVert.Add(new Vector3(+0.4f, +0.6f, -0.8f));
        tiVert.Add(new Vector3(+0.4f, +0.6f, +0.8f));


        timesh = new Mesh();
        timesh.vertices = tiVert.ToArray();

        tisch.GetComponent<MeshFilter>().mesh = timesh;


        int durchlauf = 0;

        for (int i = 0; i < timesh.vertices.Length - 2; i++) {

            if (durchlauf % 2 == 0) {
                tiTri.Add(i);
                tiTri.Add(i + 1);
                tiTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                tiTri.Add(i + 1);
                tiTri.Add(i);
                tiTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }


            durchlauf++;
        }

        timesh.triangles = tiTri.ToArray();

        tisch.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);

        //Hocker werden an Tisch angesetzt...Austausch mit Stuehlen?
        hockermesh(xpos - 0.8f, ypos, zpos + 0.4f, hocker1);
        hocker1.transform.Rotate(0, 90, 0);
        hockermesh(xpos + 0.8f, ypos, zpos + 0.4f, hocker2);
        hocker2.transform.Rotate(0, -90, 0);
        hockermesh(xpos - 0.8f, ypos, zpos - 0.4f, hocker3);
        hocker3.transform.Rotate(0, 90, 0);
        hockermesh(xpos + 0.8f, ypos, zpos - 0.4f, hocker4);
        hocker4.transform.Rotate(0, -90, 0);

        hockerzahl = hockerzahl + 4;
        tischzahl = tischzahl + 1;

        createUVHocker(tisch);
    }

    void hockermesh(float xpos, float ypos, float zpos, GameObject hocker) {
        hocker.transform.position = new Vector3(xpos, ypos, zpos);
        hoVert = new List<Vector3>();
        hoTri = new List<int>();



        hoVert.Add(new Vector3(+0.2f, 0, -0.2f));
        hoVert.Add(new Vector3(+0.2f, 0.4f, -0.2f));
        hoVert.Add(new Vector3(+0.2f, 0, +0.2f));
        hoVert.Add(new Vector3(+0.2f, +0.4f, +0.2f));

        hoVert.Add(new Vector3(+0.2f, 0, +0.2f));
        hoVert.Add(new Vector3(+0.2f, +0.4f, +0.2f));
        hoVert.Add(new Vector3(-0.2f, 0, +0.2f));
        hoVert.Add(new Vector3(-0.2f, +0.4f, +0.2f));

        hoVert.Add(new Vector3(-0.2f, 0, +0.2f));
        hoVert.Add(new Vector3(-0.2f, +0.4f, +0.2f));
        hoVert.Add(new Vector3(-0.2f, 0, -0.2f));
        hoVert.Add(new Vector3(-0.2f, +0.4f, -0.2f));

        hoVert.Add(new Vector3(-0.2f, 0, -0.2f));
        hoVert.Add(new Vector3(-0.2f, +0.4f, -0.2f));
        hoVert.Add(new Vector3(+0.2f, 0, -0.2f));
        hoVert.Add(new Vector3(+0.2f, +0.4f, -0.2f));

        hoVert.Add(new Vector3(-0.2f, +0.4f, -0.2f));
        hoVert.Add(new Vector3(-0.2f, +0.4f, +0.2f));
        hoVert.Add(new Vector3(+0.2f, +0.4f, -0.2f));
        hoVert.Add(new Vector3(+0.2f, +0.4f, +0.2f));


        homesh = new Mesh();
        homesh.vertices = hoVert.ToArray();

        hocker.GetComponent<MeshFilter>().mesh = homesh;

        int durchlauf = 0;

        for (int i = 0; i < homesh.vertices.Length - 2; i = i + 1) {

            if (durchlauf % 2 == 0) {
                hoTri.Add(i);
                hoTri.Add(i + 1);
                hoTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                hoTri.Add(i + 1);
                hoTri.Add(i);
                hoTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }

            durchlauf++;
        }

        homesh.triangles = hoTri.ToArray();

        hocker.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 1.0f);
        createUVHocker(hocker);
    }

    //Treppe an der Buehne
    void treppenmesh(float xpos, float ypos, float zpos, GameObject treppe) {
        treppe.transform.position = new Vector3(xpos, ypos, zpos);
        treVert = new List<Vector3>();
        treTri = new List<int>();



        treVert.Add(new Vector3(0, 0f, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(0, 0f, +0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4), +0.5f));

        treVert.Add(new Vector3(0, 0f, +0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(-0.6f, 0f, +0.5f));
        treVert.Add(new Vector3(-0.6f, 0f + (0.5f / 4), +0.5f));

        treVert.Add(new Vector3(-0.6f, 0f, +0.5f));
        treVert.Add(new Vector3(-0.6f, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(-0.6f, 0f, -0.5f));
        treVert.Add(new Vector3(-0.6f, 0f + (0.5f / 4), -0.5f));

        treVert.Add(new Vector3(-0.6f, 0f, -0.5f));
        treVert.Add(new Vector3(-0.6f, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(0, 0f, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4), -0.5f));

        treVert.Add(new Vector3(-0.6f, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(-0.6f, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4), +0.5f));



        treVert.Add(new Vector3(0, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(0, 0f + ((0.5f / 4) * 2), +0.5f));

        treVert.Add(new Vector3(0, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(0, 0f + ((0.5f / 4) * 2), +0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + ((0.5f / 4) * 2), +0.5f));

        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4), +0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + ((0.5f / 4) * 2), +0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + ((0.5f / 4) * 2), -0.5f));

        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4), -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 2, -0.5f));

        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(-0.4f, 0f + (0.5f / 4) * 2, +0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 2, +0.5f));



        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 3, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 2, +0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 3, +0.5f));

        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 2, +0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 3, +0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 2, +0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 3, +0.5f));

        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 2, +0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 3, +0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 3, -0.5f));

        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 3, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 2, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 3, -0.5f));

        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 3, -0.5f));
        treVert.Add(new Vector3(-0.2f, 0f + (0.5f / 4) * 3, +0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 3, -0.5f));
        treVert.Add(new Vector3(0, 0f + (0.5f / 4) * 3, +0.5f));

        tremesh = new Mesh();
        tremesh.vertices = treVert.ToArray();

        treppe.GetComponent<MeshFilter>().mesh = tremesh;

        int durchlauf = 0;
        for (int i = 0; i < tremesh.vertices.Length - 2; i++) {

            if (durchlauf % 2 == 0) {
                treTri.Add(i);
                treTri.Add(i + 1);
                treTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                treTri.Add(i + 1);
                treTri.Add(i);
                treTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }


            durchlauf++;
        }

        tremesh.triangles = treTri.ToArray();

        treppe.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 1.0f);

        createUVHocker(treppe);
    }

    //Bereiche werden generiert
    void berBuilder(GameObject bereich, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 vert4) {

        berVert = new List<Vector3>();
        berTri = new List<int>();

        berVert.Add(vert1);
        berVert.Add(vert2);
        berVert.Add(vert3);
        berVert.Add(vert4);

        bermesh = new Mesh();
        bermesh.vertices = berVert.ToArray();
        bereich.GetComponent<MeshFilter>().mesh = bermesh;

        berTri.Add(0);
        berTri.Add(1);
        berTri.Add(2);

        berTri.Add(2);
        berTri.Add(1);
        berTri.Add(3);

        bermesh.triangles = berTri.ToArray();
    }

    //Bargenerierung, Startpunkt am Ansatzpunkt, der nah an der Kamera ist
    void barBuilder(GameObject bar, float xpos, float ypos, float zpos) {
        bar.transform.position = new Vector3(xpos, ypos, zpos);
        barVert = new List<Vector3>();
        barTri = new List<int>();
        //xpos = -7; ypos = 0.1f, zpos = -5
        barVert.Add(new Vector3(0, 0, 0));
        barVert.Add(new Vector3(0, +0.5f, 0));
        barVert.Add(new Vector3(+2, 0, 0));
        barVert.Add(new Vector3(+2, +0.5f, 0));

        barVert.Add(new Vector3(+2, 0, 0));
        barVert.Add(new Vector3(+2, +0.5f, 0));
        barVert.Add(new Vector3(+2, 0, +5));
        barVert.Add(new Vector3(+2, +0.5f, +5));

        barVert.Add(new Vector3(+2, 0, +5));
        barVert.Add(new Vector3(+2, +0.5f, +5));
        barVert.Add(new Vector3(0, 0, +5));
        barVert.Add(new Vector3(0, +0.5f, +5));

        barVert.Add(new Vector3(0, 0, +5));
        barVert.Add(new Vector3(0, +0.5f, +5));
        barVert.Add(new Vector3(0, 0, +4));
        barVert.Add(new Vector3(0, +0.5f, +4));

        barVert.Add(new Vector3(0, 0, +4));
        barVert.Add(new Vector3(0, +0.5f, +4));
        barVert.Add(new Vector3(+1, 0, +4));
        barVert.Add(new Vector3(+1, +0.5f, +4));

        barVert.Add(new Vector3(+1, 0, +4));
        barVert.Add(new Vector3(+1, +0.5f, +4));
        barVert.Add(new Vector3(+1, 0, +1));
        barVert.Add(new Vector3(+1, +0.5f, +1));

        barVert.Add(new Vector3(+1, 0, +1));
        barVert.Add(new Vector3(+1, +0.5f, +1));
        barVert.Add(new Vector3(0, 0, +1));
        barVert.Add(new Vector3(0, +0.5f, +1));

        barVert.Add(new Vector3(0, 0, +1));
        barVert.Add(new Vector3(0, +0.5f, +1));
        barVert.Add(new Vector3(0, 0, 0));
        barVert.Add(new Vector3(0, +0.5f, 0));

        barVert.Add(new Vector3(0, +0.5f, 0));
        barVert.Add(new Vector3(0, +0.5f, +1));
        barVert.Add(new Vector3(+2, +0.5f, 0));
        barVert.Add(new Vector3(+2, +0.5f, +1));

        barVert.Add(new Vector3(+2, +0.5f, +1));
        barVert.Add(new Vector3(+1, +0.5f, +1));
        barVert.Add(new Vector3(+2, +0.5f, +5));
        barVert.Add(new Vector3(+1, +0.5f, +5));

        barVert.Add(new Vector3(+1, +0.5f, +5));
        barVert.Add(new Vector3(+1, +0.5f, +4));
        barVert.Add(new Vector3(0, +0.5f, +5));
        barVert.Add(new Vector3(0, +0.5f, +4));

        barmesh = new Mesh();
        barmesh.vertices = barVert.ToArray();
        bar.GetComponent<MeshFilter>().mesh = barmesh;



        int durchlauf = 0;
        for (int i = 0; i < barmesh.vertices.Length - 2; i++) {

            if (durchlauf % 2 == 0) {
                barTri.Add(i);
                barTri.Add(i + 1);
                barTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                barTri.Add(i + 1);
                barTri.Add(i);
                barTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }


            durchlauf++;
        }

        barmesh.triangles = barTri.ToArray();

        createUVBar(bar);

        hockermesh(xpos + 2.6f, ypos, zpos + 0.5f, hocker1);
        hocker1.transform.Rotate(0, -90, 0);
        hockermesh(xpos + 2.6f, ypos, zpos + 1.3f, hocker2);
        hocker2.transform.Rotate(0, -90, 0);
        hockermesh(xpos + 2.6f, ypos, zpos + 2.1f, hocker3);
        hocker3.transform.Rotate(0, -90, 0);
        hockermesh(xpos + 2.6f, ypos, zpos + 2.9f, hocker4);
        hocker4.transform.Rotate(0, -90, 0);
        hockermesh(xpos + 2.6f, ypos, zpos + 3.7f, hocker5);
        hocker5.transform.Rotate(0, -90, 0);
        hockermesh(xpos + 2.6f, ypos, zpos + 4.5f, hocker6);
        hocker6.transform.Rotate(0, -90, 0);

        hockerzahl = hockerzahl + 6;
    }

    // bar auf der rechten seite, Ansatzpunkt am wietesten weg von der Kamera
    void barBuilder2(GameObject bar, float xpos, float ypos, float zpos) {
        bar.transform.position = new Vector3(xpos, ypos, zpos);
        barVert = new List<Vector3>();
        barTri = new List<int>();
        //xpos = 7; ypos = 0.1f, zpos = 0   -5


        barVert.Add(new Vector3(0, 0, 0));
        barVert.Add(new Vector3(0, +0.5f, 0));
        barVert.Add(new Vector3(-2, 0, 0));
        barVert.Add(new Vector3(-2, +0.5f, 0));

        barVert.Add(new Vector3(-2, 0, 0));
        barVert.Add(new Vector3(-2, +0.5f, 0));
        barVert.Add(new Vector3(-2, 0, -5));
        barVert.Add(new Vector3(-2, +0.5f, -5));

        barVert.Add(new Vector3(-2, 0, -5));
        barVert.Add(new Vector3(-2, +0.5f, -5));
        barVert.Add(new Vector3(0, 0, -5));
        barVert.Add(new Vector3(0, +0.5f, -5));

        barVert.Add(new Vector3(0, 0, -5));
        barVert.Add(new Vector3(0, +0.5f, -5));
        barVert.Add(new Vector3(0, 0, -4));
        barVert.Add(new Vector3(0, +0.5f, -4));

        barVert.Add(new Vector3(0, 0, -4));
        barVert.Add(new Vector3(0, +0.5f, -4));
        barVert.Add(new Vector3(-1, 0, -4));
        barVert.Add(new Vector3(-1, +0.5f, -4));

        barVert.Add(new Vector3(-1, 0, -4));
        barVert.Add(new Vector3(-1, +0.5f, -4));
        barVert.Add(new Vector3(-1, 0, -1));
        barVert.Add(new Vector3(-1, +0.5f, -1));

        barVert.Add(new Vector3(-1, 0, -1));
        barVert.Add(new Vector3(-1, +0.5f, -1));
        barVert.Add(new Vector3(0, 0, -1));
        barVert.Add(new Vector3(0, +0.5f, -1));

        barVert.Add(new Vector3(0, 0, -1));
        barVert.Add(new Vector3(0, +0.5f, -1));
        barVert.Add(new Vector3(0, 0, 0));
        barVert.Add(new Vector3(0, +0.5f, 0));

        barVert.Add(new Vector3(0, +0.5f, 0));
        barVert.Add(new Vector3(0, +0.5f, -1));
        barVert.Add(new Vector3(-2, +0.5f, 0));
        barVert.Add(new Vector3(-2, +0.5f, -1));

        barVert.Add(new Vector3(-2, +0.5f, -1));
        barVert.Add(new Vector3(-1, +0.5f, -1));
        barVert.Add(new Vector3(-2, +0.5f, -5));
        barVert.Add(new Vector3(-1, +0.5f, -5));

        barVert.Add(new Vector3(-1, +0.5f, -5));
        barVert.Add(new Vector3(-1, +0.5f, -4));
        barVert.Add(new Vector3(0, +0.5f, -5));
        barVert.Add(new Vector3(0, +0.5f, -4));

        barmesh = new Mesh();
        barmesh.vertices = barVert.ToArray();
        bar.GetComponent<MeshFilter>().mesh = barmesh;


        int durchlauf = 0;
        for (int i = 0; i < barmesh.vertices.Length - 2; i++) {

            if (durchlauf % 2 == 0) {
                barTri.Add(i);
                barTri.Add(i + 1);
                barTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                barTri.Add(i + 1);
                barTri.Add(i);
                barTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }


            durchlauf++;
        }

        barmesh.triangles = barTri.ToArray();

        createUVBar(bar);

        hockermesh(xpos - 2.6f, ypos, zpos - 4.5f, hocker1);
        hocker1.transform.Rotate(0, 90, 0);
        hockermesh(xpos - 2.6f, ypos, zpos - 3.7f, hocker2);
        hocker2.transform.Rotate(0, 90, 0);
        hockermesh(xpos - 2.6f, ypos, zpos - 2.9f, hocker3);
        hocker3.transform.Rotate(0, 90, 0);
        hockermesh(xpos - 2.6f, ypos, zpos - 2.1f, hocker4);
        hocker4.transform.Rotate(0, 90, 0);
        hockermesh(xpos - 2.6f, ypos, zpos - 1.3f, hocker5);
        hocker5.transform.Rotate(0, 90, 0);
        hockermesh(xpos - 2.6f, ypos, zpos - 0.5f, hocker6);
        hocker6.transform.Rotate(0, 90, 0);

        hockerzahl = hockerzahl + 6;
    }

    //Zufällige Generierung
    void randomize() {
        if (hockerzahl != 0) {
            UpdateObserver();
            GameObject[] hocks = getHocker();
            for (int i = 1; i <= hockerzahl; i++) {
                hock = hock + i;
               
                if (GameObject.Find(hock).transform.eulerAngles.y == 90) {
                    GameObject.Find(hock).transform.Rotate(0, -90, 0);
                }
                if (GameObject.Find(hock).transform.eulerAngles.y == 270)
                {
                    GameObject.Find(hock).transform.Rotate(0, 90, 0);
                }
                if (GameObject.Find(hock).transform.eulerAngles.y == 180)
                {
                    GameObject.Find(hock).transform.Rotate(0, -180, 0);
                }

                hock = "Hocker";
            }
        }

        hockerzahl = 0;
        tischzahl = 0;
        konfig = Mathf.Floor(Random.Range(1, 4));
        float barpos = Random.Range(-6, -1);
        float barpos2 = Random.Range(-1, 3);
        if (konfig == 1) {
            //berBuilder(barbereich, new Vector3(-7, 0.1f, barpos - 1), new Vector3(-7, 0.1f, barpos + 6), new Vector3(-4, 0.1f, barpos - 1), new Vector3(-4, 0.1f, barpos + 6));
            barBuilder(bar, -7, 0, barpos);
            //berBuilder(buehnenbereich, new Vector3(-4f, 0.1f, 4), new Vector3(-4f, 0.1f, 7), new Vector3(4f, 0.1f, 4), new Vector3(4f, 0.1f, 7));
            //berBuilder(tischbereich, new Vector3(-1, 0.1f, -4.5f), new Vector3(-1, 0.1f, 1), new Vector3(4, 0.1f, -4.5f), new Vector3(4, 0.1f, 1));
            tischmeshwaage(-0.5f, 0, 0, tisch1, hocker7, hocker8, hocker9, hocker10);
            tischmeshsenk(2.5f, 0, -1.5f, tisch2, hocker11, hocker12, hocker13, hocker14);
        }

        if (konfig == 2) {
            //berBuilder(barbereich, new Vector3(4, 0.1f, barpos2 - 6), new Vector3(4, 0.1f, barpos2 + 1), new Vector3(7, 0.1f, barpos2 - 6), new Vector3(7, 0.1f, barpos2 + 1));
            barBuilder2(bar, 7, 0, barpos2);
            //berBuilder(buehnenbereich, new Vector3(-4, 0.1f, 4), new Vector3(-4, 0.1f, 7), new Vector3(+4, 0.1f, 4), new Vector3(+4, 0.1f, 7));
            //berBuilder(tischbereich, new Vector3(-4, 0.1f, -4.5f), new Vector3(-4, 0.1f, 1), new Vector3(1, 0.1f, -4.5f), new Vector3(1, 0.1f, 1));
            tischmeshwaage(0.5f, 0, 0, tisch1, hocker7, hocker8, hocker9, hocker10);
            tischmeshsenk(-2.5f, 0, -1.5f, tisch2, hocker11, hocker12, hocker13, hocker14);
        }

        if (konfig == 3) {
            if (lastkonf != 3) {
                tisch3 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tisch4 = GameObject.CreatePrimitive(PrimitiveType.Quad);

                hocker15 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker16 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker17 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker18 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker19 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker20 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker21 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker22 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker23 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker24 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker25 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker26 = GameObject.CreatePrimitive(PrimitiveType.Quad);
                hocker27 = GameObject.CreatePrimitive(PrimitiveType.Quad);

                tisch3.name = "Tisch3";
                tisch4.name = "Tisch4";

                hocker15.name = "Hocker15";
                hocker16.name = "Hocker16";
                hocker17.name = "Hocker17";
                hocker18.name = "Hocker18";
                hocker19.name = "Hocker19";
                hocker20.name = "Hocker20";
                hocker21.name = "Hocker21";
                hocker22.name = "Hocker22";
                hocker23.name = "Hocker23";
                hocker24.name = "Hocker24";
                hocker25.name = "Hocker25";
                hocker26.name = "Hocker26";
                hocker27.name = "Hocker27";
            }

            //berBuilder(barbereich, new Vector3(-7, 0.1f, barpos - 1), new Vector3(-7, 0.1f, barpos + 6), new Vector3(-4, 0.1f, barpos - 1), new Vector3(-4, 0.1f, barpos + 6));
            barBuilder(bar, -7, 0, barpos);
            //berBuilder(buehnenbereich, new Vector3(-4f, 0.1f, 4), new Vector3(-4f, 0.1f, 7), new Vector3(4f, 0.1f, 4), new Vector3(4f, 0.1f, 7));
            //berBuilder(tischbereich, new Vector3(-3, 0.1f, -5), new Vector3(-3, 0.1f, 1), new Vector3(6, 0.1f, -5), new Vector3(6, 0.1f, 1));
            tischmeshsenk(3.5f, 0, -1, tisch1, hocker7, hocker8, hocker9, hocker10);
            tischmeshsenk(3.5f, 0, -4, tisch2, hocker11, hocker12, hocker13, hocker14);
            tischmeshsenk(-0.5f, 0, -1, tisch3, hocker15, hocker16, hocker17, hocker18);
            tischmeshsenk(-0.5f, 0, -4, tisch4, hocker19, hocker20, hocker21, hocker22);
        }
        if (hockerzahl != maxhocker) {
            for (int i = hockerzahl + 1; i <= maxhocker; i++) {

                nr = i.ToString();
                hock = hock + nr;
                Destroy(GameObject.Find(hock));
                hock = "Hocker";
            }
        }
        if (tischzahl != maxtisch) {
            for (int i = tischzahl + 1; i <= maxtisch; i++) {
                tinr = i.ToString();
                tis = tis + tinr;
                Destroy(GameObject.Find(tis));
                tis = "Tisch";
            }

            GameObject[] tische = getTische();
            for (int i = 0; i < tische.Length; i++) {
                tische[i].GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }

            GameObject[] hockers = getTische();
            for (int i = 0; i < hockers.Length; i++) {
                hockers[i].GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                hockers[i].GetComponent<Renderer>().receiveShadows = true;
            }

            bar.GetComponent<Renderer>().receiveShadows = true;
            bar.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            buehne.GetComponent<Renderer>().receiveShadows = true;
            buehne.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        lastkonf = konfig;
        GameObject.Find("AudienceBehave").GetComponent<AudienceBehave>().Reassemble();
        /*tischbereich.GetComponent<Renderer>().receiveShadows = true;
        buehnenbereich.GetComponent<Renderer>().receiveShadows = true;
        barbereich.GetComponent<Renderer>().receiveShadows = true;
        spawn.GetComponent<Renderer>().receiveShadows = true;*/
    }

    void createUVWand(GameObject Wand) {
        Renderer rend = Wand.GetComponent<Renderer>();
        Material wallMat = Resources.Load<Material>("Materials/Wall2");
        rend.material = wallMat;
        Mesh mesh = Wand.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
        }
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();


    }

    void createUVWandHV(GameObject Wand) {
        Renderer rend = Wand.GetComponent<Renderer>();
        Material wallMat = Resources.Load<Material>("Materials/Wall2");
        rend.material = wallMat;
        Mesh mesh = Wand.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void createUVWandBD(GameObject Wand) {
        Renderer rend = Wand.GetComponent<Renderer>();
        Material wallMat = Resources.Load<Material>("Materials/Floor");
        rend.material = wallMat;
        Mesh mesh = Wand.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void createUVBar(GameObject square) {
        Renderer rend = square.GetComponent<Renderer>();
        Material wallMat = Resources.Load<Material>("Materials/Bark");
        rend.material = wallMat;
        Mesh mesh = square.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        int durchlauf = 0;

        for (int i = 0; i < vertices.Length; i = i + 4) {
            durchlauf++;
            if (durchlauf % 2 == 0) {
                uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
                uvs[i + 1] = new Vector2(vertices[i + 1].z, vertices[i + 1].y);
                uvs[i + 2] = new Vector2(vertices[i + 2].z, vertices[i + 2].y);
                uvs[i + 3] = new Vector2(vertices[i + 3].z, vertices[i + 3].y);
            }

            if (durchlauf % 2 == 1 & durchlauf % 5 != 0) {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
                uvs[i + 1] = new Vector2(vertices[i + 1].x, vertices[i + 1].y);
                uvs[i + 2] = new Vector2(vertices[i + 2].x, vertices[i + 2].y);
                uvs[i + 3] = new Vector2(vertices[i + 3].x, vertices[i + 3].y);
            }

            if (i >= vertices.Length - 12) {
                //Debug.Log("a");
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                uvs[i + 1] = new Vector2(vertices[i + 1].x, vertices[i + 1].z);
                uvs[i + 2] = new Vector2(vertices[i + 2].x, vertices[i + 2].z);
                uvs[i + 3] = new Vector2(vertices[i + 3].x, vertices[i + 3].z);
                durchlauf = 0;
            }
        }

        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void createUVHocker(GameObject square) {
        Renderer rend = square.GetComponent<Renderer>();
        Material wallMat = Resources.Load<Material>("Materials/Bark");
        rend.material = wallMat;
        Mesh mesh = square.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        int durchlauf = 0;

        for (int i = 0; i < vertices.Length; i = i + 4) {
            durchlauf++;
            if (durchlauf % 2 == 1 & durchlauf % 5 != 0) {
                uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
                uvs[i + 1] = new Vector2(vertices[i + 1].z, vertices[i + 1].y);
                uvs[i + 2] = new Vector2(vertices[i + 2].z, vertices[i + 2].y);
                uvs[i + 3] = new Vector2(vertices[i + 3].z, vertices[i + 3].y);
            }

            if (durchlauf % 2 == 0) {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
                uvs[i + 1] = new Vector2(vertices[i + 1].x, vertices[i + 1].y);
                uvs[i + 2] = new Vector2(vertices[i + 2].x, vertices[i + 2].y);
                uvs[i + 3] = new Vector2(vertices[i + 3].x, vertices[i + 3].y);
            }

            if (durchlauf % 5 == 0) {
                //Debug.Log("a");
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                uvs[i + 1] = new Vector2(vertices[i + 1].x, vertices[i + 1].z);
                uvs[i + 2] = new Vector2(vertices[i + 2].x, vertices[i + 2].z);
                uvs[i + 3] = new Vector2(vertices[i + 3].x, vertices[i + 3].z);
                durchlauf = 0;
            }
        }

        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }


    void Start() {

        //Instanzierung der Bereiche auf der Plane Boden
        boden = GameObject.CreatePrimitive(PrimitiveType.Plane);
        /*barbereich = GameObject.CreatePrimitive(PrimitiveType.Quad);
        buehnenbereich = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tischbereich = GameObject.CreatePrimitive(PrimitiveType.Quad);
        kamerabereich = GameObject.CreatePrimitive(PrimitiveType.Quad);*/

        //Instanzierung der Objekte in den Bereichen
        buehne = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bar = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker3 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker4 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker5 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker6 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker7 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker8 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker9 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker10 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker11 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker12 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker13 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hocker14 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tisch1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        tisch2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        treppe = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wand1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wand2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wand3 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wand4 = GameObject.CreatePrimitive(PrimitiveType.Quad);
        decke = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //spawn = GameObject.CreatePrimitive(PrimitiveType.Quad);

        //Instanzierung der Hilfslisten der Objekte
        barVert = new List<Vector3>();
        barTri = new List<int>();

        buVert = new List<Vector3>();
        buTri = new List<int>();

        //Name der Objekte/Bereiche
        boden.name = "Boden";
        /*barbereich.name = "Barbereich";
        buehnenbereich.name = "Buehnenbereich";
        tischbereich.name = "Tischbereich";
        kamerabereich.name = "Kamerabereich";*/
        buehne.name = "Buehne";
        bar.name = "Bar";
        tisch1.name = "Tisch1";
        tisch2.name = "Tisch2";
        hocker1.name = "Hocker1";
        hocker2.name = "Hocker2";
        hocker3.name = "Hocker3";
        hocker4.name = "Hocker4";
        hocker5.name = "Hocker5";
        hocker6.name = "Hocker6";
        hocker7.name = "Hocker7";
        hocker8.name = "Hocker8";
        hocker9.name = "Hocker9";
        hocker10.name = "Hocker10";
        hocker11.name = "Hocker11";
        hocker12.name = "Hocker12";
        hocker13.name = "Hocker13";
        hocker14.name = "Hocker14";
        wand1.name = "wand1";
        wand2.name = "wand2";
        wand3.name = "wand3";
        wand4.name = "wand4";
        decke.name = "decke";
        //spawn.name = "spawn";

        //Bereiche der Taverne einteilen



        //Farben der einzelnen Bereiche
        /* barbereich.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
         buehnenbereich.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
         tischbereich.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f);*/


        //Objekte verschieben/skalieren/rotieren

        //buehne

        buehne.transform.position = new Vector3(0, 0.1f, 5.75f);


        buVert.Add(new Vector3(3f, 0f, -1.25f));
        buVert.Add(new Vector3(3f, 0.5f, -1.25f));
        buVert.Add(new Vector3(3f, 0f, 1.25f));
        buVert.Add(new Vector3(3f, 0.5f, 1.25f));

        buVert.Add(new Vector3(3f, 0f, 1.25f));
        buVert.Add(new Vector3(3f, 0.5f, 1.25f));
        buVert.Add(new Vector3(-3f, 0f, 1.25f));
        buVert.Add(new Vector3(-3f, 0.5f, 1.25f));

        buVert.Add(new Vector3(-3f, 0f, 1.25f));
        buVert.Add(new Vector3(-3f, 0.5f, 1.25f));
        buVert.Add(new Vector3(-3f, 0f, -1.25f));
        buVert.Add(new Vector3(-3f, 0.5f, -1.25f));

        buVert.Add(new Vector3(-3f, 0f, -1.25f));
        buVert.Add(new Vector3(-3f, 0.5f, -1.25f));
        buVert.Add(new Vector3(3f, 0f, -1.25f));
        buVert.Add(new Vector3(3f, 0.5f, -1.25f));

        buVert.Add(new Vector3(-3f, 0.5f, -1.25f));
        buVert.Add(new Vector3(-3f, 0.5f, 1.25f));
        buVert.Add(new Vector3(3f, 0.5f, -1.25f));
        buVert.Add(new Vector3(3f, 0.5f, 1.25f));

        bumesh = new Mesh();
        bumesh.vertices = buVert.ToArray();

        buehne.GetComponent<MeshFilter>().mesh = bumesh;

        int durchlauf = 0;

        for (int i = 0; i < bumesh.vertices.Length - 2; i++) {

            if (durchlauf % 2 == 0) {
                buTri.Add(i);
                buTri.Add(i + 1);
                buTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1) {
                buTri.Add(i + 1);
                buTri.Add(i);
                buTri.Add(i + 2);
            }

            if (durchlauf % 2 == 1 & durchlauf != 0) {
                i = i + 2;
            }


            durchlauf++;
        }



        bumesh.triangles = buTri.ToArray();

        createUVHocker(buehne);


        wand1.GetComponent<Renderer>().receiveShadows = true;
        wand2.GetComponent<Renderer>().receiveShadows = true;
        wand3.GetComponent<Renderer>().receiveShadows = true;
        wand4.GetComponent<Renderer>().receiveShadows = true;
        boden.GetComponent<Renderer>().receiveShadows = true;
        decke.GetComponent<Renderer>().receiveShadows = true;



        //treppe
        treppenmesh(-3f, 0.1f, 5.75f, treppe);

        //wände
        berBuilder(wand1, new Vector3(-7, 0f, -9f), new Vector3(-7, 5, -9f), new Vector3(-7, 0f, 7f), new Vector3(-7f, 5, 7f));
        createUVWand(wand1);
        wand2.transform.position = new Vector3(0, 2.5f, 7);

        berBuilder(wand2, new Vector3(-7, -2.5f, 0), new Vector3(-7, 2.5f, 0), new Vector3(7, -2.5f, 0), new Vector3(7, 2.5f, 0));
        createUVWandHV(wand2);
        berBuilder(wand3, new Vector3(7, 0, 7), new Vector3(7, 5, 7), new Vector3(7, 0, -9), new Vector3(7, 5, -9));
        createUVWand(wand3);
        berBuilder(wand4, new Vector3(7, 0, -7), new Vector3(7, 5, -7), new Vector3(-7, 0, -7), new Vector3(-7, 5, -7));
        createUVWandHV(wand4);

        berBuilder(decke, new Vector3(7, 5, -9), new Vector3(7, 5, 7), new Vector3(-7, 5, -9), new Vector3(-7, 5, 7));
        createUVWandBD(decke);
        berBuilder(boden, new Vector3(-7, 0, -9), new Vector3(-7, 0, 7), new Vector3(7, 0, -9), new Vector3(7, 0, 7));
        createUVWandBD(boden);

        //berBuilder(spawn, new Vector3(4.5f, 0.1f, 4.5f), new Vector3(4.5f, 0.1f, 6.5f), new Vector3(6.5f, 0.1f, 4.5f), new Vector3(6.5f, 0.1f, 6.5f));
        //berBuilder(kamerabereich, new Vector3(-7, 0.1f, -9), new Vector3(-7, 0.1f, -7), new Vector3(7, 0.1f, -9), new Vector3(7, 0.1f, -7));
        randomize();

        NotifyAll();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            randomize();
        }
    }

    public GameObject getBuehne() {
        return buehne;
    }

    public GameObject[] getHocker() {
        string str = "Hocker";
        GameObject[] a = new GameObject[hockerzahl];
        for (int i = 1; i <= hockerzahl; i++) {
            str = str + i;
            a[i - 1] = GameObject.Find(str);
            str = "Hocker";
        }
        return a;

    }

    public GameObject[] getTische() {
        string str = "Tisch";
        GameObject[] a = new GameObject[tischzahl];
        for (int i = 1; i <= tischzahl; i++) {
            str = str + i;
            a[i - 1] = GameObject.Find(str);
            str = "Tisch";
        }
        return a;
    }
    public GameObject getWandRechts() {
        return wand3;
    }

    /* Reihenfolge der Vertices von der sichtbaren Seite der Wand: links unten, links oben, rechts unten, rechts oben*/
    public Vector3[] getWandRechtsVert() {
        Vector3[] a = new Vector3[4];
        a[0] = new Vector3(7, 0, 7);
        a[1] = new Vector3(7, 5, 7);
        a[2] = new Vector3(7, 0, -7);
        a[3] = new Vector3(7, 5, -7);
        return a;
    }

    public GameObject getWandLinks() {
        return wand1;
    }

    public Vector3[] getWandLinksVert() {
        Vector3[] a = new Vector3[4];
        a[0] = new Vector3(-7, 0, -7);
        a[1] = new Vector3(-7, 5, -7);
        a[2] = new Vector3(-7, 0, 7);
        a[3] = new Vector3(-7, 5, 7);
        return a;
    }

    public GameObject getWandFern() {
        return wand2;
    }

    public Vector3[] getWandFernVert() {
        Vector3[] a = new Vector3[4];
        a[0] = new Vector3(-7, 0, 7);
        a[1] = new Vector3(-7, 5, 7);
        a[2] = new Vector3(7, 0, 7);
        a[3] = new Vector3(7, 5, 7);
        return a;
    }

    public GameObject getWandNah() {
        return wand4;
    }

    public Vector3[] getWandNahVert() {
        Vector3[] a = new Vector3[4];
        a[0] = new Vector3(7, 0, -7);
        a[1] = new Vector3(7, 5, -7);
        a[2] = new Vector3(-7, 0, -7);
        a[3] = new Vector3(-7, 5, -7);
        return a;
    }

    public GameObject getBoden() {
        return boden;
    }

    public GameObject getDecke() {
        return decke;
    }

    public GameObject getSpawn() {
        return spawn;
    }

    public GameObject getBar() {
        return bar;
    }

    public GameObject getTreppe() {
        return treppe;
    }

    public Vector3[] getSpawnVert() {
        Vector3[] a = new Vector3[4];
        a[0] = new Vector3(4.5f, 0.1f, 4.5f);
        a[1] = new Vector3(4.5f, 0.1f, 6.5f);
        a[2] = new Vector3(6.5f, 0.1f, 4.5f);
        a[3] = new Vector3(6.5f, 0.1f, 6.5f);
        return a;
    }

    public Vector3 getBarkeep() {
        if (konfig == 2) {
            return new Vector3(bar.transform.position.x - 0.5f, bar.transform.position.y, bar.transform.position.z - 2.5f);
        } else {
            return new Vector3(bar.transform.position.x + 0.5f, bar.transform.position.y, bar.transform.position.z + 2.5f);
        }
    }

    public void UpdateObserver()
    {
        
            GameObject character = GameObject.Find("Bard");
            character.GetComponent<Rigidbody>().isKinematic = false;
            Vector3[] spawnPos = getSpawnVert();
            character.transform.position = new Vector3(spawnPos[0].x, 0.6f, spawnPos[0].z);
            character.transform.Translate((spawnPos[3].x - spawnPos[0].x) / 2, 0, (spawnPos[3].z - spawnPos[0].z) / 2);
            // Set LookAt Point
            character.transform.LookAt(new Vector3(0, 0, 0));
            character.transform.Rotate(0, -90, 0);
            character.GetComponent<Rigidbody>().isKinematic = true;
            //init.UpdateJoint();
            NotifyAll();
        
    }
}