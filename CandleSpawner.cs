using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleSpawner : MonoBehaviour {
    GameObject candle, wick;
    float height;
    int lastLayerIndexStart, verticesPerLayer, layers;
    bool initialized;
    Vector3[] newVertices;
    Mesh candleMesh;

    // Use this for initialization
    void Start () {
        verticesPerLayer = 20;
        layers = 5;
        initialized = false;
        height = 2f;
        InitCandle();
        while (!initialized) {
            // Wait until initialized
        }
        StartCoroutine(BurnDown());
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
        CreateVertices();
    }

    void CreateVertices() {
        newVertices = new Vector3[verticesPerLayer * layers + 1];
        for (int j = 0; j < layers; j++) {
            for (int i = 0; i < verticesPerLayer; i++) {
                if (i == 0) lastLayerIndexStart = i + (j * verticesPerLayer);
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
        /*for (int i = 0; i < newVertices.Length; i++) {
            GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            test.transform.position = newVertices[i];
            test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }*/

        List<int> faces = new List<int>();

        for (int j = 0; j < (layers - 1); j++) {
            for (int i = 0; i < verticesPerLayer; i++) {
                faces.Add(i + 1 + (j * verticesPerLayer));
                faces.Add(i + (j * verticesPerLayer));
                faces.Add(i + verticesPerLayer + (j * verticesPerLayer));

                faces.Add(i + verticesPerLayer + (j * verticesPerLayer));
                faces.Add(i + verticesPerLayer + 1 + (j * verticesPerLayer));
                faces.Add(i + 1 + (j * verticesPerLayer));

                if (j == (layers - 2) && i < (verticesPerLayer - 1)) {
                    faces.Add(i + verticesPerLayer + 1 + (j * verticesPerLayer));
                    faces.Add(i + verticesPerLayer + (j * verticesPerLayer));
                    faces.Add(newVertices.Length - 1);
                }
            }
            if (j == 0) {
                faces.Add(0 + (j * verticesPerLayer));
                faces.Add((verticesPerLayer - 1) + (j * verticesPerLayer));
                faces.Add(verticesPerLayer + (j * verticesPerLayer));
            }
        }

        candleMesh = new Mesh();
        candle.AddComponent<MeshFilter>();
        candle.AddComponent<MeshRenderer>();
        candle.GetComponent<MeshRenderer>().receiveShadows = true;
        candle.GetComponent<MeshFilter>().mesh = candleMesh;
        candle.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);

        candleMesh.vertices = newVertices;
        candleMesh.triangles = faces.ToArray();

        initialized = true;
    }

    IEnumerator BurnDown() {
        int i = 0;
        int lastIndex = newVertices.Length - 1;
        float burnValue = 0.01f;
        while (newVertices[lastLayerIndexStart].y >= newVertices[lastLayerIndexStart - verticesPerLayer].y) {
            newVertices[lastIndex] = newVertices[lastIndex] + new Vector3(0, -burnValue, 0);
            newVertices[lastLayerIndexStart] = newVertices[lastLayerIndexStart] + new Vector3(0, -burnValue, 0);
            newVertices[lastLayerIndexStart + 1] = newVertices[lastLayerIndexStart + 1] + new Vector3(0, -(burnValue / 2), 0);
            newVertices[lastLayerIndexStart - 1] = newVertices[lastLayerIndexStart - 1] + new Vector3(0, -(burnValue / 2), 0);
            candleMesh.vertices = newVertices;
            i++;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
