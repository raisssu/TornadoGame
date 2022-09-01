using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHealthBar : MonoBehaviour
{
    public float rotateSpeed = 20f;
    public void Update() {
        // transform.RotateAround(transform.parent.position, new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    }
}
