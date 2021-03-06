﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* add it to Gameobject "NoteReader" in Unity
 * 
 * contains Beatmap and return-Methods for it
 */
public class NoteReader : MonoBehaviour {
    NoteBoard noteBoard;

    //errechnet aus den 122bpm
    //float beat =  0.49180327868f
    //beat * 2 =      0.97960655737f

    //songinfo (spaeter auslagern)
    //0 = up, 1 = right, 2 = down, 3 = left
    int[] songNotes = {1, 0, 2, 1,
        1, 0, 3, 0,
        3, 0, 3, 0, 
        3, 0, 3, 03, 
    0, 3, 0, 1,

    //part 2
        2, 1, 2, 1,
    2, 1, 2, 1,
    2, 1, 2, 1,
    2, 1,
        //part 3
    };
    

    float[] noteTiming = { 0.49180327868f,0.49180327868f, 0.49180327868f,      0.49180327868f,
        0.95560655737f, 0.97960655737f, 0.97960655737f, 0.97960655737f,
        0.97960655737f, 0.97960655737f, 0.97960655737f, 0.97960655737f,
        0.97960655737f, 0.97960655737f, 0.97960655737f, 0.97960655737f,
    0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,
    0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,
    0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,
    0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,

    //part 2
        0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,
    0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,
    0.97960655737f,0.97960655737f,0.97960655737f,0.97960655737f,
    0.97960655737f,0.97960655737f
        //part 3
    };


    // Use this for initialization
    void Start() {
        /*
        float[] noteTiming = { beat,beat,beat,      0.5f,
        1, 1, 1, 1,
        1, 1, 1, 1,
        1, 1, 1, 1, 1 };
        */
        /*
        for (int i = 0; i < songNotes.Length; i++)
        {
            if (i % 5 == 0 && i > 0) {
                noteTiming[i] = beat * 2;
            }
            else{
                noteTiming[i] = beat;
            }
            
        }*/
    }

    /*no update needed
    // Update is called once per frame
    void Update() {
    }
    */
    // stuff for giving notes to others
    public int readNotes(int i) {
        return songNotes[i];
    }

    public float readTime(int i) {
        return noteTiming[i];
    }

    public int songlength() {
        return songNotes.Length;
    }
}