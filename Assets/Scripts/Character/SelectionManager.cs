using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    
    [SerializeField] private string tag = "Tree";
    [SerializeField] public GameObject interactText;
    [SerializeField] private float damage = 1f;
    public float reach = 8f;
    public KeyCode interactKey = KeyCode.E;
    private TreeManager treeManager;
    private GameObject healthBar;
    
    
    // Update is called once per frame
    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit)){
            Transform selection = hit.transform;
            GameObject obj = hit.transform.gameObject;  
            Transform hb = obj.transform.Find("Canvas");
            if(hb!=null){
                healthBar = hb.gameObject;
            }
                
            
            if(selection.CompareTag(tag)){
                if (hit.distance < reach){
                    interactText.SetActive(true);
                    if(healthBar!=null){
                        healthBar.SetActive(true);
                    }
                    if(Input.GetKeyDown(interactKey)){
                        selection.GetComponent<TreeManager>().ChopTree(damage);
                    }
                }
                else{
                    healthBar.SetActive(false);
                    interactText.SetActive(false);
                }
            }
        }
        
    }
}
