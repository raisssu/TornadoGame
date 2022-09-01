using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : MonoBehaviour
{
    private void OnBecameInvisible() {
        enabled = false;
    }
    private void OnBecameVisible() {
        enabled = true;
    }
    public float treeHealth = 10f; 
    [SerializeField] Image foreground = null; 
    
    public void ChopTree(float damage) { 
        treeHealth -= damage;
        foreground.fillAmount = treeHealth/10f;
        
        if(treeHealth <= 0){
            Destroy(gameObject);
        }
    }
    
}
