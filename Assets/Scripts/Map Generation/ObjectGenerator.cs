using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    //make a list of script components, 
    public GameObject prefab;
    GameObject asset;
    public string tagName;

    public GameObject parentObject;

    [Header("Raycast Settings")]
    public int density;
    public int minHeight;
    public int maxHeight;
    [SerializeField]
    public Vector2 xRange;
    public Vector2 yRange;
    
    [Header("Spawn Modifications")]
    public Vector3 sizeScale = new Vector3(1,1,1);
    
    public float lowerPlacementValue;

    public void SpawnObjects()
    {
        for (int i = 0; i < density; i++)
        {
            Vector3 originPoint = RandomOrigin();
            Vector3 spawnPoint = Vector3.zero;

            Ray ray = new Ray(originPoint, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                while (!hit.collider.name.Equals("Mesh") || hit.point.y < minHeight || hit.point.y > maxHeight)
                {
                    originPoint = RandomOrigin();
                    ray = new Ray(originPoint, Vector3.down);

                    if (Physics.Raycast(ray, out hit) && hit.point.y > minHeight && hit.point.y < maxHeight)
                    {
                        spawnPoint = hit.point;
                        spawnPoint.y -= lowerPlacementValue;
                        asset = Instantiate(prefab, spawnPoint, Quaternion.Euler(new Vector3(-90, 0, 0)));
                        
                        asset.transform.parent = parentObject.transform;
                        asset.transform.localScale = Vector3.Scale(asset.transform.localScale, sizeScale);
                        asset.tag = tagName;
                    }
                }
            }
        }
    }

    public Vector3 RandomOrigin()
    {
        Vector3 origin = new Vector3(Random.Range(xRange.x, xRange.y), transform.position.y, Random.Range(yRange.x, yRange.y));
        return origin;
    }

    public void ClearObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject.DestroyImmediate(objects[i]);
        }
    }

}
