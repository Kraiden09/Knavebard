using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beatmap : MonoBehaviour {
    /*
     * ALLE WICHTIGEN STRUCTS IN ZUSAMMENHANG
     * MIT DER ERSTELLUNG DES SONGS
     */

    public struct Beatmap
    {
        // Anzahl der Inputs und der Zeitangaben => Arraygroesse
        public int songlength;

        // Zeitabstaende zwischen Noten
        public int[] timing;

        // festgelegte Noten des Songs
        public Directions[] directions;
    }

    /* Richtungen des Inputs:
     *      q   u   e
     *      l       r
     *      y   d   c
     *      
     * auf niedriger Schwierigkeit nur:
     *          u
     *      l       r
     *          d
     */
    public struct Directions
    {
        // Richtung des Inputs
        public char note;

        // Typ des Inputs, z.b. "h" fuer "hold" also eine langezogene Note
        // "n" fuer "normal" als einzelner Tastendruck
        public char type;
    }

    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */
}
