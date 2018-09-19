using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupTrigger : MonoBehaviour {
    public Text triggerText;
    AudioSource Slurp1;
    AudioSource Slurp2;
    AudioSource Slurp3;
    AudioSource Slurp4;
    GameObject cup1, cup2, cup3, cup4;
	// Use this for initialization
	void Start () {
        triggerText = GameObject.Find("InteractionText").GetComponent<Text>();

        cup1 = GameObject.Find("cup1");
        cup2 = GameObject.Find("cup2");
        cup3 = GameObject.Find("cup3");
        cup4 = GameObject.Find("cup4");

        Slurp1 = cup1.AddComponent<AudioSource>();
        Slurp1.clip = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/DRINK/DRINK_Long_mono");
        Slurp1.volume = 0.5f;

        Slurp2 = cup2.AddComponent<AudioSource>();
        Slurp2.clip = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/DRINK/DRINK_Long_mono");
        Slurp2.volume = 0.5f;

        Slurp3 = cup3.AddComponent<AudioSource>();
        Slurp3.clip = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/DRINK/DRINK_Long_mono");
        Slurp3.volume = 0.5f;

        Slurp4 = cup4.AddComponent<AudioSource>();
        Slurp4.clip = Resources.Load<AudioClip>("Universal Sound FX/HUMAN/DRINK/DRINK_Long_mono");
        Slurp4.volume = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void TextAtTrigger()
    {

            triggerText.text = "Press \"C\" to drink.";

    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.name == "Bard")
        {
            if (triggerText == null)
            {
                triggerText = GameObject.Find("InteractionText").GetComponent<Text>();
            }
            TextAtTrigger();
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (Input.GetKeyDown(KeyCode.C)){
            Slurp1.Play();
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (triggerText != null)
        {
            triggerText.text = "";
        }
    }
}
