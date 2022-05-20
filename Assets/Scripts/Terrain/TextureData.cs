using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureData : MonoBehaviour
{
    public Color[] baseColours;
    [Range(0, 1)]
    public float[] baseStartHeights;
    [Range(0, 1)]
    public float[] baseBlends;

    //float savedMinHeight;
    //float savedMaxHeight;

    public void ApplyToMaterial(Material material, float minHeight, float maxHeight)
    {
        material.SetInt("baseColourCount", baseColours.Length);
        material.SetColorArray("baseColours", baseColours);
        material.SetFloatArray("baseStartHeights", baseStartHeights);
        material.SetFloatArray("baseBlends", baseBlends);

        UpdateMeshHeight(material, minHeight, maxHeight);
    }

    public void UpdateMeshHeight(Material material, float minHeight, float maxHeight)
    {
        //savedMinHeight = minHeight;
        //savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }
}