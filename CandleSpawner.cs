using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleSpawner : MonoBehaviour {
    GameObject candle, wick;
    float height;

	// Use this for initialization
	void Start () {
        height = 2f;
        InitCandle();
    }
	
	// Update is called once per frame
	void Update () {
	}

    void InitCandle() {
        candle = new GameObject();
        wick = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wick.transform.parent = candle.transform;
        candle.name = "Candle";
        wick.name = "Wick";
        float wickHeight = (height + 0.35f);
        wick.transform.localScale = new Vector3(0.02f, wickHeight / 2, 0.02f);
        wick.transform.Translate(0, wickHeight / 2, 0);
        wick.GetComponent<Renderer>().material.color = new Color(0.44f, 0.31f, 0.18f);
        CreateVertices(20, 5);
    }

    void CreateVertices(int verticesPerLayer, int layers) {
        Vector3[] newVertices = new Vector3[verticesPerLayer * layers + 1];
        for (int j = 0; j < layers; j++) {
            for (int i = 0; i < verticesPerLayer; i++) {
                //newVertices[i + (j * (verticesPerLayer + 1))] = new Vector3(0.5f, ((height / (layers - 1)) * (j - 1) + (height / (layers - 1))), 0f);
                newVertices[i + (j * verticesPerLayer)] = new Vector3(0.5f, ((height / (layers - 1)) * (j - 1) + (height / (layers - 1))), 0f);
            }
            //newVertices[((j + 1) * verticesPerLayer) + j] = new Vector3(0, ((height / (layers - 1)) * (j - 1) + (height / (layers - 1))), 0f);
        }
        newVertices[verticesPerLayer * layers] = new Vector3(0, ((height / (layers - 1)) * (layers - 2) + (height / (layers - 1))), 0f);

        // Rotate
        Quaternion rotation = Quaternion.Euler(0, 360 / verticesPerLayer, 0);
        for (int j = 0; j < newVertices.Length; j++) { 
            for (int i = 0; i < (newVertices.Length - j); i++) {
                newVertices[i] = rotation * newVertices[i];
            }
        }

        // For Testing purposes
        for (int i = 0; i < newVertices.Length; i++) {
            GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            test.transform.position = newVertices[i];
            test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
