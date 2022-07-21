using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Material material;
    public enum DrawMode{NoiseMap, ColorMap, Mesh, FalloffMap};
    public DrawMode drawMode;
    
    [Header("Generation Settings")]
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves; 
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;
    
    [Header("Falloff Settings")]
    public bool useFalloffMap;
    public Vector2Int fallOffMapSize;

    [Range(0, 1)]
    public float fallOffStart;
    [Range(0, 2)]
    public float fallOffEnd;    
    
    [Header("Mesh Generation")]
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    
    public bool autoUpdate;
    
    public TerrainType[] regions;
    
    float[,] falloffMap;
    
    GameObject mesh;
    
    void Awake() {
        falloffMap = FalloffGenerator.GenerateFalloffMap(fallOffMapSize, fallOffStart, fallOffEnd);
        
        CreateMesh();
    }
    
    public void GenerateMap(){
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        
        Color[] colorMap = new Color[mapWidth*mapHeight];
        for (int y = 0; y < mapHeight; y++){
            for (int x = 0; x < mapWidth; x++){
                if (useFalloffMap){
                    noiseMap [x, y] = Mathf.Clamp01(noiseMap[x,y] * falloffMap[x,y]);
                }
                float currentHeight = noiseMap[x,y];
                for (int i = 0; i < regions.Length; i++){
                    if (currentHeight <= regions[i].height){
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap){
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.Mesh){
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        else if (drawMode == DrawMode.FalloffMap){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(fallOffMapSize, fallOffStart, fallOffEnd)));
        }

    }
    
    void OnValidate() {
        if (mapWidth < 1){
            mapWidth = 1;
        }
        if (mapHeight < 1){
            mapHeight = 1;
        }
        if (lacunarity < 1){
            lacunarity = 1;
        }
        if (octaves < 0){
            octaves = 0;
        }
        
        falloffMap = FalloffGenerator.GenerateFalloffMap(fallOffMapSize, fallOffStart, fallOffEnd);
               
    }
    
    public void CreateMesh(){
        mesh = GameObject.Find("Mesh");
        if (!mesh.GetComponent<MeshCollider>()) {
            mesh.AddComponent<MeshCollider>();
        }
        else {
            DestroyImmediate(mesh.GetComponent<MeshCollider>());
            mesh.AddComponent<MeshCollider>();
        }
    }
    
}

[System.Serializable]
public struct TerrainType{
    public string name;
    public float height; 
    public Color color;  
}