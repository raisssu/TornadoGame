using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        
        if(DrawDefaultInspector()){
            if(mapGen.autoUpdate){
                mapGen.GenerateMap();
                mapGen.CreateMesh();
            }
        }
        if(GUILayout.Button("Generate")){
            mapGen.GenerateMap(); 
        }
    }
}

[CustomEditor(typeof(ObjectGenerator))]
public class ObjectGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectGenerator objGen = (ObjectGenerator)target;
        DrawDefaultInspector();
        
        if (GUILayout.Button("Generate")){
            objGen.SpawnObjects();
        }
        
        if (GUILayout.Button("Clear")){
            objGen.ClearObjects();
        }
    }
}