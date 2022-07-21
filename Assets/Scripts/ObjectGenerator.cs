using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    // add deletion of existing trees to generate new set. 
    public GameObject prefab;
    GameObject tree;
    public string tagName;
    
    [Header("Raycast Settings")]
    public int density;
    public int minHeight;
    public int maxHeight;
    [SerializeField]
    public Vector2 xRange;
    public Vector2 yRange;
    
    public void SpawnObjects(){
        for (int i = 0; i < density; i++) {
            Vector3 originPoint = RandomOrigin();
            Vector3 spawnPoint = Vector3.zero;
            
            Ray ray = new Ray(originPoint, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                
                while (!hit.collider.name.Equals("Mesh") || hit.point.y < minHeight || hit.point.y > maxHeight) {
                    originPoint = RandomOrigin();
                    ray = new Ray(originPoint, Vector3.down);
                    
                    if (Physics.Raycast(ray, out hit) && hit.point.y > minHeight && hit.point.y < maxHeight) {
                        spawnPoint = hit.point;
                        tree = Instantiate(prefab, spawnPoint, Quaternion.Euler(new Vector3(-90, 0, 0)));
                        tree.tag = tagName;
                    }
                }
            }
        }
    }
    
    public Vector3 RandomOrigin(){
        Vector3 origin = new Vector3(Random.Range(xRange.x, xRange.y), transform.position.y, Random.Range(yRange.x, yRange.y));
        return origin;
    }
    
    public void ClearObjects(){
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);
        for (int i = 0; i < objects.Length; i++){
            GameObject.DestroyImmediate(objects[i]);
        }
    }
    
}
