using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        //print(other.transform.tag);
    }
    private void OnTriggerEnter(Collider other) {
         //print(other.transform.tag);
    }
}
