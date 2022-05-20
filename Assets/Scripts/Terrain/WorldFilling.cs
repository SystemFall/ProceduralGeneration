using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFilling : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject grassPrefab;

    public void EnvironmentFill(Transform mesh, Vector3[] vertices)
    {
        FillWithGrass(mesh.GetChild(0), vertices);
        FillWithTrees(mesh.GetChild(1), vertices);
    }
    private void FillWithGrass(Transform mesh, Vector3[] vertices)
    {
        foreach (Vector3 vertice in vertices)
        {
            if (vertice.y > -3 && vertice.y < 3 && Random.Range(0, 100) < 50)
            {
                Vector3 newVertice = new Vector3(vertice.x, vertice.y + .25f, vertice.z);
                Transform grass = Instantiate(grassPrefab, newVertice, Quaternion.identity).transform;
                grass.parent = mesh;
            }
        }
        CombineMeshes(mesh);
    }
    private void FillWithTrees(Transform mesh, Vector3[] vertices)
    {
        foreach (Vector3 vertice in vertices)
        {
            if (vertice.y > -3 && vertice.y < 3 && Random.Range(0, 100) < 3)
            {
                Vector3 newVertice = new Vector3(vertice.x, vertice.y - .2f, vertice.z);
                Transform tree = Instantiate(treePrefab, newVertice, Quaternion.identity).transform;
                tree.parent = mesh;
            }
        }
        CombineMeshes(mesh);
    }
    private void CombineMeshes(Transform mesh)
    {
        MeshFilter[] meshFilters = mesh.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        mesh.GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        mesh.gameObject.SetActive(true);
    }
}