using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReadTerrain))]
public class PCGEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ReadTerrain terrainReader = (ReadTerrain) target;

        if (GUILayout.Button("Spawn Trees"))
        {
            terrainReader.SpawnTrees();
        }
        if (GUILayout.Button("Save the Trees as prefab"))
        {
            terrainReader.SaveTrees();
        }
    }
}
