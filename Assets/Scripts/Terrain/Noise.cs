using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public static Vector3[] GenerateVerticesMap(int posX, int posZ, int mapSize, int seed, float scale, int octaves, float persistance, float lacunarity, float meshHeightMultiplier, Vector2 offset)
    {
        Vector3[] vertices = new Vector3[(mapSize + 1) * (mapSize + 1)];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0)
        {
            scale = .0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapSize / 2f;
        float halfHeight = mapSize / 2f;

        for(int h = 0, z = posZ; z <= posZ + mapSize; z++)
        {
            for(int x = posX; x <= posX + mapSize; x++)
            {
                float amplitude = 1.5f;
                float frequency = 1.5f;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (z - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if(noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                vertices[h++] = new Vector3(x, noiseHeight * meshHeightMultiplier, z);
            }
        }

        return vertices;
    }
}
