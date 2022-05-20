using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public GameObject meshPrefab;
    public Transform meshContainer;
    public Gradient gradient;

    private Mesh mesh;

    int[] _triangles;
    Color[] _colors;

    public MeshFilter BuildMesh(int mapSize, Vector3[] vertices)
    {
        mesh = new Mesh();
        MeshFilter meshFilter = Instantiate(meshPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<MeshFilter>();
        meshFilter.transform.parent = meshContainer;
        meshFilter.GetComponent<MeshCollider>().sharedMesh = mesh;
        meshFilter.mesh = mesh;

        _triangles = new int[mapSize * mapSize * 6];

        _colors = new Color[vertices.Length];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                _triangles[tris] = vert;
                _triangles[tris + 1] = vert + mapSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + mapSize + 1;
                _triangles[tris + 5] = vert + mapSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = _triangles;
        
        mesh.RecalculateNormals();
        return meshFilter;
    }
    private void UpdateMesh(Vector3[] _vertices)
    {
        mesh.Clear();

        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.colors = _colors;

        mesh.RecalculateNormals();
    }
}
