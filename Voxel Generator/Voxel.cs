using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel
{
    public Mesh mesh        { get; set; }

    public Vector3 position { get; set; }
    public int index        { get; set; }
    public Chunk owner      { get; set; }

    public int id = 0;

    public bool drawn { get; set; }


    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uv;

    void GenerateUVS()
    {
        switch (id)
        {
            case 0:
                uv.Add(new Vector2(0f, 0.75f)); // bed rock 2
                uv.Add(new Vector2(0.25f, 0.75f));
                uv.Add(new Vector2(0f, 1f));
                uv.Add(new Vector2(0.25f, 1f));
                break;
            //0.75,0.75|1,0.75|0.75,1|1,1
            case 1:
                uv.Add(new Vector2(0.75f, 0.75f)); // stone 2
                uv.Add(new Vector2(1f, 0.75f));
                uv.Add(new Vector2(0.75f, 1f));
                uv.Add(new Vector2(1f, 1f));
                break;
            //0.25,0.75|0.5,0.75|0.25,1|0.5,1
            case 2:
                uv.Add(new Vector2(0.25f, 0.75f)); // dirt 2
                uv.Add(new Vector2(0.5f, 0.75f));
                uv.Add(new Vector2(0.25f, 1f));
                uv.Add(new Vector2(0.5f, 1f));
                break;
            //0.5,0.75|0.75,0.75|0.5,1|0.75,1
            case 3:
                uv.Add(new Vector2(0.5f, 0.75f)); // sand 2
                uv.Add(new Vector2(0.75f, 0.75f));
                uv.Add(new Vector2(0.5f, 1f));
                uv.Add(new Vector2(0.75f, 1f));
                break;
            default:
                uv.Add(new Vector2(0f, 0.75f)); // bed rock 2
                uv.Add(new Vector2(0.25f, 0.75f));
                uv.Add(new Vector2(0f, 1f));
                uv.Add(new Vector2(0.25f, 1f));
                break;
        }


    }

    void GenerateTriangles()
    {
        //Top right triangle
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count - 3);
        
        //Bottom left triangle
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 3);
    }

    // Create face of voxel
    void CF_front()
    {
        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(1, 0, 0));
        vertices.Add(position + new Vector3(0, 1, 0));
        vertices.Add(position + new Vector3(1, 1, 0));
        GenerateTriangles();
        GenerateUVS();
    }
    void CF_back()
    {
        vertices.Add(position + new Vector3(0, 1, 1));
        vertices.Add(position + new Vector3(1, 1, 1));
        vertices.Add(position + new Vector3(0, 0, 1));
        vertices.Add(position + new Vector3(1, 0, 1));
        GenerateTriangles();
        GenerateUVS();
    }
    void CF_left()
    {
        vertices.Add(position + new Vector3(0, 0, 1));
        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(0, 1, 1));
        vertices.Add(position + new Vector3(0, 1, 0));
        GenerateTriangles();
        GenerateUVS();
    }
    void CF_right()
    {
        vertices.Add(position + new Vector3(1, 0, 0));
        vertices.Add(position + new Vector3(1, 0, 1));
        vertices.Add(position + new Vector3(1, 1, 0));
        vertices.Add(position + new Vector3(1, 1, 1));
        GenerateTriangles();
        GenerateUVS();
    }
    void CF_top()
    {
        vertices.Add(position + new Vector3(0, 1, 0));
        vertices.Add(position + new Vector3(1, 1, 0));
        vertices.Add(position + new Vector3(0, 1, 1));
        vertices.Add(position + new Vector3(1, 1, 1));
        GenerateTriangles();
        GenerateUVS();
    }
    void CF_bottom()
    {
        vertices.Add(position + new Vector3(0, 0, 1));
        vertices.Add(position + new Vector3(1, 0, 1));
        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(1, 0, 0));
        GenerateTriangles();
        GenerateUVS();
    }



    public void GenerateFaces()
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        if(!owner.VoxelExists(x, y, z - 1)) 
            CF_front();
        if(!owner.VoxelExists(x, y, z + 1))
            CF_back();
        if(!owner.VoxelExists(x - 1, y, z))
            CF_left();
        if (!owner.VoxelExists(x + 1, y, z))
            CF_right();
        if (!owner.VoxelExists(x, y + 1, z))
            CF_top();
        if(!owner.VoxelExists(x, y - 1, z))
            CF_bottom();

        return;
    }

    //TODO: make drawn better
    public void Draw()
    {
        drawn = false;
        GenerateFaces();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        drawn = true;
    }

    public Voxel(Vector3 _position, int _id, Chunk _owner)
    {
        owner = _owner;
        position = _position;
        //index = _index;
        id = _id;

        mesh = new Mesh();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uv = new List<Vector2>();
    }
}