using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteReader : MonoBehaviour {
    NoteBoard noteBoard;

    //errechnet aus den 122bpm
    //float beat =  0.49180327868f
    //beat * 2 =      0.98360655737f

    //songinfo (spaeter auslagern)
    int[] songNotes = {1, 0, 2, 1,
        1, 1, 1, 1,
        1, 1, 1, 1,
        2, 2, 2, 2 };

    float[] noteTiming = { 0.49180327868f,0.49180327868f,0.49180327868f,      0.49180327868f,
        0.98360655737f, 0.98360655737f, 0.98360655737f, 0.98360655737f,
        0.98360655737f, 0.98360655737f, 0.98360655737f, 0.98360655737f,
        0.98360655737f, 0.98360655737f, 0.98360655737f, 0.98360655737f };


// Use this for initialization
void Start () {


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
	
	// Update is called once per frame
	void Update () {
	}

    public int readNotes(int i)
    {
        return songNotes[i];
    }

    public float readTime(int i)
    {
        return noteTiming[i];
    }

    public int songlength()
    {
        return songNotes.Length;
    }
}
