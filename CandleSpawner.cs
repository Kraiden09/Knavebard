using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleSpawner : MonoBehaviour {
    taverne tavern;
    GameObject candle, wick, candleLight, candleHolder;
    float height;
    int lastLayerIndexStart, verticesPerLayer, layers;
    bool initialized, wallLocated;
    Vector3[] newVertices;
    Mesh candleMesh;

    Vector3 posWallLeft;

    // Use this for initialization
    void Start () {
        initialized = false;
        wallLocated = false;
        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        StartCoroutine(GetWallLeft());
        verticesPerLayer = 20;
        layers = 5;
        height = 2f;
        InitCandle();
        while (!initialized && !wallLocated) {
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

        candleLight = new GameObject();
        candleLight.transform.parent = wick.transform;
        candleLight.name = "Candle Light";
        candleLight.transform.position = new Vector3(0, wickHeight, 0);
        Light lightComp = candleLight.AddComponent<Light>();
        lightComp.color = new Color(1, 0.8393834f, 0.4009434f);

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

        CreateFaces(faces);

        candleMesh = new Mesh();
        candle.AddComponent<MeshFilter>();
        candle.AddComponent<MeshRenderer>();
        candle.GetComponent<MeshRenderer>().receiveShadows = true;
        candle.GetComponent<MeshFilter>().mesh = candleMesh;
        candle.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);

        candleMesh.vertices = newVertices;
        candleMesh.triangles = faces.ToArray();

        candleMesh.RecalculateNormals();

        candle.transform.localScale = new Vector3(0.359f, 0.359f, 0.359f);

        CreateCandleHolder();

        initialized = true;
    }

    void CreateFaces(List<int> faces) {
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
    }

    void CreateCandleHolder() {
        candleHolder = (GameObject)Instantiate(Resources.Load("Prefab/CandleHolder"));
        candleHolder.transform.position = posWallLeft;
    }

    IEnumerator GetWallLeft() {
        bool valid = false;
        while (!valid) {
            valid = true;
            try {
                posWallLeft = tavern.getWandLinks();
            } catch (NullReferenceException nre) {
                valid = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
        wallLocated = true;
    }

    IEnumerator BurnDown() {
        int lastIndex = newVertices.Length - 1;

        float burnValue = 0.015f;
        float burnTimeRate = 0.1f;

        float wickBurnt;
        bool middleBurnt = false, layerBurnt = false;

        Mesh wickMesh = wick.GetComponent<MeshFilter>().mesh;

        int main = lastLayerIndexStart;
        int layer = main / verticesPerLayer;

        int right, left;

        bool[] burnt = new bool[verticesPerLayer];

        while (layer > 0) {
            for (int i = 0; i < verticesPerLayer; i++) {
                burnt[i] = false;
            }

            while (!layerBurnt) {
                main = UnityEngine.Random.Range(0, verticesPerLayer);

                while (burnt[main]) {
                    main = (main + 1) % verticesPerLayer;
                }

                main = main + (layer * verticesPerLayer);

                right = ((main + 1) % verticesPerLayer) + (verticesPerLayer * layer);
                left = ((main - 1) % verticesPerLayer) + (verticesPerLayer * layer);

                while (newVertices[main].y >= newVertices[lastLayerIndexStart - verticesPerLayer].y) {
                    if (!middleBurnt) {
                        // Burn Middle Part
                        newVertices[lastIndex] = newVertices[lastIndex] + new Vector3(0, -burnValue, 0);
                    }
                    // Burn "Main" Outer Part
                    newVertices[main] = newVertices[main] + new Vector3(0, -burnValue, 0);
                    // Burn Part Right of "Main" Part
                    if ((newVertices[right] + new Vector3(0, -(burnValue / 2), 0)).y > newVertices[main - verticesPerLayer].y) {
                        newVertices[right] = newVertices[right] + new Vector3(0, -(burnValue / 2), 0);
                    }
                    // Burn Part Left of "Main" Part
                    if ((newVertices[left] + new Vector3(0, -(burnValue / 2), 0)).y > newVertices[main - verticesPerLayer].y) {
                        newVertices[left] = newVertices[left] + new Vector3(0, -(burnValue / 2), 0);
                    }
                    /*if (lastLayerIndexStart % verticesPerLayer == 0) {
                        if ((newVertices[(lastLayerIndexStart + verticesPerLayer) - 1] + new Vector3(0, -(burnValue / 2), 0)).y > newVertices[lastLayerIndexStart - verticesPerLayer].y) {
                            newVertices[(lastLayerIndexStart + verticesPerLayer) - 1] = newVertices[(lastLayerIndexStart + verticesPerLayer) - 1] + new Vector3(0, -(burnValue / 2), 0);
                        }
                    } else {
                    }*/

                    //Debug.Log((wickMesh.bounds.size.y * wick.transform.localScale.y * candle.transform.localScale.y) - (0.05f) + " " + (((candleMesh.bounds.size.y * candle.transform.localScale.y) / (layers - 1)) * (layer - 1)));
                    if (((wickMesh.bounds.size.y * wick.transform.localScale.y * candle.transform.localScale.y) - 0.05f ) > ((candleMesh.bounds.size.y / (layers - 1)) * (layer - 1)) * candle.transform.localScale.y) {
                        wickBurnt = ((candleMesh.bounds.size.y / (layers - 1)) * burnValue * ((burnTimeRate * 10) * 2.3f)) / verticesPerLayer;
                        wick.transform.localScale -= new Vector3(0, wickBurnt, 0);
                        wick.transform.Translate(0, -wickBurnt * candle.transform.localScale.y, 0);
                    }

                    candleMesh.vertices = newVertices;
                    candleMesh.RecalculateNormals();
                    yield return new WaitForSeconds(burnTimeRate);
                }
                middleBurnt = true;
                burnt[main % verticesPerLayer] = true;

                for (int i = 0; i < verticesPerLayer; i++) {
                    if (burnt[i] == false) {
                        layerBurnt = false;
                        break;
                    } else {
                        layerBurnt = true;
                    }
                }

                // Fix lighting / normals
                int[] tris = candleMesh.triangles;
                for (int i = 0; i < tris.Length; i++) {
                    if (tris[i] == main) tris[i] = (main - verticesPerLayer);
                }
                candleMesh.triangles = tris;
                candleMesh.RecalculateNormals();
            }

            /*for (int i = 0; i < verticesPerLayer; i++) {
                newVertices[(((layer - 1) * verticesPerLayer) + i)] = newVertices[(layer * verticesPerLayer) + i];
            }*/

            newVertices[layer * verticesPerLayer] = newVertices[lastIndex];
            lastIndex = layer * verticesPerLayer;

            layers--;
            layer--;
            lastLayerIndexStart -= verticesPerLayer;
            middleBurnt = false;
            layerBurnt = false;

            Array.Resize(ref newVertices, newVertices.Length - verticesPerLayer);
            List<int> faces = new List<int>();
            CreateFaces(faces);

            candleMesh.triangles = faces.ToArray();
            candleMesh.vertices = newVertices;

        }
    }
}
