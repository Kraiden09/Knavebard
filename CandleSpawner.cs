using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleSpawner : MonoBehaviour {
    taverne tavern;
    GameObject candleLight;
    float height, wickHeight;
    bool initialized, wallLocated;
    Vector3[][] newVertices;
    //Mesh candleMesh;

    readonly int verticesPerLayer = 20;
    readonly int layers = 5;
    // Needs to be an even number
    readonly int numberOfCandles = 6;

    Light lightComp;

    Vector3[] posWallLeft;
    Vector3[] posWallRight;

    int[] lastLayerIndexStart;
    ParticleSystem[] particleSystems;
    GameObject[] candles, wicks, candleHolders;
    Mesh[] candleMeshs;
    bool[] isLit;
    Light[] lights;

    // Use this for initialization
    void Start() {
        initialized = false;
        wallLocated = false;
        tavern = GameObject.Find("Tavern").GetComponent<taverne>();
        StartCoroutine(GetWallLeft());
        StartCoroutine(GetWallRight());
        //verticesPerLayer = 20;
        //layers = 5;
        height = 2f;
        // One half for the left wall, the other half for the right wall
        //numberOfCandles = 2;
        particleSystems = new ParticleSystem[numberOfCandles];
        candles = new GameObject[numberOfCandles];
        wicks = new GameObject[numberOfCandles];
        candleHolders = new GameObject[numberOfCandles];
        candleMeshs = new Mesh[numberOfCandles];
        newVertices = new Vector3[numberOfCandles][];
        lastLayerIndexStart = new int[numberOfCandles];
        isLit = new bool[numberOfCandles];
        lights = new Light[numberOfCandles];
        InitStart();
    }

    // Update is called once per frame
    void Update() {
    }

    void InitStart() {
        InitCandles();
        while (!initialized && !wallLocated) {
            // Wait until initialized
        }
        for (int i = 0; i < numberOfCandles; i++) {
            StartCoroutine(BurnDown(candles[i], wicks[i], i));
        }
    }

    void InitCandles() {
        GameObject candle, wick;
        for (int i = 0; i < numberOfCandles; i++) {
            candle = new GameObject();
            wick = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wick.transform.parent = candle.transform;
            candle.name = "Candle" + (i + 1);
            wick.name = "Wick";
            wickHeight = (height + 0.35f);
            wick.transform.localScale = new Vector3(0.02f, wickHeight / 2, 0.02f);
            wick.transform.Translate(0, wickHeight / 2, 0);
            wick.GetComponent<Renderer>().material.color = new Color(0.44f, 0.31f, 0.18f);

            candleLight = new GameObject();
            candleLight.transform.parent = wick.transform;
            candleLight.name = "Candle Light";
            candleLight.transform.position = new Vector3(0, wickHeight, 0);
            lightComp = candleLight.AddComponent<Light>();
            lightComp.color = new Color(1, 0.8393834f, 0.4009434f);
            candleLight.AddComponent<LightScript>();

            candles[i] = candle;
            wicks[i] = wick;
            isLit[i] = true;
            lights[i] = lightComp;

            CreateVertices(candles[i], i);
            CreateBurnParticle(candles[i], wicks[i], i);
        }
        CreateCandleHolder();
    }

    private void CreateBurnParticle(GameObject candle, GameObject wick, int index) {
        // Create emitter
        GameObject emitter = new GameObject();
        emitter.name = "Emitter";
        emitter.transform.parent = wick.transform;
        emitter.transform.position = wick.transform.position;
        emitter.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        // Set emitter position to wick position
        emitter.transform.Translate(0, (wickHeight * candle.transform.localScale.y) / 2, 0);
        // Particle System for further customization
        ParticleSystem ps = emitter.AddComponent<ParticleSystem>();
        // Rotate emitter to generate particles upwards
        emitter.transform.Rotate(-90, 0, 0);
        // Set emission var for further customization
        var emission = ps.emission;
        // Set main var for further (general) customization
        var main = ps.main;
        // Particle lifetime
        float lifetime = 0.5f;
        main.startLifetime = lifetime;
        // Allows particles to be customized
        ParticleSystemRenderer emittedParticles = ps.GetComponent<ParticleSystemRenderer>();
        // Emitted Particles Mesh = Sphere
        emittedParticles.renderMode = ParticleSystemRenderMode.Mesh;
        emittedParticles.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        emittedParticles.material = (Material)Resources.Load("Materials/NoteParticleMat");
        // Color Gradient for fading
        var col = ps.colorOverLifetime;
        col.enabled = true;
        Gradient grad = new Gradient();
        Color newColor = lightComp.color;
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(newColor, 0.0f), new GradientColorKey(newColor, 1.0f) },
                     new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        col.color = grad;
        emittedParticles.material.SetColor("_TintColor", newColor);
        main.startSize = 0.05f;
        main.startSpeed = 2;
        emission.enabled = true;
        // Emission Rate
        emission.rateOverTime = 70;

        particleSystems[index] = ps;

        ps.Play();
    }

    void CreateVertices(GameObject candle, int index) {
        newVertices[index] = new Vector3[verticesPerLayer * layers + 1];
        for (int j = 0; j < layers; j++) {
            for (int i = 0; i < verticesPerLayer; i++) {
                if (i == 0) lastLayerIndexStart[index] = i + (j * verticesPerLayer);
                //newVertices[i + (j * (verticesPerLayer + 1))] = new Vector3(0.5f, ((height / (layers - 1)) * (j - 1) + (height / (layers - 1))), 0f);
                newVertices[index][i + (j * verticesPerLayer)] = new Vector3(0.5f, ((height / (layers - 1)) * (j - 1) + (height / (layers - 1))), 0f);
            }
            //newVertices[((j + 1) * verticesPerLayer) + j] = new Vector3(0, ((height / (layers - 1)) * (j - 1) + (height / (layers - 1))), 0f);
        }
        newVertices[index][verticesPerLayer * layers] = new Vector3(0, ((height / (layers - 1)) * (layers - 2) + (height / (layers - 1))), 0f);

        // Rotate
        Quaternion rotation = Quaternion.Euler(0, 360 / verticesPerLayer, 0);
        for (int j = 0; j < newVertices[index].Length; j++) {
            for (int i = 0; i < (newVertices[index].Length - j); i++) {
                newVertices[index][i] = rotation * newVertices[index][i];
            }
        }

        // For Testing purposes
        /*for (int i = 0; i < newVertices[index].Length; i++) {
            GameObject test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            test.transform.position = newVertices[index][i];
            test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }*/

        List<int> faces = new List<int>();

        CreateFaces(faces, index, layers);

        candleMeshs[index] = new Mesh();
        candle.AddComponent<MeshFilter>();
        candle.AddComponent<MeshRenderer>();
        candle.GetComponent<MeshRenderer>().receiveShadows = true;
        candle.GetComponent<MeshFilter>().mesh = candleMeshs[index];
        candle.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);

        candleMeshs[index].vertices = newVertices[index];
        candleMeshs[index].triangles = faces.ToArray();

        candleMeshs[index].RecalculateNormals();

        candle.transform.localScale = new Vector3(0.359f, 0.359f, 0.359f);

        initialized = true;
    }

    void CreateFaces(List<int> faces, int index, int curLayers) {
        for (int j = 0; j < (curLayers - 1); j++) {
            for (int i = 0; i < verticesPerLayer; i++) {
                faces.Add(i + 1 + (j * verticesPerLayer));
                faces.Add(i + (j * verticesPerLayer));
                faces.Add(i + verticesPerLayer + (j * verticesPerLayer));

                faces.Add(i + verticesPerLayer + (j * verticesPerLayer));
                faces.Add(i + verticesPerLayer + 1 + (j * verticesPerLayer));
                faces.Add(i + 1 + (j * verticesPerLayer));

                if (j == (curLayers - 2) && i < (verticesPerLayer - 1)) {
                    faces.Add(i + verticesPerLayer + 1 + (j * verticesPerLayer));
                    faces.Add(i + verticesPerLayer + (j * verticesPerLayer));
                    faces.Add(newVertices[index].Length - 1);
                }
            }
            if (j == 0) {
                faces.Add(0 + (j * verticesPerLayer));
                faces.Add((verticesPerLayer - 1) + (j * verticesPerLayer));
                faces.Add(verticesPerLayer + (j * verticesPerLayer));
            }
        }
    }

    // For position of the candle holders and candles
    int numberOfCH;
    float distanceOfWalls, spaceBetween, correction;

    void CreateCandleHolder() {
        float wallLen = Math.Abs(posWallLeft[0].x + posWallLeft[2].x);
        distanceOfWalls = Math.Abs(posWallLeft[0].x - posWallRight[2].x);
        float outerMargin = 2.2f;
        // Correction - Candle Holder closer to wall
        correction = -0.264f;
        float hangingHeight = 2.719f;
        // Number of CandleHolder
        numberOfCH = numberOfCandles / 2;
        if (numberOfCH > 1) {
            spaceBetween = (wallLen - (2 * outerMargin)) / (numberOfCH - 1);
        } else {
            spaceBetween = 0;
        }

        for (int i = 0; i < numberOfCH; i++) {
            candleHolders[i] = (GameObject)Instantiate(Resources.Load("Prefab/CandleHolder"));
            candleHolders[i].transform.position = posWallLeft[0] + new Vector3(correction, hangingHeight, outerMargin);
            candleHolders[i].transform.Translate(0, 0, spaceBetween * i);
            candleHolders[i].name = "Candle Holder" + (i + 1);

            candleHolders[i + numberOfCH] = (GameObject)Instantiate(Resources.Load("Prefab/CandleHolder"));
            candleHolders[i + numberOfCH].transform.position = candleHolders[i].transform.position;
            candleHolders[i + numberOfCH].transform.Translate(distanceOfWalls - (correction * 2), 0, 0);
            candleHolders[i + numberOfCH].transform.Rotate(0, 180, 0);
            candleHolders[i + numberOfCH].name = "Candle Holder" + (i + numberOfCH + 1);

            //AddMaterialCH(candleHolders[i]);
            //AddMaterialCH(candleHolders[i].transform.GetChild(0).gameObject);
            //AddMaterialCH(candleHolders[i + numberOfCH].transform.GetChild(0).gameObject);
        }
        MoveCandles();
    }

    void AddMaterialCH(GameObject go) {
        Renderer rend = go.GetComponent<Renderer>();
        Material mat = Resources.Load<Material>("Materials/Steel");
        rend.material = mat;
        Mesh mesh = go.GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void MoveCandles() {
        for (int i = 0; i < (numberOfCandles / 2); i++) {
            candles[i].transform.position = candleHolders[i].transform.position;
            candles[i].transform.Translate(0.791f, 0.154f, 0.022f);

            candles[i + numberOfCH].transform.position = candleHolders[i + numberOfCH].transform.position;
            candles[i + numberOfCH].transform.Translate(-0.791f, 0.154f, -0.022f);
            candles[i + numberOfCH].transform.Rotate(0, 180, 0);
        }
    }

    public GameObject[] GetCandles() {
        return candles;
    }

    public bool IsInit() {
        return initialized;
    }

    public bool ToggleOnOff(int index) {
        bool newState = isLit[index];
        if (candles[index] != null) {
            newState = !newState;
            isLit[index] = newState;
            lights[index].enabled = newState;
            if (!newState) particleSystems[index].Stop();
            else particleSystems[index].Play();
        }
        return newState;
    }

    /*IEnumerator WaitForCandles() {
        for (int i = 0; i < numberOfCandles; i++) {
            Debug.Log(candles[i]);
        }
        while (candles[0] == null) {
            yield return new WaitForSeconds(0.05f);
        }
        candlesCreated = true;
    }*/

    IEnumerator GetWallLeft() {
        bool valid = false;
        while (!valid) {
            valid = true;
            try {
                posWallLeft = tavern.getWandLinksVert();
            } catch (NullReferenceException) {
                valid = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
        wallLocated = true;
    }

    IEnumerator GetWallRight() {
        bool valid = false;
        while (!valid) {
            valid = true;
            try {
                posWallRight = tavern.getWandRechtsVert();
            } catch (NullReferenceException) {
                valid = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
        wallLocated = true;
    }

    IEnumerator BurnDown(GameObject candle, GameObject wick, int index) {
        int lastIndex = newVertices[index].Length - 1;
        int curLayers = layers;

        float burnValue = 0.015f;
        float burnTimeRate = 0.1f;

        float wickBurnt;
        bool middleBurnt = false, layerBurnt = false;

        Mesh wickMesh = wick.GetComponent<MeshFilter>().mesh;

        int main = lastLayerIndexStart[index];
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

                while (newVertices[index][main].y >= newVertices[index][lastLayerIndexStart[index] - verticesPerLayer].y) {
                    if (isLit[index]) {
                        if (!middleBurnt) {
                            // Burn Middle Part
                            newVertices[index][lastIndex] = newVertices[index][lastIndex] + new Vector3(0, -burnValue, 0);
                        }
                        // Burn "Main" Outer Part
                        newVertices[index][main] = newVertices[index][main] + new Vector3(0, -burnValue, 0);
                        // Burn Part Right of "Main" Part
                        if ((newVertices[index][right] + new Vector3(0, -(burnValue / 2), 0)).y > newVertices[index][main - verticesPerLayer].y) {
                            newVertices[index][right] = newVertices[index][right] + new Vector3(0, -(burnValue / 2), 0);
                        }
                        // Burn Part Left of "Main" Part
                        if ((newVertices[index][left] + new Vector3(0, -(burnValue / 2), 0)).y > newVertices[index][main - verticesPerLayer].y) {
                            newVertices[index][left] = newVertices[index][left] + new Vector3(0, -(burnValue / 2), 0);
                        }
                        /*if (lastLayerIndexStart % verticesPerLayer == 0) {
                            if ((newVertices[(lastLayerIndexStart + verticesPerLayer) - 1] + new Vector3(0, -(burnValue / 2), 0)).y > newVertices[lastLayerIndexStart - verticesPerLayer].y) {
                                newVertices[(lastLayerIndexStart + verticesPerLayer) - 1] = newVertices[(lastLayerIndexStart + verticesPerLayer) - 1] + new Vector3(0, -(burnValue / 2), 0);
                            }
                        } else {
                        }*/

                        //Debug.Log((wickMesh.bounds.size.y * wick.transform.localScale.y * candle.transform.localScale.y) - (0.05f) + " " + (((candleMesh.bounds.size.y * candle.transform.localScale.y) / (layers - 1)) * (layer - 1)));
                        if (((wickMesh.bounds.size.y * wick.transform.localScale.y * candle.transform.localScale.y) - 0.05f) > ((candleMeshs[index].bounds.size.y / (curLayers - 1)) * (layer - 1)) * candle.transform.localScale.y) {
                            wickBurnt = ((candleMeshs[index].bounds.size.y / (curLayers - 1)) * burnValue * ((burnTimeRate * 10) * 2.3f)) / verticesPerLayer;
                            wick.transform.localScale -= new Vector3(0, wickBurnt, 0);
                            wick.transform.Translate(0, -wickBurnt * candle.transform.localScale.y, 0);
                        }
                        candleMeshs[index].vertices = newVertices[index];
                        candleMeshs[index].RecalculateNormals();
                    }
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
                int[] tris = candleMeshs[index].triangles;
                for (int i = 0; i < tris.Length; i++) {
                    if (tris[i] == main) tris[i] = (main - verticesPerLayer);
                }
                candleMeshs[index].triangles = tris;
                candleMeshs[index].RecalculateNormals();
            }

            /*for (int i = 0; i < verticesPerLayer; i++) {
                newVertices[(((layer - 1) * verticesPerLayer) + i)] = newVertices[(layer * verticesPerLayer) + i];
            }*/

            newVertices[index][layer * verticesPerLayer] = newVertices[index][lastIndex];
            lastIndex = layer * verticesPerLayer;

            curLayers--;
            layer--;
            lastLayerIndexStart[index] -= verticesPerLayer;
            middleBurnt = false;
            layerBurnt = false;

            Array.Resize(ref newVertices[index], newVertices[index].Length - verticesPerLayer);
            List<int> faces = new List<int>();
            CreateFaces(faces, index, curLayers);

            candleMeshs[index].triangles = faces.ToArray();
            candleMeshs[index].vertices = newVertices[index];
        }
        isLit[index] = false;
        // Set Candle Text to "Burned Out"
        // NRE?
        GameObject.Find("TriggerCandle" + (index + 1)).GetComponent<CandleTrigger>().SetIsLit(isLit[index]);
        particleSystems[index].Stop();
        Destroy(candles[index]);
    }
}
