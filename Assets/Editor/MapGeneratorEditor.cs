using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SmoothAreaLoading))]
public class MapGeneratorEditor : Editor
{
    /*public override void OnInspectorGUI()
    {
        SmoothAreaLoading mapGen = (SmoothAreaLoading)target;

        if(DrawDefaultInspector())
        {
            if(mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }*/
}