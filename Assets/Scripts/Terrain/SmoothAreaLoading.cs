using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SmoothAreaLoading : MonoBehaviour
{
    public Transform player;

    public int chunkWidth;
    public int chunkLength;
    public int chunkSize;
    public int chunkLoadingRadius;

    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;

    public bool autoUpdate;

    private List<MapData> mapData;

    private Vector2Int centerOfLoadedArea;
    private MeshGenerator meshGenerator;
    private WorldFilling worldFilling;

    private List<Vector2Int> chunksCoordinate;

    private void Start()
    {
        mapData = new List<MapData>();
        chunksCoordinate = new List<Vector2Int>();
        meshGenerator = FindObjectOfType<MeshGenerator>();
        worldFilling = GetComponent<WorldFilling>();

        player.GetComponent<PlayerMovement>().SetChunkSize(chunkSize);
    }
    private void UpdateLoadedArea()
    {
        for(int x = centerOfLoadedArea.x - chunkLoadingRadius + 1; x < centerOfLoadedArea.x + chunkLoadingRadius; x++)
        {
            float circleValue = Mathf.Sqrt(Mathf.Pow(chunkLoadingRadius, 2) - Mathf.Pow(x - centerOfLoadedArea.x, 2)) + centerOfLoadedArea.y;
            
            for (int y = centerOfLoadedArea.y; y < centerOfLoadedArea.y + chunkLoadingRadius; y++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                if (y < circleValue && !mapData.Exists(_x => _x.chunkCoords == coord))
                    chunksCoordinate.Add(coord);
            }
            for (int y = centerOfLoadedArea.y - chunkLoadingRadius + 1; y < centerOfLoadedArea.y; y++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                if (y > (-circleValue + centerOfLoadedArea.y * 2) && !mapData.Exists(_x => _x.chunkCoords == coord))
                    chunksCoordinate.Add(coord);
            }
        }
        if(chunksCoordinate.Count > 0)
            StartCoroutine(CreateMeshChunk(chunksCoordinate[0]));
    }
    private IEnumerator CreateMeshChunk(Vector2Int coords)
    {
        yield return new WaitForSeconds(.05f);

        Vector3[] vertices = Noise.GenerateVerticesMap(coords.x * chunkSize, coords.y * chunkSize, chunkSize, seed, noiseScale, octaves, persistance, lacunarity, meshHeightMultiplier, offset);

        mapData.Add(new MapData(meshGenerator.BuildMesh(chunkSize, vertices), coords));

        worldFilling.EnvironmentFill(mapData[mapData.Count - 1].meshFilter.transform, vertices);

        chunksCoordinate.Remove(coords);

        if(chunksCoordinate.Count > 0)
            StartCoroutine(CreateMeshChunk(chunksCoordinate[0]));
    }
    private IEnumerator RemoveUnloadedArea(int index)
    {
        yield return new WaitForSeconds(.01f);

        index++;
        if (Vector2.Distance(centerOfLoadedArea, mapData[index-1].chunkCoords) > chunkLoadingRadius)
        {
            Destroy(mapData[index-1].meshFilter.gameObject);
            mapData.Remove(mapData[index-1]);
            index = 0;
        }
        if (mapData.Count > index)
            StartCoroutine(RemoveUnloadedArea(index));
    }
    public void SetCenterOfLoadedArea(Vector2Int center)
    {
        Debug.Log("new center = " + center);
        centerOfLoadedArea = center;
        UpdateLoadedArea();
        if(mapData.Count > 0)
            StartCoroutine(RemoveUnloadedArea(0));
    }
}
public class MapData
{
    public MeshFilter meshFilter;
    public Vector2Int chunkCoords;

    public MapData(MeshFilter mesh, Vector2Int coords)
    {
        meshFilter = mesh;
        chunkCoords = coords;
    }
    public MeshFilter GetMesh()
    {
        return meshFilter;
    }
    public Vector2Int GetCoords()
    {
        return chunkCoords;
    }
}