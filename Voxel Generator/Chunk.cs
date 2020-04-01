using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    MeshFilter meshFilter;
    Mesh mesh;
    MeshCollider mcollider;


    public Vector3Int index;
    public Voxel[,,] voxels;
    public bool chunkGenerated = false;
    public bool chunkDrawn = false;

    //List<CombineInstance> combineList;

    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "chunkMesh";
        voxels = new Voxel[Main.CHUNKSIZE, Main.CHUNKSIZE, Main.CHUNKSIZE];
        meshFilter = gameObject.GetComponent<MeshFilter>();
        mcollider = gameObject.AddComponent<MeshCollider>();
    }

    //private void FixedUpdate()
    //{
    //    if ((index.x > Main.player_location.x + (Main.RENDERDISTANCE)) ||
    //        (index.x < Main.player_location.x - (Main.RENDERDISTANCE)) ||
    //        (index.z > Main.player_location.z + (Main.RENDERDISTANCE)) ||
    //        (index.z < Main.player_location.z - (Main.RENDERDISTANCE)))
    //            gameObject.SetActive(false);
    //}

    public bool VoxelExists(int x, int y, int z)
    {
        if ((x < Main.CHUNKSIZE && x >= 0) && (y < Main.CHUNKSIZE && y >= 0) && (z < Main.CHUNKSIZE && z >= 0)) {
            return voxels[x, y, z] != null;
        }//FRONT
        else if ((z >= Main.CHUNKSIZE) && (index.z + 1 < Main.WORLDSIZE))
        {
            if (Main.current_world.chunks[index.x, index.z + 1] != null)
                return Main.current_world.chunks[index.x, index.z + 1].voxels[x, y, 0] != null;
        }//BACK
        else if ((z < 0) && (index.z - 1 >= 0))
        {
            if (Main.current_world.chunks[index.x, index.z - 1] != null)
                return Main.current_world.chunks[index.x, index.z - 1].voxels[x, y, Main.CHUNKSIZE - 1] != null;
        }//RIGHT
        else if ((x >= Main.CHUNKSIZE) && (index.x + 1 < Main.WORLDSIZE))
        {
            if (Main.current_world.chunks[index.x + 1, index.z] != null)
                return Main.current_world.chunks[index.x + 1, index.z].voxels[0, y, z] != null;
        }//LEFT
        else if ((x < 0) && (index.x - 1 >= 0))
        {
            if (Main.current_world.chunks[index.x - 1, index.z] != null)
                return Main.current_world.chunks[index.x - 1, index.z].voxels[Main.CHUNKSIZE - 1, y, z] != null;
        }

        return false;
    }
    public void DrawVoxels()
    {
        float startTime = Time.realtimeSinceStartup;
        
        if(chunkDrawn == false)
        {
            foreach (Voxel voxel in voxels)
                if(voxel != null)
                    if(voxel.drawn == false)
                        voxel.Draw();

            List<CombineInstance> combineList = new List<CombineInstance>();

            foreach (Voxel voxel in voxels)
            {
                if(voxel != null)
                {
                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.transform = gameObject.transform.localToWorldMatrix;
                    combineInstance.mesh = voxel.mesh;
                    combineList.Add(combineInstance);
                    Destroy(voxel.mesh);
                }
            }

            mesh.CombineMeshes(combineList.ToArray(), true, true);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;
            mcollider.sharedMesh = mesh;
            chunkDrawn = true;
        }

        if(Main.debug == true)
            print("TIME TO DRAW " + gameObject.name + ": " + (Time.realtimeSinceStartup - startTime) + " seconds");
    }
    public void GenerateVoxels()
    {
        float noiseScale = 0.0475f;
        //float noiseFrequency = .875f;
        float noiseThreshold = 0.475f;

        /*
         * 
             ((chunkIndex.x* World.chunk_size.x) + (World.seed + chunkIndex.x + x)) * World.noiseScale,

            float noiseScale = 0.0475f;
            float noiseFrequency = .0875f;
            float noiseThreshold = 0.475f;
         * 
         */
        float startTime = Time.realtimeSinceStartup;
        if(chunkGenerated == false)
        {
            float perlinNoise = 0.0f;
            for (int x = 0, i = 0; x < Main.CHUNKSIZE; x++) {
                for (int y = 0; y < Main.CHUNKSIZE; y++) {
                    for (int z = 0; z < Main.CHUNKSIZE; z++, i++)
                    {
                        perlinNoise = Mathf.RoundToInt( Mathf.PerlinNoise(
                                     (((index.x * Main.CHUNKSIZE) + (Main.seed + x)) * noiseScale),
                                     (((index.z * Main.CHUNKSIZE) + (Main.seed + z)) * noiseScale)) * Main.CHUNKSIZE);

                        if( y <= perlinNoise)
                        {
                            perlinNoise = Main.Perlin3D(
                                        (((index.x * Main.CHUNKSIZE) + (Main.seed +  x)) * noiseScale),// * noiseFrequency,
                                        (((index.y * Main.CHUNKSIZE) + (Main.seed +  y)) * noiseScale),// * noiseFrequency,
                                        (((index.z * Main.CHUNKSIZE) + (Main.seed +  z)) * noiseScale));// * noiseFrequency);

                            if (perlinNoise <= noiseThreshold)
                            {
                                Voxel voxel = new Voxel(new Vector3(x, y, z), Random.Range(0, 3), this);
                                voxels[x, y, z] = voxel;
                            }
                        }
                    }
                }
            }

            chunkGenerated = true;
        }

        if(Main.debug == true)
            print("TIME TO GENERATE " + gameObject.name + ": " + (Time.realtimeSinceStartup - startTime) + " seconds");
    }





    //TODO: Fix updating chunks to update voxels surrounding removed / updated voxel instead of whole chunk.
    // For loop interation to check parity?
    //      then drawn the faces
    public bool updating = false;
    public IEnumerator _UpdateChunk()
    {
        if (updating == false)
        {
            chunkDrawn = false;
            DrawVoxels();
            yield return new WaitUntil(() => chunkDrawn == true);
        }
        
        yield return new WaitForEndOfFrame();
    }
    public void UpdateChunk()
    {
        StartCoroutine(_UpdateChunk());
    }
}
