using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(Vector2Int size, float fallOffStart, float fallOffEnd){
        float[,] map = new float[size.x, size.y];
        
        for (int y = 0; y < size.y; y++){
            for (int x = 0; x < size.x; x++){
                
                Vector2 pos = new Vector2 ((float)x / size.x * 2 - 1, (float)y / size.y * 2 - 1);
                
                float t = Mathf.Max(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
                
                if (t < fallOffStart){
                    map [x, y] = 1;
                }
                else if (t > fallOffEnd){
                    map [x, y] = 0;
                }
                else{
                    map[x, y] = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(fallOffStart, fallOffEnd, t));
                }
                
                // float x = i/(float)size * 2 - 1;
                // float y = j / (float)size * 2 - 1;
                
                // float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                // map[i, j] = Evaluate(value);
            }
        }
        return map;
    }
    
    static float Evaluate(float value){
        float a = 3;
        float b = 2.2f;
        
        return Mathf.Pow(value,a) / (Mathf.Pow(value, a) + Mathf.Pow(b- b * value, a));
    }
}
