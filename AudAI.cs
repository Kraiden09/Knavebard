using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudAI : MonoBehaviour {
    public int id;
    AudienceBehave audienceBehave;
    GameObject iam;
    bool inZone;

    AudioClip[] shouts;
    AudioSource exclamation;

    // Use this for initialization
    void Start() {
        GetComponent<BoxCollider>().size = new Vector3(5, 4, 5);
        GetComponent<BoxCollider>().isTrigger = true;
        audienceBehave = GameObject.Find("AudienceBehave").GetComponent<AudienceBehave>();
        iam = transform.gameObject;
        inZone = false;

        shouts = new AudioClip[4];

        // Audio from Asset Store -> Universal Sound FX by imphenzia
        exclamation = gameObject.AddComponent<AudioSource>();
        shouts[0] = Resources.Load<AudioClip>("Universal Sound FX/Voices/Exclamations/EXCLAMATION_Male_B_Woh_01_mono");
        shouts[1] = Resources.Load<AudioClip>("Universal Sound FX/Voices/Exclamations/EXCLAMATION_Male_B_Wooah_01_mono");
        shouts[2] = Resources.Load<AudioClip>("Universal Sound FX/Voices/Exclamations/EXCLAMATION_Male_B_Yippee_01_mono");
        shouts[3] = Resources.Load<AudioClip>("Universal Sound FX/Voices/Exclamations/EXCLAMATION_Male_B_Whoo_01_mono"); 
        exclamation.volume = 0.07f;
    }

    /*
	// Update is called once per frame
	void Update () {
        //could eat performance
        if (inZone)
        {
            if (Input.GetKeyDown("return"))
            {
                StartCoroutine(audienceBehave.Jump(iam));
            }
        }
    }
    */

    /*
// moving by little hops (be as cute as possible)
public void HopTo(Vector3 destination)
{
    transform.Translate(destination);
}


//wiggle to the beat from side to side (while sitting)
public void Wiggle()
{

}
*/
    /*
    public void OnTriggerEnter(Collider other)
    {
        inZone = true;
    }

    public void OnTriggerExit(Collider other)
    {
        inZone = false;
    }
    */

    public void OnTriggerStay(Collider other) {
        if (!audienceBehave.jumping) {
            if (Input.GetKeyDown("return")) {
                exclamation.clip = shouts[Random.Range(0, shouts.Length)];
                exclamation.Play();
                StartCoroutine(audienceBehave.Jump(iam));
                audienceBehave.jumping = true;
            }
        }
    }
}