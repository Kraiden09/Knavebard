﻿using System.Collections;
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
    GameObject tisch1;
    GameObject tisch2;
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
    GameObject treppe;
    GameObject wand1;
    GameObject wand2;
    GameObject wand3;
    GameObject wand4;
    GameObject decke;

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

        tiVert = new List<Vector3>();
        tiTri = new List<int>();

        tiVert.Add(new Vector3(xpos + 0.8f, ypos, zpos - 0.4f));
        tiVert.Add(new Vector3(xpos + 0.8f, ypos + 0.6f, zpos - 0.4f));
        tiVert.Add(new Vector3(xpos + 0.8f, ypos, zpos + 0.4f));
        tiVert.Add(new Vector3(xpos + 0.8f, ypos + 0.6f, zpos + 0.4f));

        tiVert.Add(new Vector3(xpos - 0.8f, ypos, zpos + 0.4f));
        tiVert.Add(new Vector3(xpos - 0.8f, ypos + 0.6f, zpos + 0.4f));

        tiVert.Add(new Vector3(xpos - 0.8f, ypos, zpos - 0.4f));
        tiVert.Add(new Vector3(xpos - 0.8f, ypos + 0.6f, zpos - 0.4f));

        tiVert.Add(new Vector3(xpos + 0.8f, ypos, zpos - 0.4f));
        tiVert.Add(new Vector3(xpos + 0.8f, ypos + 0.6f, zpos - 0.4f));

        tiVert.Add(new Vector3(xpos - 0.8f, ypos + 0.6f, zpos - 0.4f));
        tiVert.Add(new Vector3(xpos - 0.8f, ypos + 0.6f, zpos + 0.4f));
        tiVert.Add(new Vector3(xpos + 0.8f, ypos + 0.6f, zpos - 0.4f));
        tiVert.Add(new Vector3(xpos + 0.8f, ypos + 0.6f, zpos + 0.4f));

        timesh = new Mesh();
        timesh.vertices = tiVert.ToArray();

        tisch.GetComponent<MeshFilter>().mesh = timesh;

        for (int i = 0; i < timesh.vertices.Length - 2; i++)
        {
            if (i % 2 == 0)
            {
                tiTri.Add(i);
                tiTri.Add(i + 1);
                tiTri.Add(i + 2);
            }

            if (i % 2 == 1)
            {
                tiTri.Add(i + 1);
                tiTri.Add(i);
                tiTri.Add(i + 2);
            }
        }

        timesh.triangles = tiTri.ToArray();

        tisch.GetComponent<Renderer>().material.color = new Color(0.0f,0.0f,1.0f);

        //Hocker werden an Tisch angesetzt...Austausch mit Stuehlen?
        hockermesh(xpos - 0.4f, ypos, zpos + 0.8f, hocker1);
        hockermesh(xpos + 0.4f, ypos, zpos + 0.8f, hocker2);
        hockermesh(xpos - 0.4f, ypos, zpos - 0.8f, hocker3);
        hockermesh(xpos + 0.4f, ypos, zpos - 0.8f, hocker4);
    }

    //senkrechte Tischmethode
    void tischmeshsenk(float xpos, float ypos, float zpos, GameObject tisch, GameObject hocker1, GameObject hocker2, GameObject hocker3, GameObject hocker4)
    {

        tiVert = new List<Vector3>();
        tiTri = new List<int>();

        tiVert.Add(new Vector3(xpos + 0.4f, ypos, zpos - 0.8f));
        tiVert.Add(new Vector3(xpos + 0.4f, ypos + 0.6f, zpos - 0.8f));
        tiVert.Add(new Vector3(xpos + 0.4f, ypos, zpos + 0.8f));
        tiVert.Add(new Vector3(xpos + 0.4f, ypos + 0.6f, zpos + 0.8f));

        tiVert.Add(new Vector3(xpos - 0.4f, ypos, zpos + 0.8f));
        tiVert.Add(new Vector3(xpos - 0.4f, ypos + 0.6f, zpos + 0.8f));

        tiVert.Add(new Vector3(xpos - 0.4f, ypos, zpos - 0.8f));
        tiVert.Add(new Vector3(xpos - 0.4f, ypos + 0.6f, zpos - 0.8f));

        tiVert.Add(new Vector3(xpos + 0.4f, ypos, zpos - 0.8f));
        tiVert.Add(new Vector3(xpos + 0.4f, ypos + 0.6f, zpos - 0.8f));

        tiVert.Add(new Vector3(xpos - 0.4f, ypos + 0.6f, zpos - 0.8f));
        tiVert.Add(new Vector3(xpos - 0.4f, ypos + 0.6f, zpos + 0.8f));
        tiVert.Add(new Vector3(xpos + 0.4f, ypos + 0.6f, zpos - 0.8f));
        tiVert.Add(new Vector3(xpos + 0.4f, ypos + 0.6f, zpos + 0.8f));

        timesh = new Mesh();
        timesh.vertices = tiVert.ToArray();

        tisch.GetComponent<MeshFilter>().mesh = timesh;

        for (int i = 0; i < timesh.vertices.Length - 2; i++)
        {
            if (i % 2 == 0)
            {
                tiTri.Add(i);
                tiTri.Add(i + 1);
                tiTri.Add(i + 2);
            }

            if (i % 2 == 1)
            {
                tiTri.Add(i + 1);
                tiTri.Add(i);
                tiTri.Add(i + 2);
            }
        }

        timesh.triangles = tiTri.ToArray();

        tisch.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);

        //Hocker werden an Tisch angesetzt...Austausch mit Stuehlen?
        hockermesh(xpos - 0.8f, ypos, zpos + 0.4f, hocker1);
        hockermesh(xpos + 0.8f, ypos, zpos + 0.4f, hocker2);
        hockermesh(xpos - 0.8f, ypos, zpos - 0.4f, hocker3);
        hockermesh(xpos + 0.8f, ypos, zpos - 0.4f, hocker4);
    }

    void hockermesh(float xpos, float ypos, float zpos, GameObject hocker)
    {

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

        hocker.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 1.0f);
    }

    //Treppe an der Buehne
    void treppenmesh(float xpos, float ypos, float zpos, GameObject treppe) {
        treVert = new List<Vector3>();
        treTri = new List<int>();

        treVert.Add(new Vector3(xpos - 0.4f, 0.1f, zpos - 0.5f));
        treVert.Add(new Vector3(xpos - 0.4f, 0.1f + (0.5f/3), zpos - 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f, zpos - 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + (0.5f/3), zpos - 0.5f));

        treVert.Add(new Vector3(xpos, 0.1f, zpos + 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + (0.5f/3), zpos + 0.5f));

        treVert.Add(new Vector3(xpos - 0.4f, 0.1f, zpos + 0.5f));
        treVert.Add(new Vector3(xpos - 0.4f, 0.1f + (0.5f/3), zpos + 0.5f));

        treVert.Add(new Vector3(xpos - 0.4f, 0.1f, zpos - 0.5f));
        treVert.Add(new Vector3(xpos - 0.4f, 0.1f + (0.5f / 3), zpos - 0.5f));
        //hier muss cnt erhöht werden sonst zu viele tris
        treVert.Add(new Vector3(xpos - 0.4f, 0.1f + (0.5f / 3), zpos - 0.5f));
        treVert.Add(new Vector3(xpos - 0.4f, 0.1f + (0.5f / 3), zpos + 0.5f));
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + (0.5f / 3), zpos - 0.5f));
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + (0.5f / 3), zpos + 0.5f));
        //cnt++
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + (0.5f / 3), zpos - 0.5f));
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + ((0.5f / 3)*2), zpos - 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + (0.5f / 3), zpos - 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + ((0.5f / 3) * 2), zpos - 0.5f));

        treVert.Add(new Vector3(xpos, 0.1f + (0.5f / 3), zpos + 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + ((0.5f / 3) * 2), zpos + 0.5f));

        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + (0.5f / 3), zpos + 0.5f));
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + ((0.5f / 3) * 2), zpos + 0.5f));

        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + (0.5f / 3), zpos + 0.5f));
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + ((0.5f / 3) * 2), zpos + 0.5f));
        //cnt++
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + ((0.5f / 3) * 2), zpos - 0.5f));
        treVert.Add(new Vector3(xpos - 0.2f, 0.1f + ((0.5f / 3) * 2), zpos + 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + ((0.5f / 3) * 2), zpos - 0.5f));
        treVert.Add(new Vector3(xpos, 0.1f + ((0.5f / 3) * 2), zpos + 0.5f));


        tremesh = new Mesh();
        tremesh.vertices = treVert.ToArray();

        treppe.GetComponent<MeshFilter>().mesh =tremesh;

        for (int i = 0; i < tremesh.vertices.Length - 2; i++)
        {
            if (i % 2 == 0)
            {
                treTri.Add(i);
                treTri.Add(i + 1);
                treTri.Add(i + 2);
            }

            if (i % 2 == 1)
            {
                treTri.Add(i + 1);
                treTri.Add(i);
                treTri.Add(i + 2);
            }
        }

        tremesh.triangles = treTri.ToArray();

        treppe.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 1.0f);
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

    //Zufällige Generierung
    void randomize() {
        float konfig = Mathf.Floor(Random.Range(1, 3));
        Debug.Log(konfig);
        if (konfig == 1) {
            berBuilder(barbereich, new Vector3(-5, 0.1f, -5f), new Vector3(-5, 0.1f, 0), new Vector3(-2, 0.1f, -5f), new Vector3(-2, 0.1f, 0));
            berBuilder(buehnenbereich, new Vector3(-3, 0.1f, 2), new Vector3(-3, 0.1f, 5), new Vector3(+3, 0.1f, 2), new Vector3(+3, 0.1f, 5));
            berBuilder(tischbereich, new Vector3(-1, 0.1f, -4.5f), new Vector3(-1, 0.1f, 1), new Vector3(4, 0.1f, -4.5f), new Vector3(4, 0.1f, 1));
        }

        if (konfig == 2)
        {
            berBuilder(barbereich, new Vector3( 2, 0.1f, -5f), new Vector3( 2, 0.1f, 0), new Vector3( 5, 0.1f, -5f), new Vector3( 5, 0.1f, 0));
            berBuilder(buehnenbereich, new Vector3(-3, 0.1f, 2), new Vector3(-3, 0.1f, 5), new Vector3(+3, 0.1f, 2), new Vector3(+3, 0.1f, 5));
            berBuilder(tischbereich, new Vector3(-4, 0.1f, -4.5f), new Vector3(-4, 0.1f, 1), new Vector3( 1, 0.1f, -4.5f), new Vector3( 1, 0.1f, 1));
        }

        if (konfig == 3)
        {

        }

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

        //Instanzierung der Hilfslisten der Objekte
        barVert = new List<Vector3>();
        barTri = new List<int>();

        buVert = new List<Vector3>();
        buTri = new List<int>();

        //Name der Objekte/Bereiche
        boden.name = "Boden";
        barbereich.name = "Barbereich";
        buehnenbereich.name = "Buehnenbereich";
        tischbereich.name = "Tischbereich";
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

        //Bereiche der Taverne einteilen



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

        //Tische
        tischmeshwaage(0.5f, 0.1f, -1, tisch1, hocker7, hocker8, hocker9, hocker10);
        tischmeshsenk(2.5f, 0.1f, -2.5f, tisch2, hocker11, hocker12, hocker13, hocker14);

        //Farben der Objekte
        buehne.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
        bar.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
        boden.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
        decke.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
        wand1.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
        wand2.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
        wand3.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
        wand4.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);

        //treppe
        treppenmesh(-2.5f, 0.6f, 3.75f, treppe);

        //wände
        berBuilder( wand1, new Vector3(-5, 0f, -5f), new Vector3(-5, 3, -5f), new Vector3(-5, 0f, 5f), new Vector3(-5f, 3, 5f));
        berBuilder( wand2, new Vector3(-5, 0f, 5), new Vector3(-5, 3, 5), new Vector3( 5, 0f, 5), new Vector3(5, 3, 5));
        berBuilder( wand3, new Vector3( 5, 0, 5), new Vector3( 5, 3, 5), new Vector3(5, 0, -5), new Vector3(5, 3, -5));
        berBuilder( wand4, new Vector3(5, 0, -5), new Vector3(5, 3, -5), new Vector3(-5, 0, -5), new Vector3(-5, 3, -5));

        berBuilder(decke, new Vector3(5, 3, -5), new Vector3(5, 3, 5), new Vector3( -5, 3, -5), new Vector3(-5, 3, 5));

        randomize();
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.S))
            {
                randomize();
            }
	}
}