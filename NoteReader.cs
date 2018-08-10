using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteReader : MonoBehaviour {
    NoteBoard noteBoard;


    //songinfo (spaeter auslagern)
    int[] songNotes = { 1, 2, 3, 0, 1, 2, 3, 1, 1, 1, 2, 2, 2 };
    float[] noteTiming = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };


    // Use this for initialization
    void Start () {
		
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
