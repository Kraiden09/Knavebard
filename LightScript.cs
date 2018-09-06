using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    private Light Candle;
    private AudienceBehave jam;

    private int jamSchwabi;

    private Color red;
    private Color yellow;
    private Color green;

    private bool upordown;

	// Use this for initialization
	void Start () {
        Candle = GetComponent<Light>();
        jam = FindObjectOfType<AudienceBehave>();
        Candle.intensity = 0.15f;
        Candle.range = 7;
        red = new Color(255, 0, 0);
        yellow = new Color(255, 222, 0);
        green = new Color(0, 255, 0);
        Candle.color = yellow;
        upordown = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (jam.jam < 0) {
            jamSchwabi = 0;
        }
        else if (jam.jam >= 0 && jam.jam < 20) {
            jamSchwabi = 1;
        }
        else {
            jamSchwabi = 2;
        }
        Debug.LogWarning(jamSchwabi);
		switch (jamSchwabi) {
            case 0:
                Debug.LogWarning("CASE=rot");
                StartFadeLight(red);
                break;
            case 1:
                Debug.LogWarning("CASE=gelb");
                StartFadeLight(yellow);
                break;
            case 2:
                Debug.LogWarning("CASE=grün");
                StartFadeLight(green);
                break;
            default:
                Debug.LogWarning("Lichtwechsel - Fehler");
                break;
        }

        /* //Flackern
        
        if (Random.value > 0.5f)
            upordown = true;
        else
            upordown = false;

        if (Candle.intensity > 0.1f && upordown)
            Candle.intensity += (Time.deltaTime+Random.value);

        if (Candle.intensity < 0.8f && !upordown)
            Candle.intensity -= (Time.deltaTime + Random.value);
            */
    }

    void StartFadeLight(Color Color) {
        StartCoroutine(FadeLightCore(Color));
    }

    IEnumerator FadeLightCore(Color Color) {
        Color tmp = Candle.color;
        while (Color != Candle.color) {
            if (Color == red) {
                Candle.color = Candle.color + new Color(0, -1, 0);
                Debug.LogWarning("-grün");
            } else if (Color == green) {
                if (Candle.color.g == 255) {
                    Candle.color = Candle.color + new Color(-1, 0, 0);
                    Debug.LogWarning("-rot");
                }
                else {
                    Candle.color = Candle.color + new Color(-1, 1, 0);
                    Debug.LogWarning("-rot|+grün");
                }
            } else if (Color == yellow) {
                if (tmp == red) {
                    Candle.color = Candle.color + new Color(0, 1, 0);
                    Debug.LogWarning("+grün");
                } else if (tmp == green) {
                    if (Candle.color.g == 222) {
                        Candle.color = Candle.color + new Color(1, 0, 0);
                        Debug.LogWarning("+rot");
                    }
                    else {
                        Candle.color = Candle.color + new Color(1, -1, 0);
                        Debug.LogWarning("+rot|-grün");
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);

        }
    }
}
