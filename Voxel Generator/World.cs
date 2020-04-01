using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Main
{
    public const int CHUNKSIZE = 24;
    public const int WORLDSIZE = 8;
    public const int RENDERDISTANCE = 6;
    public static int seed = 0;

    public static GameObject player;
    public static Vector3 player_location = Vector3.zero;

    public static World current_world;

    public static bool debug = false;

    public static GameObject CHUNKTEMPLATE = Resources.Load("OBJ/CHUNKTEMPLATE") as GameObject;

    public static float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6f;
    }

    static Main()
    {
        QualitySettings.vSyncCount = 0;
        seed = Random.Range(0, 999999);
    }
}

public class World : MonoBehaviour
{
    public Chunk[,] chunks;
    public bool generatingWorld = false;
    GameObject chunkContainer;

    public GameObject player;
    public Vector3 player_location;

    IEnumerator GenerateFixedWorld()
    {
        generatingWorld = true;
        Main.seed = Random.Range(0, 999999);

        if (chunkContainer != null)
            Destroy(chunkContainer);

        chunkContainer = new GameObject();
        chunkContainer.name = "World [" + (Main.WORLDSIZE * Main.WORLDSIZE) + " chunks @ ~" + (Main.CHUNKSIZE * Main.CHUNKSIZE) +  " vx/ch]";
        
        foreach (Chunk chunk in chunks)
            Destroy(chunk);

        for (int x = 0; x < Main.WORLDSIZE; x++)
        {
            for (int z = 0; z < Main.WORLDSIZE; z++)
            {
                GameObject chunkObject = Instantiate(Main.CHUNKTEMPLATE);
                chunkObject.name = "ChunkObject @ " + x + ", " + z;
                chunkObject.transform.parent = chunkContainer.transform;

                Chunk chunk = chunkObject.GetComponent<Chunk>();
                chunk.index = new Vector3Int(x, 0, z);
                chunks[x, z] = chunk;
            }
        }

        yield return new WaitForFixedUpdate();

        foreach (Chunk chunk in chunks)
        {
            chunk.GenerateVoxels();
            yield return new WaitUntil(() => chunk.chunkGenerated == true);
        }

        foreach (Chunk chunk in chunks)
        {
            chunk.DrawVoxels();
            yield return new WaitUntil(() => chunk.chunkDrawn == true);
            //TODO: Find a better way???
            chunk.gameObject.transform.position = new Vector3(chunk.index.x * Main.CHUNKSIZE, 0, chunk.index.z * Main.CHUNKSIZE);
        }

        generatingWorld = false;
    }
    IEnumerator GenerateWorld()
    {
        chunkContainer = new GameObject();
        chunkContainer.name = "World [" + (Main.WORLDSIZE * Main.WORLDSIZE) + " chunks @ ~" + (Main.CHUNKSIZE * Main.CHUNKSIZE) + " vx/ch]";

        generatingWorld = true;
        while (true)
        {
            //foreach (Chunk chunk in chunks)
            //{
            //    if (chunk != null)
            //    {
            //        if
            //        ((chunk.index.x > player_location.x + Main.RENDERDISTANCE) ||
            //        (chunk.index.x < player_location.x - Main.RENDERDISTANCE) ||
            //        (chunk.index.z > player_location.z + Main.RENDERDISTANCE) ||
            //        (chunk.index.z < player_location.z - Main.RENDERDISTANCE))
            //            chunk.gameObject.SetActive(false);
            //        else chunk.gameObject.SetActive(true);
            //    }
            //}
            for (int x = (int)player_location.x - Main.RENDERDISTANCE; x < (int)player_location.x + Main.RENDERDISTANCE; x++)
            {
                for (int z = (int)player_location.z - Main.RENDERDISTANCE; z < (int)player_location.z + Main.RENDERDISTANCE; z++)
                {
                    if((x < Main.WORLDSIZE && z < Main.WORLDSIZE) && (x > 0 && z > 0))
                    {
                        if (chunks[x, z] == null)
                        {
                            GameObject chunkObject = Instantiate(Main.CHUNKTEMPLATE);
                            chunkObject.name = "ChunkObject @ " + x + ", " + z;
                            chunkObject.transform.parent = chunkContainer.transform;

                            Chunk chunk = chunkObject.GetComponent<Chunk>();
                            chunk.index = new Vector3Int(x, 0, z);
                            chunks[x, z] = chunk;
                        }
                    }
                }
            }

            yield return new WaitForFixedUpdate();
            foreach (Chunk chunk in chunks)
            {
                if(chunk != null)
                {
                    if(chunk.chunkGenerated == false)
                    {
                        chunk.GenerateVoxels();
                        yield return new WaitUntil(() => chunk.chunkGenerated == true);
                    }
                }
            }
            foreach (Chunk chunk in chunks)
            {
                if (chunk != null)
                {
                    if (chunk.chunkDrawn == false)
                    {
                        chunk.DrawVoxels();
                        yield return new WaitUntil(() => chunk.chunkDrawn == true);
                        //TODO: Find a better way???

                        chunk.gameObject.transform.position = new Vector3(chunk.index.x * Main.CHUNKSIZE, 0, chunk.index.z * Main.CHUNKSIZE);
                    }
                }
            }

            generatingWorld = false;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        Main.seed = Random.Range(0, 999999);
        Main.player = player;

        chunks = new Chunk[Main.WORLDSIZE, Main.WORLDSIZE];
        Main.current_world = this;
        StartCoroutine(GenerateFixedWorld());
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
    //    player_location = player.transform.position;
    //    player_location.x = Mathf.RoundToInt(player_location.x / Main.CHUNKSIZE);
    //    player_location.z = Mathf.RoundToInt(player_location.z / Main.CHUNKSIZE);

    //    Main.player_location = player_location;

    //    if (generatingWorld == false)
    //    {
    //        for (int x = (int)player_location.x - Main.RENDERDISTANCE; x < (int)player_location.z + Main.RENDERDISTANCE; x++)
    //        {
    //            for (int z = (int)player_location.z - Main.RENDERDISTANCE; z < (int)player_location.z + Main.RENDERDISTANCE; z++)
    //            {
    //                if ((x > 0 && z > 0) && (x < Main.WORLDSIZE && z < Main.WORLDSIZE))
    //                {
    //                    if (chunks[x, z] != null)
    //                        if (chunks[x, z].gameObject.activeSelf == false)
    //                            chunks[x, z].gameObject.SetActive(true);
    //                }
    //            }
    //        }
    //    }

        if (Input.GetKeyDown(KeyCode.P))
            if(generatingWorld == false) StartCoroutine(GenerateFixedWorld());
    }
}
